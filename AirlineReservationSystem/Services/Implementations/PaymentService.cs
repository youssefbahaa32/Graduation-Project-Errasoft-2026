using AirlineReservationSystem.Services.Interfaces;

namespace AirlineReservationSystem.Services.Implementations
{
    public class PaymentService: IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IGenericRepository<Payment> paymentRepo,
            IGenericRepository<Booking> bookingRepo,
            IUnitOfWork unitOfWork)
        {
            _paymentRepo = paymentRepo;
            _bookingRepo = bookingRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<Payment> Items, int TotalCount)> GetAllPaymentsAsync(
            string? searchTransactionRef = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            Expression<Func<Payment, bool>> filter = string.IsNullOrWhiteSpace(searchTransactionRef)
                ? p => !p.IsDeleted
                : p => !p.IsDeleted && p.TransactionReference.Contains(searchTransactionRef);

            var totalCount = await _paymentRepo.CountAsync(filter, ct);

            var all = await _paymentRepo.GetAllAsync(
                expression: filter,
                includes: new Expression<Func<Payment, object>>[] { p => p.Booking },
                tracked: false,
                cancellationToken: ct);

            var items = all
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalCount);
        }

        public async Task<Payment?> GetDetailsAsync(int id, CancellationToken ct = default)
            => await _paymentRepo.GetOneAsync(
                expression: p => p.Id == id && !p.IsDeleted,
                includes: new Expression<Func<Payment, object>>[] { p => p.Booking },
                tracked: false,
                cancellationToken: ct);

        public async Task<Payment?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _paymentRepo.GetOneAsync(
                expression: p => p.Id == id && !p.IsDeleted,
                tracked: false,
                cancellationToken: ct);

        public async Task AddPaymentAsync(Payment payment, CancellationToken ct = default)
        {
            if (payment.Amount <= 0)
                throw new InvalidOperationException("Amount must be greater than zero.");

            var bookingExists = await _bookingRepo.ExistsAsync(b => b.Id == payment.BookingId, ct);
            if (!bookingExists)
                throw new InvalidOperationException("Selected booking does not exist.");

            var bookingHasPayment = await _paymentRepo.ExistsAsync(
                p => p.BookingId == payment.BookingId && !p.IsDeleted, ct);
            if (bookingHasPayment)
                throw new InvalidOperationException("This booking already has a payment.");

            var isDuplicateRef = await _paymentRepo.ExistsAsync(
                p => p.TransactionReference == payment.TransactionReference && !p.IsDeleted, ct);
            if (isDuplicateRef)
                throw new InvalidOperationException("This transaction reference is already used.");

            payment.Status = PaymentStatus.Pending;
            await _paymentRepo.AddAsync(payment, ct);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePaymentAsync(Payment payment, CancellationToken ct = default)
        {
            var existing = await _paymentRepo.GetByIdAsync(payment.Id, ct);
            if (existing is null || existing.IsDeleted)
                throw new KeyNotFoundException($"Payment with id {payment.Id} was not found.");

            if (payment.Amount <= 0)
                throw new InvalidOperationException("Amount must be greater than zero.");

            var bookingExists = await _bookingRepo.ExistsAsync(b => b.Id == payment.BookingId, ct);
            if (!bookingExists)
                throw new InvalidOperationException("Selected booking does not exist.");

            if (!string.Equals(payment.TransactionReference, existing.TransactionReference, StringComparison.Ordinal))
            {
                var isDuplicateRef = await _paymentRepo.ExistsAsync(
                    p => p.TransactionReference == payment.TransactionReference
                         && !p.IsDeleted
                         && p.Id != payment.Id, ct);

                if (isDuplicateRef)
                    throw new InvalidOperationException("This transaction reference is already used by another payment.");
            }

            existing.BookingId = payment.BookingId;
            existing.Amount = payment.Amount;
            existing.PaymentMethod = payment.PaymentMethod;
            existing.Status = payment.Status;
            existing.TransactionReference = payment.TransactionReference;

            _paymentRepo.Update(existing);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync(int id, CancellationToken ct = default)
        {
            var payment = await _paymentRepo.GetByIdAsync(id, ct);
            if (payment is null || payment.IsDeleted)
                throw new KeyNotFoundException($"Payment with id {id} was not found.");

            payment.IsDeleted = true;
            _paymentRepo.Update(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetBookingsForLookupAsync(
         int? excludeCurrentBookingId = null,
         CancellationToken ct = default)
        {
            var bookings = await _bookingRepo.GetAllAsync(
                expression: b => b.Payment == null || b.Id == excludeCurrentBookingId,
                tracked: false,
                cancellationToken: ct);

            return bookings.ToList();
        }
    }
}
