namespace AirlineReservationSystem.ViewModels.AirportVM
{
    public class AirportListVM
    {
        
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string IATACode { get; set; }=null!;


        public string ICAOCode { get; set; }= null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public AirportStatus Status { get; set; }

        public string? ImageUrl { get; set; }
    }
}
