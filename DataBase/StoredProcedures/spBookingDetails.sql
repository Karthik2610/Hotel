/****** Object:  StoredProcedure [dbo].[spBookingDetails]    Script Date: 7/5/2026 12:13:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--exec spBookingDetails '2026-06-19','2026-06-20'
CREATE PROCEDURE [dbo].[spBookingDetails]
	(@CheckInDate datetime=null,@CheckOutDate datetime=null)
AS
BEGIN



	select BD.bookingid [Booking ID],RoomName [Room Name],CD.FirstName [First Name],CD.LastName [Last Name], CD.PhoneNumber [Phone Number],RD.RoomID,
  --case when BD.checkindate=CAST(GETDATE() AS DATE) then 'CheckIn - '+ convert(nvarchar, CAST(@CheckInDate AS DATE))
  --when BD.checkoutdate=CAST(GETDATE() AS DATE) then 'CheckOut - '+convert(nvarchar,CAST(@CheckOutDate AS DATE))  
  --else
  'CheckIn - '+convert(nvarchar, CAST(BD.CheckInDate AS DATE)) +''+  ' CheckOut- '+convert(nvarchar,CAST(BD.CheckOutDate AS DATE)) 
  --end 
  as Details,BD.BookingStatus
  from BookingDetails BD 
  inner join CustomerDetails CD on CD.CustomerID=BD.CutomerID
 inner join RoomDetails RD on RD.bookingid=BD.bookingid
 inner join RoomsLU R on R.RoomID=RD.RoomID
  where BD.checkindate >=@CheckInDate and 
  BD.checkoutdate<=@CheckOutDate end  





GO


