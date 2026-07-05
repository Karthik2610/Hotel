
/****** Object:  StoredProcedure [dbo].[spCustomerDetails]    Script Date: 7/5/2026 12:14:19 PM ******/
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
CREATE PROCEDURE [dbo].[spCustomerDetails]
	(@BookingID int=null)
AS
BEGIN



	select CD.*
  from BookingDetails BD 
  inner join CustomerDetails CD on CD.CustomerID=BD.CutomerID 
  where BD.BookingID=@BookingID



  END

GO


