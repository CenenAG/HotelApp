﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if not exists (select 1 from dbo.RoomTypes)
begin
    insert into dbo.RoomTypes (Title, Description, Price)
    values('King Size Bed', 'A room with a king-size bed and a window.',1500),
    ('Two Queen Size Beds','A room with Two Queen Size Beds and a window.',2000),
    ('Executive Suite','A room with a king-size bed and a window.',3000);
end

if not exists (select 1 from dbo.Rooms)
begin
    declare @roomId1 int;
    declare @roomId2 int;
    declare @roomId3 int;

    select @roomId1=Id from dbo.RoomTypes where title='King Size Bed';
    select @roomId2=Id from dbo.RoomTypes where title='Two Queen Size Beds';
    select @roomId3=Id from dbo.RoomTypes where title='Executive Suite';

    insert into dbo.Rooms(RoomNumber, RoomTypeId)
    values
    ('101',@roomId1),
    ('102',@roomId1),
    ('103',@roomId1),
    ('201',@roomId2),
    ('202',@roomId2),
    ('203',@roomId2),
    ('301',@roomId3),
    ('302',@roomId3),
    ('303',@roomId3)
end
