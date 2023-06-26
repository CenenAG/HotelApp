CREATE PROCEDURE [dbo].[spBookings_CheckIn]
	@bookingId int
AS
	Update Bookings set Checkedin=1 where Id=@bookingId
RETURN 0
