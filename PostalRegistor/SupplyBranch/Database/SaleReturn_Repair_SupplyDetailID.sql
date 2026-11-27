/* =========================================================================
   Sale Return - one-time repair
   -------------------------------------------------------------------------
   Pehle frmSaleReturn ghalti se SaleReturnDetail.SupplyDetailID me
   SupplyDetail.IndentDetailID save karta tha. Is script se purane rows ko
   asal SupplyDetailID par shift kiya jata hai.

   Safe / idempotent hai: sirf woh rows update hoti hain jin ki current
   SupplyDetailID kisi SupplyDetail row me mojood nahi, aur unhi rows ke
   liye jahan usi supply ke andar IndentDetailID match karta hai.

   NOTE: Naye records ke liye is script ki zaroorat nahi (code fix ho chuka hai).
   ========================================================================= */

SET NOCOUNT OFF;

BEGIN TRAN;

UPDATE D
SET D.SupplyDetailID = SD.SupplyDetailID
FROM SaleReturnDetail D
INNER JOIN SaleReturnMaster M ON M.ReturnID = D.ReturnID
INNER JOIN SupplyDetail SD ON SD.SupplyID = M.SupplyID
                          AND SD.IndentDetailID = D.SupplyDetailID
                          AND SD.CategoryID = D.CategoryID
                          AND SD.DenominationID = D.DenominationID
WHERE NOT EXISTS (SELECT 1 FROM SupplyDetail X WHERE X.SupplyDetailID = D.SupplyDetailID);

PRINT 'Repaired SaleReturnDetail rows: ' + CAST(@@ROWCOUNT AS varchar(10));

COMMIT;
GO
