
namespace AirlineReservationSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Airport
            CreateMap<Airport, AirportIndexVM>().ReverseMap();
            CreateMap<Airport, AirportDetailsVM>().ReverseMap();
            CreateMap<Airport, AirportCreateVM>().ReverseMap();
            CreateMap<Airport, AirportUpdateVM>().ReverseMap();

            // Aircraft
            CreateMap<Aircraft, AircraftIndexVM>().ReverseMap();
            CreateMap<Aircraft, AircraftDetailsVM>().ReverseMap();
            CreateMap<Aircraft, AircraftCreateVM>().ReverseMap();
            CreateMap<Aircraft, AircraftUpdateVM>().ReverseMap();

            // Seat
            CreateMap<Seat, SeatCreateVM>().ReverseMap();   
            CreateMap<Seat, SeatUpdateVM>().ReverseMap();
            CreateMap<Seat, SeatIndexVM>().ForMember(
               dest => dest.Aircraft,
               opt => opt.MapFrom(src => src.Aircraft.Model)); 
            CreateMap<Seat, SeatDetailsVM>()
                .ForMember(
                dest => dest.Aircraft,
                opt => opt.MapFrom(src => src.Aircraft.Model));

            // Flight
            CreateMap<Flight, FlightIndexVM>().ReverseMap();
            CreateMap<Flight, FlightDetailsVM>().ReverseMap();
            // Index
            CreateMap<Flight, FlightIndexVM>()
                .ForMember(d => d.Aircraft,
                    o => o.MapFrom(s => s.Aircraft.Model))
                .ForMember(d => d.DepartureAirport,
                    o => o.MapFrom(s => s.DepartureAirport.Name))
                .ForMember(d => d.ArrivalAirport,
                    o => o.MapFrom(s => s.ArrivalAirport.Name));

            // Details
            CreateMap<Flight, FlightDetailsVM>()
                .ForMember(d => d.Aircraft,
                    o => o.MapFrom(s => s.Aircraft.Model))
                .ForMember(d => d.DepartureAirport,
                    o => o.MapFrom(s => s.DepartureAirport.Name))
                .ForMember(d => d.ArrivalAirport,
                    o => o.MapFrom(s => s.ArrivalAirport.Name));
        }
    }
}