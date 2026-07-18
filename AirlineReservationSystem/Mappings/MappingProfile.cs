
namespace AirlineReservationSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Airport
            CreateMap<Airport, AirportListVM>();
            CreateMap<AirportCreateVM, Airport>();
            CreateMap<Airport, AirportUpdateVM>().ReverseMap();

            // Aircraft
            CreateMap<Aircraft, AircraftListVM>();
            CreateMap<AircraftCreateVM, Aircraft>();
            CreateMap<Aircraft, AircraftUpdateVM>().ReverseMap();

            // Seat
            CreateMap<Seat, SeatListVM>()
                .ForMember(
                 dest => dest.Aircraft,
                 opt => opt.MapFrom(src => src.Aircraft.Model));
            CreateMap<SeatCreateVM, Seat>();
            CreateMap<Seat, SeatUpdateVM>().ReverseMap();

            // Flight
            CreateMap<Flight, FlightListVM>()
                .ForMember(
                    dest => dest.Aircraft,
                    opt => opt.MapFrom(src => src.Aircraft.Model))
                .ForMember(
                    dest => dest.DepartureAirport,
                    opt => opt.MapFrom(src => src.DepartureAirport.Name))
                .ForMember(
                    dest => dest.ArrivalAirport,
                    opt => opt.MapFrom(src => src.ArrivalAirport.Name));

            CreateMap<FlightCreateVM, Flight>();

            CreateMap<Flight, FlightUpdateVM>().ReverseMap();

           

        }
    }
}