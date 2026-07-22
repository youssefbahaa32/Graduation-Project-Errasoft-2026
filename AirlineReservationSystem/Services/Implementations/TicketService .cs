using AirlineReservationSystem.Common;
using AirlineReservationSystem.ViewModels.TicketVM;

namespace AirlineReservationSystem.Services.Implementations
{
    
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IGenericRepository<BookingPassenger> _bookingPassengerRepository;
        private readonly IGenericRepository<FlightSeat> _flightSeatRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(
            ITicketRepository ticketRepository,
            IGenericRepository<BookingPassenger> bookingPassengerRepository,
            IGenericRepository<FlightSeat> flightSeatRepository,
            IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _bookingPassengerRepository = bookingPassengerRepository;
            _flightSeatRepository = flightSeatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TicketIndexVm> SearchAsync(
            string? ticketNumber,
            string? passengerName,
            TicketStatus? status,
            int page,
            int pageSize,
            SortOrder sortOrder,
            CancellationToken cancellationToken = default)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (tickets, totalCount) = await _ticketRepository.SearchAsync(
                ticketNumber, passengerName, status, page, pageSize, sortOrder, cancellationToken);

            var items = tickets.Select(t => new TicketListItemVm
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                Barcode = t.Barcode,
                Fare = t.Fare,
                Status = t.Status,
                IssuedAt = t.IssuedAt,
                PassengerFullName = $"{t.BookingPassenger.Passenger.FirstName} {t.BookingPassenger.Passenger.LastName}",
                BookingReference = t.BookingPassenger.Booking.BookingReference,
                FlightNumber = t.BookingPassenger.FlightSeat.Flight?.FlightNumber,
                SeatNumber = t.BookingPassenger.FlightSeat.Seat?.SeatNumber
            }).ToList();

            return new TicketIndexVm
            {
                Items = items,
                Search = ticketNumber,
                PassengerName = passengerName,
                Status = status,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                SortOrder = sortOrder
            };
        }

        public async Task<TicketDetailsVm?> GetDetailsVmAsync(int id, CancellationToken cancellationToken = default)
        {
            var t = await _ticketRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (t is null) return null;

            return new TicketDetailsVm
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                Barcode = t.Barcode,
                Fare = t.Fare,
                Status = t.Status,
                IssuedAt = t.IssuedAt,
                PassengerFullName = $"{t.BookingPassenger.Passenger.FirstName} {t.BookingPassenger.Passenger.LastName}",
                PassportNumber = t.BookingPassenger.Passenger.PassportNumber,
                BookingReference = t.BookingPassenger.Booking.BookingReference,
                FlightNumber = t.BookingPassenger.FlightSeat.Flight?.FlightNumber,
                SeatNumber = t.BookingPassenger.FlightSeat.Seat?.SeatNumber
            };
        }

        public async Task<Ticket> IssueTicketAsync(int bookingPassengerId, CancellationToken cancellationToken = default)
        {
            var bookingPassenger = await _bookingPassengerRepository.GetOneWithIncludesAsync(
                bp => bp.Id == bookingPassengerId,
                include: q => q.Include(bp => bp.Booking).Include(bp => bp.FlightSeat),
                cancellationToken: cancellationToken);

            if (bookingPassenger is null)
                throw new InvalidOperationException("BookingPassenger not found.");

            if (bookingPassenger.Booking.Status != BookingStatus.Confirmed)
                throw new InvalidOperationException("Cannot issue a ticket for a booking that is not confirmed.");

            var alreadyHasTicket = await _ticketRepository.ExistsForBookingPassengerAsync(bookingPassengerId, cancellationToken);
            if (alreadyHasTicket)
                throw new InvalidOperationException("A ticket has already been issued for this passenger.");

            string ticketNumber;
            do
            {
                ticketNumber = GenerateTicketNumber();
            }
            while (await _ticketRepository.ExistsByTicketNumberAsync(ticketNumber, cancellationToken));

            var ticket = new Ticket
            {
                TicketNumber = ticketNumber,
                Barcode = GenerateBarcode(ticketNumber),
                Fare = bookingPassenger.FlightSeat.Price,
                Status = TicketStatus.Issued,
                IssuedAt = DateTime.UtcNow,
                BookingPassengerId = bookingPassengerId
            };

            await _ticketRepository.AddAsync(ticket, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ticket;
        }

        public async Task<bool> CancelTicketAsync(int id, CancellationToken cancellationToken = default)
        {
            var ticket = await _ticketRepository.GetOneWithIncludesAsync(
                t => t.Id == id,
                include: q => q.Include(t => t.BookingPassenger).ThenInclude(bp => bp.FlightSeat),
                cancellationToken: cancellationToken);

            if (ticket is null) return false;

            if (ticket.Status == TicketStatus.Boarded)
                throw new InvalidOperationException("Cannot cancel a ticket that has already been boarded.");

            ticket.Status = TicketStatus.Cancelled;
            _ticketRepository.Update(ticket);

            var seat = ticket.BookingPassenger.FlightSeat;
            seat.Status = FlightSeatStatus.Available;
            _flightSeatRepository.Update(seat);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> ReinstateAsync(int id, CancellationToken cancellationToken = default)
        {
            var ticket = await _ticketRepository.GetOneWithIncludesAsync(
                t => t.Id == id,
                include: q => q.Include(t => t.BookingPassenger).ThenInclude(bp => bp.FlightSeat),
                cancellationToken: cancellationToken);

            if (ticket is null) return false;

            if (ticket.Status != TicketStatus.Cancelled)
                throw new InvalidOperationException("Only a cancelled ticket can be reinstated.");

            var seat = ticket.BookingPassenger.FlightSeat;
            if (seat.Status != FlightSeatStatus.Available)
                throw new InvalidOperationException("Seat is no longer available for reinstatement.");

            ticket.Status = TicketStatus.Issued;
            seat.Status = FlightSeatStatus.Reserved;
            _ticketRepository.Update(ticket);
            _flightSeatRepository.Update(seat);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static string GenerateTicketNumber()
            => $"TKT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        private static string GenerateBarcode(string ticketNumber)
            => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ticketNumber));
    }
}
