using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.Front;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Repository.Repositories
{
    public class FrontOrdersMasterRepository
    {
        public List<FrontOrdersMasterResponse> GetAllOrdersMasterBySearchRequest(GridParams p_GridParams, FrontOrdersMasterRequest p_SearchRequest)
        {
            List<FrontOrdersMasterResponse> objList = new List<FrontOrdersMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    //string _SqlQuery = @"SELECT Id,OrderNo,OrderDate,FarmStaysName,NetAmount,Status,Farmstaysid,RecordsTotal = COUNT(*) OVER()  FROM [dbo].[OrderMaster]";
                    //    dbo.CurrecncyConversion('"+p_SearchRequest.CurrencyCode+@"',NetAmount)

                    string _SqlQuery = @"SELECT rr.FarmStaysRatingsAndReviewGUID,om.Id,om.OrderNo,om.CheckOutDate,OrderDate,FarmStaysName,
                                        NetAmount as NetAmount,Status,Farmstaysid,
                                        RecordsTotal = COUNT(*) OVER()  FROM [dbo].[OrderMaster] om
                                        left join [dbo].[FarmStaysRatingsAndReview] rr on rr.OrderNo = om.OrderNo";

                    bool IsWhereClauseEmpty = true;

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " [IsActive] = 1 ";
                    //}                     
                    DateTime CreatedDateFrom = DateTime.MinValue;
                    DateTime CreatedDateTo = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(p_SearchRequest.CreatedDateFrom))
                    {
                        CreatedDateFrom = DateTime.ParseExact(p_SearchRequest.CreatedDateFrom, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.CreatedDateTo))
                    {
                        CreatedDateTo = DateTime.ParseExact(p_SearchRequest.CreatedDateTo, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    }
                    if (Convert.ToDateTime(CreatedDateFrom) != DateTime.MinValue)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " cast( [OrderDate] as date) >= '" + CreatedDateFrom.ToString("dd/MMM/yyyy") + "' ";
                    }
                    if (Convert.ToDateTime(CreatedDateTo) != DateTime.MinValue)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " cast( [OrderDate] as date) <= '" + CreatedDateTo.ToString("dd/MMM/yyyy") + "'";
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.OrderNo))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " om.OrderNo Like  '%" + p_SearchRequest.OrderNo + "%'";
                    }

                    if (!string.IsNullOrEmpty(p_SearchRequest.CustomerName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += "(GuestFirstName +' '+ GuestLastName) like '%" + p_SearchRequest.CustomerName.Trim() + "%'";
                        //_SqlQuery += " GuestFirstName like '%" + p_SearchRequest.CustomerName + "%' OR  GuestLastName like '%" + p_SearchRequest.CustomerName + "%'";
                    }
                    if (p_SearchRequest.Farmstaysid > 0)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " om.Farmstaysid = " + p_SearchRequest.Farmstaysid;
                    }
                    if (p_SearchRequest.OrderStatusId > 0)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " Status = " + p_SearchRequest.OrderStatusId;
                    }
                    if (p_SearchRequest.CustomerId > 0)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " om.CustomerId = " + p_SearchRequest.CustomerId;
                    }
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FrontOrdersMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public FrontOrdersMaster GetOrderDetailByKey(long Id, string CurrencyCode)
        {
            FrontOrdersMaster objList = new FrontOrdersMaster();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    //dbo.CurrecncyConversion('" + CurrencyCode + @"',OM.Amount) as Amount,
                     //dbo.CurrecncyConversion('" + CurrencyCode + @"',OM.EBDAmount) as EBDAmount,OM.EBDName,
                     //                   dbo.CurrecncyConversion('" + CurrencyCode + @"',OM.DiscountAmount) as DiscountAmount,
                     //                  
                     //                   dbo.CurrecncyConversion('" + CurrencyCode + @"',OM.NetAmount) as NetAmount,
                     //                   dbo.CurrecncyConversion('" + CurrencyCode + @"',OM.RefundAmount) as RefundAmount,  
                    string _SqlQuery = @"select top 1 OM.Name,OM.Type,OM.IsSoloBooking,
                                        OM.Id,OM.OrderNo,OM.OrderDate,OM.Farmstaysid,OM.CustomerId,OM.CheckInDate,
                                        OM.CheckOutDate,OM.NoOfPeople,OM.Amount as Amount,
                                        OM.DiscountName,
                                        OM.EBDAmount as EBDAmount,OM.EBDName,
                                        OM.DiscountAmount as DiscountAmount,
                                        OM.NetAmount as NetAmount,
                                        OM.RefundAmount as RefundAmount,                                        
                                        OM.FarmStaysName,OM.CheckInTime,OM.CheckOutTime,
										OM.CancellationPolicyIsNonRefundable,OM.RefundablePercentage,OM.RefundableBeforDays,OM.CancellationReason,
                                        OM.MarkupPercentage,OM.GuestFirstName,OM.GuestLastName,
										OM.GuestEmail,OM.IsMale,OM.GuestCountryId,OM.GuestCity,OM.GuestMobile,
                                        OM.GuestAddress,OM.GuestPhone,OM.Status,OM.RefundAmount,
										OM.BaseCurrencyCode,OM.BaseCurrencyRate,OM.UpdatedDate,IM.RatePlanName 
                                        from OrderMaster OM 
                                        inner join InventoryMaster IM on (IM.OrderId = OM.Id) 
                                        where OM.Id=" + Id;
                                        

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteObject<FrontOrdersMaster>();

                    if (!objList.CancellationPolicyIsNonRefundable)
                    {
                        objList.IsApplyCancellationPolicy = (objList.CheckInDate - DateTime.Now.Date).Days > objList.RefundableBeforDays ? true : false;

                        if (objList.IsApplyCancellationPolicy)
                        {
                            objList.RefundAmount = (objList.NetAmount * objList.RefundablePercentage) / 100;
                        }
                    }
                     
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }
    }
}
