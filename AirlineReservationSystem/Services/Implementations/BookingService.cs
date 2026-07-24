using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.BookingVM;

namespace AirlineReservationSystem.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Bookings
                .GetAllWithBasicInfoAsync(cancellationToken);
        }

        public async Task<BookingListVM> GetPagedListAsync( BookingFilterVM filter,CancellationToken cancellationToken = default)
        {

            var bookingsQuery = await _unitOfWork.Bookings.GetAllWithBasicInfoAsync(cancellationToken);

            // تطبيق الفلاتر
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                bookingsQuery = bookingsQuery.Where(b =>
                    b.BookingReference.Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase) ||
                    b.User != null && (b.User.FirstName + " " + b.User.LastName).Contains(filter.Keyword, StringComparison.OrdinalIgnoreCase)); // فرضنا إن CustomerName جاي من الـ User
            }

            if (filter.BookingStatus.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Status == filter.BookingStatus.Value);
            }

            if (filter.PaymentStatus.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Payment?.Status == filter.PaymentStatus.Value);
            }

            if (filter.FromDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.CreatedAt.Date >= filter.FromDate.Value.Date);
            }

            if (filter.ToDate.HasValue)
            {
                bookingsQuery = bookingsQuery.Where(b => b.CreatedAt.Date <= filter.ToDate.Value.Date);
            }

            // حساب العدد الإجمالي للفلاتر المطبقة
            var totalCount = bookingsQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

            // تطبيق الـ Pagination والـ Mapping للـ BookingIndexVM
            var bookingsVM = bookingsQuery
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(b => new BookingIndexVM
                {
                    Id = b.Id,
                    BookingReference = b.BookingReference,
                    CustomerName = b.User != null ? b.User.FullName : "N/A", 
                    FlightNumber = b.Flight?.FlightNumber ?? "N/A",
                    PassengersCount = b.BookingPassengers?.Count ?? 0,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status,
                    PaymentStatus = b.Payment?.Status,
                    CreatedAt = b.CreatedAt
                }).ToList();

            return new BookingListVM
            {
                Filter = filter,
                Bookings = bookingsVM,
                CurrentPage = filter.PageNumber,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
        }





        public async Task<Booking?> GetDetailsAsync(
            int bookingId,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Bookings
                .GetBookingWithDetailsAsync(bookingId, cancellationToken);
        }

        public async Task<IEnumerable<Booking>> GetByStatusAsync(
            BookingStatus status,
            CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.Bookings
                .GetBookingsByStatusAsync(status, cancellationToken);
        }

        public async Task ChangeStatusAsync(
            int bookingId,
            BookingStatus newStatus,
            CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings
                .GetByIdAsync(bookingId, cancellationToken);

            if (booking is null)
                throw new KeyNotFoundException("Booking not found.");

            if (booking.Status == newStatus)
                throw new InvalidOperationException(
                    "Booking is already in this status.");

            // لا يمكن تغيير حالة حجز ملغى
            if (booking.Status == BookingStatus.Cancelled)
                throw new InvalidOperationException(
                    "Cancelled booking cannot be modified.");

            booking.Status = newStatus;

            _unitOfWork.Bookings.Update(booking);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelAsync(
            int bookingId,
            CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings
                .GetBookingWithDetailsAsync(bookingId, cancellationToken);

            if (booking is null)
                throw new KeyNotFoundException("Booking not found.");

            if (booking.Status == BookingStatus.Cancelled)
                throw new InvalidOperationException(
                    "Booking is already cancelled.");

            /*
             * فى المستقبل:
             * إذا كان الدفع مكتمل
             * يتم إنشاء Refund قبل الإلغاء.
             *
             * if (booking.Payment?.Status == PaymentStatus.Paid)
             * {
             *     await _refundService.CreateRefundAsync(booking.Id);
             * }
             */

            booking.Status = BookingStatus.Cancelled;

            _unitOfWork.Bookings.Update(booking);

            await _unitOfWork.SaveChangesAsync();
        }

    }
}
