namespace AirlineReservationSystem.Services.BackgroundJobs
{
    public class ExpiredBookingsCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredBookingsCleanupService> _logger;
        private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1); // بيفحص كل دقيقة

        public ExpiredBookingsCleanupService(
            IServiceProvider serviceProvider,
            ILogger<ExpiredBookingsCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupExpiredBookingsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while cleaning up expired bookings.");
                }

                // يستريح لمدة دقيقة قبل الفحص التالي
                await Task.Delay(CheckInterval, stoppingToken);
            }
        }

        private async Task CleanupExpiredBookingsAsync(CancellationToken cancellationToken)
        {
            // بنعمل Scope جديد لأن الـ DbContext والـ UnitOfWork مسجلين كـ Scoped 
            // والـ BackgroundService بيفضل عايش Singleton طول ما السيرفر شغال
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var now = DateTime.UtcNow;

            // 1. جلب الحجوزات الـ Pending اللي وقت الـ Reservation بتاعها انتهى
            var expiredBookings = await unitOfWork.Bookings.GetAllAsync(
                expression: b => b.Status == BookingStatus.Pending
                    && b.BookingPassengers.Any(bp => bp.ReservedUntil != null && bp.ReservedUntil <= now),
                includes: null,
                tracked: true,
                cancellationToken: cancellationToken
            );

            var expiredBookingsList = expiredBookings.ToList();

            if (expiredBookingsList.Count == 0)
                return;

            foreach (var booking in expiredBookingsList)
            {
                // 2. جلب تفاصيل الحجز كاملة بالـ Includes عشان نوصل للكراسي
                var bookingWithDetails = await unitOfWork.Bookings.GetOneWithIncludesAsync(
                    expression: b => b.Id == booking.Id,
                    include: q => q.Include(b => b.BookingPassengers).ThenInclude(bp => bp.FlightSeat),
                    tracked: true,
                    cancellationToken: cancellationToken
                );

                if (bookingWithDetails is null) continue;

                // تحويل الحجز لـ Expired
                bookingWithDetails.Status = BookingStatus.Expired;
                unitOfWork.Bookings.Update(bookingWithDetails);

                // . تحرير الكراسي وإرجاعها Available في الـ Seat Map
                foreach (var bp in bookingWithDetails.BookingPassengers)
                {
                    if (bp.FlightSeat != null)
                    {
                        bp.FlightSeat.Status = FlightSeatStatus.Available;
                        unitOfWork.FlightSeats.Update(bp.FlightSeat);
                    }
                    bp.ReservedUntil = null; // مسح التوقيت
                }
            }

            // حفظ كل التغييرات في قاعدة البيانات دفعة واحدة
            await unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully expired {Count} booking(s) and released their seats.", expiredBookingsList.Count);
        }

    }
}
