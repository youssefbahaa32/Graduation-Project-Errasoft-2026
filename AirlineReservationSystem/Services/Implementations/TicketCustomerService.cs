using AirlineReservationSystem.ViewModels.TicketCustomerVM;

namespace AirlineReservationSystem.Services.Implementations
{
    public class TicketCustomerService : ITicketCustomerService
    {
        private readonly IGenericRepository<Ticket> _ticketRepository;

        public TicketCustomerService(IGenericRepository<Ticket> ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<List<MyTicketItemVM>> GetMyTicketsAsync(string userId, CancellationToken cancellationToken = default)
        {

            var tickets = await _ticketRepository.GetAllAsync(
                expression: t => t.BookingPassenger.Booking.UserId == userId,
                includes: new Expression<Func<Ticket, object>>[]
                {
            t => t.BookingPassenger.Booking, 
            t => t.BookingPassenger.FlightSeat.Flight, 
            t => t.BookingPassenger.FlightSeat.Seat    
                },
                tracked: false,
                cancellationToken: cancellationToken
            );

            return tickets
                .OrderByDescending(t => t.IssuedAt)
                .Select(t => new MyTicketItemVM
                {
                    TicketId = t.Id,
                    TicketNumber = t.TicketNumber,
                    FlightNumber = t.BookingPassenger?.FlightSeat?.Flight?.FlightNumber ?? "N/A",
                    SeatNumber = t.BookingPassenger?.FlightSeat?.Seat?.SeatNumber ?? "N/A",
                    Status = t.Status,
                    IssuedAt = t.IssuedAt
                }).ToList();
        }


        public async Task<TicketDetailsCustomerVM?> GetTicketDetailsAsync(int ticketId, string userId, CancellationToken cancellationToken = default)
        {
            var ticket = await _ticketRepository.GetOneWithIncludesAsync(
                expression: t => t.Id == ticketId && t.BookingPassenger.Booking.UserId == userId,
                include: q => q
                    .Include(t => t.BookingPassenger).ThenInclude(bp => bp.Passenger)
                    .Include(t => t.BookingPassenger).ThenInclude(bp => bp.Booking)
                    .Include(t => t.BookingPassenger).ThenInclude(bp => bp.FlightSeat).ThenInclude(fs => fs.Flight)
                    .Include(t => t.BookingPassenger).ThenInclude(bp => bp.FlightSeat).ThenInclude(fs => fs.Seat),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (ticket is null) return null;

            return new TicketDetailsCustomerVM
            {
                TicketId = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Barcode = ticket.Barcode,
                Fare = ticket.Fare,
                Status = ticket.Status,
                IssuedAt = ticket.IssuedAt,
                PassengerFullName = $"{ticket.BookingPassenger.Passenger.FirstName} {ticket.BookingPassenger.Passenger.LastName}",
                BookingReference = ticket.BookingPassenger.Booking.BookingReference,
                FlightNumber = ticket.BookingPassenger.FlightSeat.Flight.FlightNumber,
                SeatNumber = ticket.BookingPassenger.FlightSeat.Seat.SeatNumber
            };
        }

    }
}
