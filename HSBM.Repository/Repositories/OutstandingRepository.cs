using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.AccountSummary;
using HSBM.EntityModel.Common;
using HSBM.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using HSBM.Common.Enums;

namespace HSBM.Repository.Repositories
{
    public class OutstandingRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public GridDataResponse GetOutstandingBySearchRequest(GridParams p_GridParams, OutstandingRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" select tt.*,fs.Name as FarmstaysName,RecordsTotal = COUNT(*) OVER(), isnull(o.IsCleared,0) as IsCleared,o.Id as Outstandingid from (
	select BookingYear,BookingMonth,Farmstaysid,Count(Farmstaysid) as TotalBooking,sum(Outstanding) as TotalOutstanding from (

	select 
	OrderDate,
	DATEPART(YEAR, OrderDate) as BookingYear,
	DATEPART(MONTH, OrderDate) as BookingMonth,
	Farmstaysid,
	Amount,
	MarkupPercentage,
	case when isnull(RefundAmount,0) > 0  then convert(decimal(18,2),(convert(decimal(18,2),  ((Amount * 100)/(100+MarkupPercentage))) - (convert(decimal(18,2),  ((Amount * 100)/(100+MarkupPercentage))) * RefundablePercentage / 100))) else convert(decimal(18,2),  ((Amount * 100)/(100+MarkupPercentage))) end as Outstanding 
	from OrderMaster ";

                    bool IsWhereClauseEmpty = false;

                    _SqlQuery += " where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + ")";

                    if (p_Request.StartDate != null && p_Request.StartDate != string.Empty)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" OrderDate  >= '" + p_Request.StartDate + @"'";
                    }

                    if (p_Request.EndDate != null && p_Request.EndDate != string.Empty)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" OrderDate  <= '" + p_Request.EndDate + @"'";
                    }

                    if (!string.IsNullOrEmpty( p_Request.FarmStaysName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" FarmStaysName  like '%" + p_Request.FarmStaysName + @"%'";
                    }

                    _SqlQuery += @" ) as t group by Farmstaysid,BookingYear,BookingMonth ) tt  inner join FarmStays fs on tt.Farmstaysid = fs.id 
                                left join Outstanding o on fs.id = o.FarmStaysId and o.Month =BookingMonth and o.Year = BookingYear ";


                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<Outstanding>();
                    if (objList.Any())
                    {
                        _GridDataResponse.recordsTotal = objList.FirstOrDefault().RecordsTotal;
                        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    }
                    _GridDataResponse.data = objList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _GridDataResponse;
        }

        public int AddOrUpdateOutstanding(long Farmstaysid, long BookingYear, long BookingMonth, bool Status, long OutstandingId, long userid)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {


                    if (OutstandingId != 0)
                        {
                            _SqlQuery = @"
                                        UPDATE [dbo].[Outstanding] SET 
                                        [FarmStaysId] = @FarmStaysId
                                        ,[Month] = @Month
                                        ,[Year] = @Year
                                        ,[IsCleared]= @IsCleared
                                        ,[UpdatedBy] = @UpdatedBy
                                        ,[UpdatedDate] = @UpdatedDate
                                        WHERE [Id] = @Id";
                        }
                        else
                        {
                            _SqlQuery = @"
                                        INSERT INTO [dbo].[Outstanding]
                                            ([FarmStaysId]
                                            ,[Month]
                                            ,[Year]
                                            ,[IsCleared]
                                            ,[CreatedBy]
                                            ,[CreatedDate]
                                            ,[UpdatedBy]
                                            ,[UpdatedDate])
                                        VALUES
                                            (@FarmStaysId
                                            ,@Month
                                            ,@Year
                                            ,@IsCleared
                                            ,@CreatedBy
                                            ,@CreatedDate
                                            ,@UpdatedBy
                                            ,@UpdatedDate)";
                        }

                        var currentDate = DateTime.Now;

                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Id", OutstandingId),
                            _DbManager.Parameter("@FarmStaysId", Farmstaysid),
                            _DbManager.Parameter("@Month", BookingMonth),
                            _DbManager.Parameter("@Year", BookingYear),
                            _DbManager.Parameter("@IsCleared", Status),
                            _DbManager.Parameter("@CreatedBy", userid),
                            _DbManager.Parameter("@CreatedDate", currentDate),
                            _DbManager.Parameter("@UpdatedBy", userid),
                            _DbManager.Parameter("@UpdatedDate", currentDate)).ExecuteNonQuery();

                        if (Affected > 0)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}

