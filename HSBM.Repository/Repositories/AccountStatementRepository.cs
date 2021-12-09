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
using System.Globalization;

namespace HSBM.Repository.Repositories
{
    public class AccountStatementRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public GridDataResponse GetAccountStatementBySearchRequest(GridParams p_GridParams, AccountStatementRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" select tt.*,fs.Name as FarmstaysName,RecordsTotal = COUNT(*) OVER() from (
	select Farmstaysid,Count(Farmstaysid) as TotalBooking,sum(profit) as TotalEarning from 
	(select Farmstaysid,Amount,MarkupPercentage,
    case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit
    from OrderMaster";

                    //convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) as profit  

                    bool IsWhereClauseEmpty = false;

                    _SqlQuery += " where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + ")";


                    //DateTime CheckInFrom = DateTime.MinValue;
                    //DateTime CheckInTo = DateTime.MinValue;
                    //if (!string.IsNullOrEmpty(p_Request.CheckInFrom))
                    //{
                    //    CheckInFrom = DateTime.ParseExact(p_Request.CheckInFrom, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    //}
                    //if (!string.IsNullOrEmpty(p_Request.CheckInTo))
                    //{
                    //    CheckInTo = DateTime.ParseExact(p_Request.CheckInTo, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    //}

                    if (!string.IsNullOrEmpty(p_Request.BookingFrom))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" OrderDate  >= '" + (DateTime.ParseExact(p_Request.BookingFrom, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture)).ToString("dd/MMM/yyyy") + @"'";
                    }

                    if (!string.IsNullOrEmpty(p_Request.BookingTo))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" OrderDate  <= '" + (DateTime.ParseExact(p_Request.BookingTo, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture)).ToString("dd/MMM/yyyy") + @"'";
                    }
                    
                    if (!string.IsNullOrEmpty(p_Request.CheckInFrom))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" CheckInDate  >= '" + (DateTime.ParseExact(p_Request.CheckInFrom, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture)).ToString("dd/MMM/yyyy") + @"'";
                    }
                    if (!string.IsNullOrEmpty(p_Request.CheckInTo))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" CheckInDate  <= '" + (DateTime.ParseExact(p_Request.CheckInTo, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture)).ToString("dd/MMM/yyyy") + @"'";
                    }

                    if (!string.IsNullOrEmpty(p_Request.FarmStaysName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += @" FarmstaysName  like  '%" + p_Request.FarmStaysName + @"%'";
                    }


                    _SqlQuery += @") as t group by Farmstaysid ) tt  inner join FarmStays fs on tt.Farmstaysid = fs.id ";
            

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<AccountStatement>();
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

        

    }
}

