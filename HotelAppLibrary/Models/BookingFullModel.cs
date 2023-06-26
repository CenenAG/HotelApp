// Ignore Spelling: Checkedin App

namespace HotelAppLibrary.Models
{
    public class BookingFullModel
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Checkedin { get; set; } 
        public decimal TotalCost { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public int RoomTypeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
