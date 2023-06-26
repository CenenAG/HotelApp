// Ignore Spelling: Sql

using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;

namespace HotelAppLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                                    new { startDate, endDate }, connectionStringName, true);
        }

        public void BookGuest(string firstName,
                              string lastName,
                              DateTime startDate,
                              DateTime endDate,
                              int roomTypeId)
        {
            //exist the guest

            GuestModel guest = _db.LoadData<GuestModel, dynamic>("dbo.spGuests_Insert",
                new GuestModel { FirstName = firstName, LastName = lastName },
                connectionStringName, true).First();

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from dbo.RoomTypes where Id=@Id",
                new { Id = roomTypeId }, connectionStringName, false).First();


            RoomModel firstAvailableRoom = _db.LoadData<RoomModel, dynamic>("dbo.sp_Rooms_GetAvailableRooms",
                new { startDate, endDate, roomTypeId }, connectionStringName, true).First();

            var dias = (endDate - startDate).Days;

            BookingModel booking = new BookingModel
            {
                RoomId = firstAvailableRoom.Id,
                GuestId = guest.Id,
                StartDate = startDate,
                EndDate = endDate,
                Checkedin = 0,
                TotalCost = roomType.Price * dias
            };

            _db.SaveData<BookingModel>("dbo.spBooking_SaveBookGuest",
                booking, connectionStringName, true);
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {
            return _db.LoadData<BookingFullModel, dynamic>("sp_Bookings_Search",
                                                    new { lastName, startDate = DateTime.Now.Date },
                                                    connectionStringName,
                                                    true);
        }

        public void CheckInGuest(int bookingId)
        {
            _db.SaveData("dbo.spBookings_CheckIn",
                new { bookingId }, connectionStringName, true);
        }
    }
}


