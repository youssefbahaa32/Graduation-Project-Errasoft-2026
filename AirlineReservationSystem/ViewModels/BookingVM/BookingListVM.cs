namespace AirlineReservationSystem.ViewModels.BookingVM
{
    public class BookingListVM
    {
        public BookingFilterVM Filter { get; set; } = new();

        public IEnumerable<BookingIndexVM> Bookings { get; set; }
            = Enumerable.Empty<BookingIndexVM>();

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
