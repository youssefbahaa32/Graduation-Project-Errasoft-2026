namespace AirlineReservationSystem.ViewModels.BookingVM
{
    public class BookingFilterVM
    {
        public string? Keyword { get; set; }

        public BookingStatus? BookingStatus { get; set; }

        public PaymentStatus? PaymentStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
