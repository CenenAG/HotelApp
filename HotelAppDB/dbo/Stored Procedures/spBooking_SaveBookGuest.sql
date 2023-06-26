CREATE PROCEDURE [dbo].[spBooking_SaveBookGuest]
    @RoomId int,
    @GuestId int,
    @StartDate datetime,
    @EndDate datetime,
    @Checkedin bit,
    @TotalCost decimal
AS
Insert into dbo.Bookings
     ( [RoomId], [GuestId], [StartDate], [EndDate], [Checkedin], [TotalCost] )
    values ( @RoomId, @GuestId, @StartDate, @EndDate, @Checkedin, @TotalCost)
RETURN 0
