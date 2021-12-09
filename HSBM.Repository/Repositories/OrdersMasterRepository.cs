using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.OrdersMaster;
using HSBM.EntityModel.InventoryMaster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Repository.Repositories
{
    public class OrdersMasterRepository
    {

        public List<OrdersMasterResponse> GetAllOrdersMasterBySearchRequest(GridParams p_GridParams, OrdersMasterRequest p_SearchRequest)
        {
            List<OrdersMasterResponse> objList = new List<OrdersMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT Id,OrderNo,OrderDate,FarmStaysName,NetAmount,Status,Farmstaysid,GuestFirstName,GuestLastName,(CASE WHEN OTA Is Null THEN 'Himalayan' ELSE OTA END) AS OTA ,RecordsTotal = COUNT(*) OVER()  FROM [dbo].[OrderMaster]";

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
                        _SqlQuery += " [OrderDate] >= '" + CreatedDateFrom.ToString("dd/MMM/yyyy") + "' ";
                    }
                    if (Convert.ToDateTime(CreatedDateTo) != DateTime.MinValue)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " [OrderDate] <= '" + CreatedDateTo.ToString("dd/MMM/yyyy") + "'";
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.OrderNo))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " OrderNo Like  '%" + p_SearchRequest.OrderNo.Trim() + "%'";
                    }

                    if (!string.IsNullOrEmpty(p_SearchRequest.CustomerName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += "(GuestFirstName +' '+GuestLastName) like '%" + p_SearchRequest.CustomerName.Trim() + "%'";
                        //_SqlQuery += " GuestFirstName like '%" + p_SearchRequest.CustomerName + "%' OR  GuestLastName like '%" + p_SearchRequest.CustomerName + "%'";
                    }
                    if (!string.IsNullOrEmpty(p_SearchRequest.FarmStaysName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " FarmStaysName like '%" + p_SearchRequest.FarmStaysName+"%'";
                    }
                    if (p_SearchRequest.OrderStatusId > 0)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " Status = " + p_SearchRequest.OrderStatusId;
                    }

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<OrdersMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public OrdersMaster GetOrderDetailByKey(long Id)
        {
            OrdersMaster objList = new OrdersMaster();
            List<InventoryMaster> inventoryList = new List<InventoryMaster>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select top 1 OM.* from  OrderMaster OM where OM.Id=" + Id;
                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteObject<OrdersMaster>();
                    if (!objList.CancellationPolicyIsNonRefundable)
                    {
                        objList.IsApplyCancellationPolicy = (objList.CheckInDate - DateTime.Now.Date).Days > objList.RefundableBeforDays ? true : false;

                        if (objList.IsApplyCancellationPolicy)
                        {
                            objList.RefundAmount = (objList.NetAmount * objList.RefundablePercentage) / 100;
                        }
                    }
                    string _SqlRoomsQuery = @"select * from  InventoryMaster where OrderId=@OrderId";
                    inventoryList = _DbManager.SetCommand(_SqlRoomsQuery,
                                _DbManager.Parameter("@OrderId", Id)).ExecuteList<InventoryMaster>();
                    if (inventoryList.Count() > 0)
                    {
                        objList.inventoryList = inventoryList;
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
