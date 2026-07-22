namespace AirlineReservationSystem.ViewModels.TicketVM
{
    public class TicketIndexVm : BaseIndexVM
    {
        public List<TicketListItemVm> Items { get; set; } = new();

        public string? PassengerName { get; set; }
        public TicketStatus? Status { get; set; }

    }
}
