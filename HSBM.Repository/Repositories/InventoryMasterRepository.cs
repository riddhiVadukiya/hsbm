using HSBM.Repository.Contracts;
using HSBM.EntityModel.InventoryMaster;
using System;
using BLToolkit.Data;
using System.Globalization;
using HSBM.EntityModel.Common;
using HSBM.Common.Utils;
using System.Linq;
using System.Collections.Generic;
using HSBM.Common.Enums;

namespace HSBM.Repository.Repositories
{
    public class InventoryMasterRepository
    {
        public int AddInventoryMaster(InventoryMaster inventoryMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    var start = DateTime.ParseExact(inventoryMaster.StartDate, Helper.DefaultDateFormat(),  CultureInfo.InvariantCulture);
                    var end = DateTime.ParseExact(inventoryMaster.EndDate, Helper.DefaultDateFormat(),  CultureInfo.InvariantCulture);

                    for (var day = start.Date; day <= end; day = day.AddDays(1))
                    {
                        _SqlQuery = @"INSERT INTO [dbo].[InventoryMaster]([FarmstaysId],[BookingDate],[NumberOfProperty],[OnSite],[OrderId],[BookingGroupId]) VALUES (@FarmstaysId,@BookingDate,@NumberOfProperty,@OnSite,@OrderId,@BookingGroupId)";

                        var Affected = _DbManager.SetCommand(_SqlQuery,
                           _DbManager.Parameter("@FarmstaysId", inventoryMaster.FarmstaysId),
                           _DbManager.Parameter("@BookingDate", day),
                           _DbManager.Parameter("@NumberOfProperty", inventoryMaster.NumberOfProperty),
                           _DbManager.Parameter("@OnSite", inventoryMaster.OnSite),
                           _DbManager.Parameter("@OrderId", inventoryMaster.OrderId),
                           _DbManager.Parameter("@BookingGroupId", inventoryMaster.BookingGroupId)
                            ).ExecuteNonQuery();
                    }

                    return 1;


                }
            }
            catch (Exception _Exception)
            {
            }
            return 0;
        }

        public GridDataResponse GetAllInventoryMasterBySearchRequest(GridParams p_GridParams, InventoryMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"  select BookingGroupId,MIN(BookingDate) as StartDate, MAX(BookingDate) as EndDate,FarmstaysId,NumberOfProperty,OrderId,OrderNo from 
                                            (Select i.*,o.OrderNo,ROW_NUMBER() OVER(PARTITION BY BookingGroupId ORDER BY BookingDate asc) as ranking from InventoryMaster i  
                                            left join OrderMaster o on i.OrderId=o.Id) t  where FarmstaysId = "+ p_Request.Id +@"
                                            group by BookingGroupId,FarmstaysId,NumberOfProperty,OrderId,OrderNo ";

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<InventoryMasterResponse>();
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

        public void DeleteInventoryMaster(Guid Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"DELETE FROM [dbo].[InventoryMaster] WHERE [BookingGroupId] = '" + Id.ToString() + "'";
                    _DbManager.SetCommand(_SqlQuery).ExecuteNonQuery();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void DeleteInventoryByOrderId(int Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"DELETE FROM [dbo].[InventoryMaster] WHERE [OrderId] = '" + Id.ToString() + "'";
                    _DbManager.SetCommand(_SqlQuery).ExecuteNonQuery();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public List<InventoryAvailable> GetInventoryAvailableByDate(DateTime StartDate, DateTime EndDate, long FarmStaysId)
        {
            List<InventoryAvailable> lst = new List<InventoryAvailable>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                   lst = _DbManager.SetSpCommand("spCheckAvailabilityForInventoryAdmin",
                        _DbManager.Parameter("@StartDT", StartDate),
                        _DbManager.Parameter("@EndDT", EndDate),
                        _DbManager.Parameter("@FarmStaysId", FarmStaysId)).ExecuteList<InventoryAvailable>();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return lst;

        }

        public GridDataResponse GetAllUpCommingOrder(GridParams p_GridParams, InventoryMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select ID,OrderNo,CheckInDate,CheckOutDate,NoOfPeople from OrderMaster where status=@status and cast( CheckInDate as date)>= cast( GETDATE() as date) and Farmstaysid=" + p_Request.FarmstaysId;
                                   

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@status",(int)BookingStatus.CONFIRMED)).ExecuteList<UpCommingOrder>();
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