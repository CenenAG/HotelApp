CREATE PROCEDURE [dbo].[sp_Bookings_Search]
	@lastName nvarchar(50),
	@startDate date
AS
select [b].[Id], [b].[RoomId], [b].[GuestId], [b].[StartDate], [b].[EndDate], [b].[Checkedin], [b].[TotalCost], 
    [g].[FirstName], [g].[LastName], 
	[r].[RoomNumber], [r].[RoomTypeId], 
	[rt].[Title] , [rt].[Description], [rt].[Price]
	from Bookings b
	inner join dbo.Guests g on b.GuestId = g.Id
	inner join dbo.Rooms r on r.Id=b.RoomId
	inner join dbo.RoomTypes rt on rt.Id = r.RoomTypeId
		where b.StartDate=@startDate
		and g.LastName like '%' +  @lastname + '%'
		and Checkedin =0
RETURN 0
