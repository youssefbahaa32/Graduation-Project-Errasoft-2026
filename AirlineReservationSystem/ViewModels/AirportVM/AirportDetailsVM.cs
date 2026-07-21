namespace AirlineReservationSystem.ViewModels.Airport
{
    public class AirportDetailsVM
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string IATACode { get; set; } = null!;

        public string ICAOCode { get; set; } = null!;
    
        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;
        public List<AirportImageVM> Images { get; set; } = [];
    }
}