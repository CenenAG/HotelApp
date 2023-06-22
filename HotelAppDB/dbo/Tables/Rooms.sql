CREATE TABLE [dbo].[Rooms]
(
	[Id] INT NOT NULL , 
    [RoomNumber] VARCHAR(10) NOT NULL, 
    [RoomTypeId] INT NOT NULL, 
    CONSTRAINT [PK_Room] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Rooms_RoomTypes] FOREIGN KEY (RoomTypeId) REFERENCES RoomTypes(Id) 
)
