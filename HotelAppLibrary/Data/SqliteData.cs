// Ignore Spelling: Sql App Sqlite

using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;

namespace HotelAppLibrary.Data
{
    public class SqliteData : IDatabaseData
    {
        private readonly ISqliteDataAccess _db;
        private const string connectionStringName = "Sqlite";

        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
        }

        public List<RoomTypeModel> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sqliteStatement = @"select t.id, t.Title, t.Description,t.Price Price 
                                from  rooms r inner join RoomTypes t on r.RoomTypeId=t.Id 
                                where r.Id not in (select roomId from Bookings b  
                                                    where (@startDate < b.StartDate and @endDate > b.EndDate) 
                                                    or (b.StartDate <= @endDate and @endDate < b.EndDate)  
                                                    or (b.StartDate <= @startDate and @startDate < b.EndDate)   )
                                group by t.id, t.Title, t.Description,t.Price";

            var output = _db.LoadData<RoomTypeModel, dynamic>(sqliteStatement,
                                                    new { startDate, endDate }, connectionStringName);

            output.ForEach(x => x.Price = x.Price / 1000);

            return output;
        }

        public void BookGuest(string firstName,
                              string lastName,
                              DateTime startDate,
                              DateTime endDate,
                              int roomTypeId)
        {

            string sql = @"select 1 from Guests where FirstName=@firstName and LastName=@lastName";
            List<GuestModel> encontrados = _db.LoadData<GuestModel, dynamic>(sql, new { firstName, lastName }, connectionStringName);
            if (encontrados.Count == 0)
            {
                sql = "insert into Guests (FirstName,LastName) values (@firstName, @lastName)";
                _db.SaveData(sql, new { firstName, lastName }, connectionStringName);
            }

            sql = "select [Id], [FirstName], [LastName] from Guests where FirstName=@firstName and LastName=@lastName";
            List<GuestModel> GuestEncontrados = _db.LoadData<GuestModel, dynamic>(sql, new { firstName, lastName },
                connectionStringName);

            RoomTypeModel roomType = _db.LoadData<RoomTypeModel, dynamic>("select * from RoomTypes where Id=@Id",
                new { Id = roomTypeId }, connectionStringName).First();


            sql = @"select r.*
	                        from  rooms r inner join RoomTypes t on r.RoomTypeId=t.Id
	                        where r.RoomTypeId=@roomTypeId
	                        and r.Id not in (
				                        select roomId from Bookings b
				                        where (@startDate between b.StartDate and b.EndDate)
				                        or (@endDate between b.StartDate and b.EndDate)
				                        or (@startDate<=b.StartDate and @endDate>=b.EndDate)
				                        )";
            RoomModel firstAvailableRoom = _db.LoadData<RoomModel, dynamic>(sql,
                new { startDate, endDate, roomTypeId }, connectionStringName).First();

            var dias = (endDate - startDate).Days;
            int guestId = GuestEncontrados[0].Id;
            decimal TotalCost = roomType.Price * dias * 100;

            sql = @"Insert into Bookings
                            ( [RoomId], [GuestId], [StartDate], [EndDate], [Checkedin], [TotalCost] )
                            values ( @RoomId, @GuestId, @StartDate, @EndDate, @Checkedin, @TotalCost)";

            _db.SaveData(sql,
                new
                {
                    RoomId = firstAvailableRoom.Id,
                    GuestId = guestId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Checkedin = 0,
                    TotalCost = TotalCost
                }, connectionStringName);
        }

        public List<BookingFullModel> SearchBookings(string lastName)
        {

            string sql = @"select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[Checkedin], [b].[TotalCost], 
                            [g].[FirstName], [g].[LastName], 
	                        [r].[RoomNumber], [r].[RoomTypeId], 
	                        [rt].[Title] , [rt].[Description], [rt].[Price]
	                        from Bookings b
	                        inner join Guests g on b.GuestId = g.Id
	                        inner join Rooms r on r.Id=b.RoomId
	                        inner join RoomTypes rt on rt.Id = r.RoomTypeId
                            where b.Checkedin=0
                            and LastName like @lastName
                            and substr([b].[StartDate],1,10)=substr(@startDate,1,10)";
      
            string lastNameTxt =  '%' +lastName + '%';


            return _db.LoadData<BookingFullModel, dynamic>(sql, new { lastName=lastNameTxt, startDate = DateTime.Now.Date.ToString("yyyy-MM-dd") }, connectionStringName);
        }

        public void CheckInGuest(int bookingId)
        {
            _db.SaveData("Update Bookings set Checkedin=1 where Id=@bookingId",
                new { bookingId }, connectionStringName);
        }

        public RoomTypeModel GetRoomTypeById(int id)
        {
            return _db.LoadData<RoomTypeModel, dynamic>("SELECT [Id], [Title], [Description], [Price] from RoomTypes where Id=@id",
                                                    new { id },
                                                    connectionStringName).FirstOrDefault();
        }
    }
}


