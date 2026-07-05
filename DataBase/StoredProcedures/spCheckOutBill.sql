
/****** Object:  StoredProcedure [dbo].[spCheckOutBill]    Script Date: 7/5/2026 12:14:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--[spCheckOutBill] 3
CREATE PROCEDURE [dbo].[spCheckOutBill]
	(@BookingID int)
AS
BEGIN


declare @CGST decimal(18,2),@SGST decimal(18,2),@ExtraBedCost decimal(18,2)

select @CGST=CGST,@SGST=SGST,@ExtraBedCost=ExtraBedCost from HotelDetails
;WITH BookingCharges AS
(
    SELECT
        R.RoomName,
        CAST(BD.CheckInDate AS DATE) AS CheckInDate,
        CAST(BD.CheckOutDate AS DATE) AS CheckOutDate,
        DATEDIFF(DAY, BD.CheckInDate, BD.CheckOutDate) AS NoOfDays,
        isnull(RD.ExtraBeds,0) ExtraBeds,
        R.Price * DATEDIFF(DAY, BD.CheckInDate, BD.CheckOutDate) AS RoomCharge,
        isnull(RD.ExtraBeds,0) * @ExtraBedCost * DATEDIFF(DAY, BD.CheckInDate, BD.CheckOutDate) AS ExtraBedCharge,
        (R.Price + (isnull(RD.ExtraBeds,0) * @ExtraBedCost)) * DATEDIFF(DAY, BD.CheckInDate, BD.CheckOutDate) AS TotalAmount,
        BD.AdvanceAmount
    FROM BookingDetails BD
    INNER JOIN RoomDetails RD
        ON RD.BookingID = BD.BookingID
    INNER JOIN RoomsLU R
        ON R.RoomID = RD.RoomID
    WHERE BD.BookingID = @BookingID
),
Totals AS
(
    SELECT SUM(TotalAmount) AS TotalAmount,
     MAX(ISNULL(AdvanceAmount,0)) AS AdvanceAmount
    FROM BookingCharges
)

SELECT
    RoomName,
    CheckInDate,
    CheckOutDate,
    NoOfDays,
    ExtraBeds,
    RoomCharge,
    ExtraBedCharge,
    TotalAmount
FROM BookingCharges

UNION ALL
SELECT
    'Advance Paid',
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    T.AdvanceAmount
FROM Totals T


UNION ALL

SELECT
    'Total',
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    T.TotalAmount
FROM Totals T

UNION ALL

SELECT
    CONCAT('CGST (', @CGST, '%)'),
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    T.TotalAmount * @CGST / 100
FROM Totals T


UNION ALL

SELECT
    CONCAT('SGST (', @SGST, '%)'),
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    T.TotalAmount * @SGST / 100
FROM Totals T


UNION ALL

SELECT
    'Grand Total',
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    T.TotalAmount
      + (T.TotalAmount * @CGST / 100)
      + (T.TotalAmount * @SGST / 100)
FROM Totals T

UNION ALL

SELECT
    'Balance Due',
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    NULL,
    (
        T.TotalAmount
        + (T.TotalAmount * @CGST / 100)
        + (T.TotalAmount * @SGST / 100)
    ) - T.AdvanceAmount
FROM Totals T;

END
GO


