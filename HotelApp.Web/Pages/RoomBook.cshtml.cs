using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages
{
    public class RoomBookModel : PageModel
    {
        private readonly IDatabaseData _db;

        [BindProperty(SupportsGet = true)]
        public int RoomTypeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        [BindProperty]
        public string FirstName { get; set; }

        [BindProperty]
        public string LastName { get; set; }

        public RoomTypeModel RoomType { get; set; } = new RoomTypeModel();

        public RoomBookModel(IDatabaseData db)
        {
            _db = db;
        }

        public void OnGet()
        {
            if (RoomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(RoomTypeId);
            }
        }

        public IActionResult OnPost()
        {
            _db.BookGuest(FirstName, LastName, StartDate, EndDate, RoomTypeId);
            return RedirectToPage("/Index");
        }
    }
}
