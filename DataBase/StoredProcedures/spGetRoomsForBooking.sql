
/****** Object:  StoredProcedure [dbo].[spGetRoomsForBooking]    Script Date: 7/5/2026 12:14:34 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--exec spGetRoomsForBooking 40,'2026-06-29','2026-06-30'
CREATE PROCEDURE [dbo].[spGetRoomsForBooking]
	(@BookingID int =null,@CheckInDate datetime=null,@CheckOutDate datetime=null)
AS
BEGIN




		create table #Temp1 (RoomID int,RoomName nvarchar(255),ExtraBeds int, IsSelected bit)
		create table #Temp2 (RoomID int)


	insert into #Temp1
	select RD.RoomID,R.RoomName,RD.ExtraBeds,1 as IsSelected  
  from BookingDetails BD   
 inner join RoomDetails RD on RD.bookingid=BD.bookingid
 inner join RoomsLU R on R.RoomID=RD.RoomID
  where BD.BookingID=@BookingID


  insert into #Temp2
	select RD.RoomID
  from BookingDetails BD   
 inner join RoomDetails RD on RD.bookingid=BD.bookingid
 inner join RoomsLU R on R.RoomID=RD.RoomID
  where BD.checkindate >=@CheckInDate and 
  BD.checkoutdate<=@CheckOutDate   

  --select * from #temp2

  
  insert into #Temp1
  select R.RoomID,R.RoomName,0,0 as IsSelected  
  from RoomsLU R where  R.RoomID not in (select RoomID from #Temp1) 
  and  R.RoomID not in (select RoomID from #Temp2) 

  select * from #Temp1

  drop table #Temp1;
  drop table #Temp2;

END  
GO


