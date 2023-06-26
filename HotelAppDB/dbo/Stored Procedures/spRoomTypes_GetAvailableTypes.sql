CREATE PROCEDURE [dbo].[spRoomTypes_GetAvailableTypes]
	@startDate date,
	@endDate date
AS
begin
	select t.id, t.Title, t.Description,t.Price
	from  rooms r inner join RoomTypes t on r.RoomTypeId=t.Id
	where r.Id not in (
				select roomId from Bookings b
				where (@startDate between b.StartDate and b.EndDate)
				or (@endDate between b.StartDate and b.EndDate)
				or (@startDate<=b.StartDate and @endDate>=b.EndDate)
				)
	group by t.id, t.Title, t.Description,t.Price
end