
using AirlineReservationSystem.ViewModels.Loyalty;
using AirlineReservationSystem.ViewModels.LoyaltyAccountVM;

namespace AirlineReservationSystem.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Airport
            CreateMap<Airport, AirportListVM>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src =>
                    src.Images
                .Select(i => i.ImageUrl)
                .FirstOrDefault()));
            CreateMap<AirportCreateVM, Airport>()

                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<AirportImage, AirportImageVM>();

            CreateMap<Airport, AirportUpdateVM>()
                .ForMember(dest => dest.ExistingImages,
                    opt => opt.MapFrom(src => src.Images));

            CreateMap<AirportUpdateVM, Airport>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Airport, AirportDetailsVM>();

            // Aircraft
            CreateMap<Aircraft, AircraftListVM>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src =>
                src.Images.Select(i => i.ImageUrl).FirstOrDefault()));

            CreateMap<AircraftCreateVM, Aircraft>()
                .ForMember(x => x.Images, opt => opt.Ignore());

            CreateMap<AircraftImage, AircraftImageVM>();

            CreateMap<Aircraft, AircraftUpdateVM>()
                .ForMember(dest => dest.ExistingImages,
                    opt => opt.MapFrom(src => src.Images));

            CreateMap<AircraftUpdateVM, Aircraft>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Aircraft, AircraftDetailsVM>();

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
                    opt => opt.MapFrom(src => src.ArrivalAirport.Name))
                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.Aircraft.Images.Select(i => i.ImageUrl).FirstOrDefault()));

            CreateMap<FlightCreateVM, Flight>();

            CreateMap<FlightImage, FlightImageVM>();

            CreateMap<Flight, FlightUpdateVM>()
                .ForMember(dest => dest.ExistingImages,
                    opt => opt.MapFrom(src => src.Images));

            CreateMap<FlightUpdateVM, Flight>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Flight, FlightDetailsVM>();

            //Passenger

            CreateMap<PassengerCreateVM, Passenger>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<PassengerImage, PassengerImageVM>();

            CreateMap<Passenger, PassengerUpdateVM>()
                .ForMember(dest => dest.ExistingImages,
                    opt => opt.MapFrom(src => src.Images));

            CreateMap<PassengerUpdateVM, Passenger>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<PassengerDetailsVM, Passenger>();

            CreateMap<Passenger, PassengerListVM>()
                .ForMember(dest => dest.ImageUrl,
                opt => opt.MapFrom(src =>
                    src.Images
                .Select(i => i.ImageUrl)
                .FirstOrDefault()));

            // Loyalty

            CreateMap<LoyaltyAccount, LoyaltyListVM>()
                .ForMember(
                    dest => dest.User,
                    opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<LoyaltyAccount, LoyaltyDetailsVM>()
                .ForMember(
                    dest => dest.User,
                    opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<LoyaltyCreateVM, LoyaltyAccount>();

            CreateMap<LoyaltyAccount, LoyaltyUpdateVM>()
                .ReverseMap();

        }
    }
}