CREATE PROCEDURE [dbo].[sp_Rooms_GetAvailableRooms]
	@startDate date,
	@endDate date,
	@roomTypeId int
AS
	select r.*
	from  rooms r inner join RoomTypes t on r.RoomTypeId=t.Id
	where r.RoomTypeId=@roomTypeId
	and r.Id not in (
				select roomId from Bookings b
				where (@startDate between b.StartDate and b.EndDate)
				or (@endDate between b.StartDate and b.EndDate)
				or (@startDate<=b.StartDate and @endDate>=b.EndDate)
				)
RETURN 0
