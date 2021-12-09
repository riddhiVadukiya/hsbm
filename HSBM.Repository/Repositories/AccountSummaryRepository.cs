using BLToolkit.Data;
using HSBM.EntityModel.AccountSummary;
using HSBM.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HSBM.Repository.Repositories
{
    public class AccountSummaryRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<AccountSummaryResponse> GetAccountSummary(AccountSummaryRequest request)
        {
            List<AccountSummaryResponse> result = new List<AccountSummaryResponse>();

            try
            {
                string query = @"
SELECT 
	OD.Id,
    OD.Description,
    SU.FirstName + ' ' + SU.LastName AS UserName,
	OD.TransactionId AS TransactionNumber,
	OD.ReferenceNumber AS ReferenceNumber,
	OD.Currency,
	OD.Amount,
	(CASE 
        WHEN SM.MarkupType = 1 THEN CAST(((OD.Amount / SM.MarkUp)) AS DECIMAL(18, 2))
		WHEN SM.MarkupType = 2 THEN CAST(SM.MarkUp AS DECIMAL(18, 2))
	END) AS MarkupAmount,
	ISNULL(FB.SupplierType, HB.HotelType) AS SupplierType,
	SM.CompanyName,
	CAST(OD.CreatedDate AS DATE) AS CreatedDate,
	(CASE WHEN OD.OrderType = 1 THEN 'Hotel' ELSE 'Flight' END) AS OrderType
FROM OrderDetail OD
LEFT JOIN FlightBookingMaster FB ON OD.Id = FB.OrderId
LEFT JOIN HotelBookingMaster HB ON OD.Id = HB.OrderId
INNER JOIN SupplierMaster SM ON FB.SupplierType = SM.SupplierType OR HB.HotelType = SM.SupplierType
INNER JOIN SystemUsers SU ON OD.UserId = SU.Id
WHERE OD.Status = '" + Common.Enums.BookingStatus.CONFIRMED.ToString() + "'";
                
                if (request.SystemUserId > 0)
                {
                    query += "AND SM.SupplierType = 5";
                }

                query += @"
ORDER BY OD.CreatedDate DESC
";

                using (DbManager _DbManager = new DbManager())
                {
                    result = _DbManager.SetCommand(query).ExecuteList<AccountSummaryResponse>();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("HotelBeds=>HotelService(AddHotelDetail-AddUpdateHotelDescriptionDetail): DescriptionNotInserted ", ex);
                return null;
            }
        }

        public List<OutstandingSummary> GetOutstandingSummary()
        {
            List<OutstandingSummary> result = new List<OutstandingSummary>();

            try
            {
                string query = @"
WITH Summary AS 
(
	SELECT 
		SM.CompanyName,
		COUNT(*) OVER (PARTITION BY SM.CompanyName) AS TotalBookings,
		SUM(CASE 
			WHEN SM.MarkupType = " + (int)Common.Enums.MarkupType.Percentage + @" THEN CAST(((OD.Amount / SM.MarkUp)) AS DECIMAL(18, 2))
			WHEN SM.MarkupType = " + (int)Common.Enums.MarkupType.FlatAmount + @" THEN CAST(SM.MarkUp AS DECIMAL(18, 2))
		END) OVER (PARTITION BY SM.Companyname) AS Outstanding
	FROM OrderDetail OD
	LEFT JOIN FlightBookingMaster FB ON OD.Id = FB.OrderId
	LEFT JOIN HotelBookingMaster HB ON OD.Id = HB.OrderId
	INNER JOIN SupplierMaster SM ON FB.SupplierType = SM.SupplierType OR HB.HotelType = SM.SupplierType
	WHERE OD.Status = 'COMPLETE'
) 
SELECT 
	Sm.CompanyName, 
	Sm.TotalBookings, 
	(SELECT TOP 1 CurrencyCode FROM CurrencyMaster WHERE IsBaseCurrency = 'true') AS Currency, 
	Sm.Outstanding
FROM Summary Sm 
GROUP BY Sm.CompanyName, Sm.TotalBookings, Sm.Outstanding
";

                using (DbManager _DbManager = new DbManager())
                {
                    result = _DbManager.SetCommand(query).ExecuteList<OutstandingSummary>();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("HotelBeds=>HotelService(AddHotelDetail-AddUpdateHotelDescriptionDetail): DescriptionNotInserted ", ex);
                return null;
            }
        }

    }
}

