CREATE PROCEDURE [dbo].[spRoomTypes_GetById]
	@id int 
AS
begin
	SELECT [Id], [Title], [Description], [Price] 
	from dbo.RoomTypes 
	where Id=@id
end

