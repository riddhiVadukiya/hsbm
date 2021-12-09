using HSBM.Repository.Contracts;
using HSBM.EntityModel.DiscountMaster;
using System;
using BLToolkit.Data;
using System.Collections.Generic;
using HSBM.EntityModel.Common;
using HSBM.Common.Utils;
using System.Linq;
using System.Globalization;
namespace HSBM.Repository.Repositories
{
    public class DiscountMasterRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int AddOrUpdateDiscountMaster(DiscountMaster discountMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistDiscountName = CheckNameForDuplicate(_DbManager, discountMaster);

                    if (_IsExistDiscountName)
                    {
                        return 2;
                    }
                    else
                    {
                        long Affected = 0;
                        DateTime dtstart = DateTime.ParseExact(discountMaster.StartDate, Helper.DefaultDateFormat(),  CultureInfo.InvariantCulture);
                        DateTime dtend = DateTime.ParseExact(discountMaster.EndDate, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);

                        DateTime? dtBooked = null;

                        //if (discountMaster.BookbyDate!=null)
                        //    dtBooked = DateTime.ParseExact(discountMaster.BookbyDate, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);

                        #region Query
                        if (discountMaster.Id != 0)
                        {
                            _SqlQuery = @"
                                        UPDATE [dbo].[DiscountMaster] SET 
                                                        [Name] = @Name,
                                                        [IsPercentage] = @IsPercentage,
                                                        [IsEBO] = @IsEBO,
                                                        [DaysBeforeBooking] = @DaysBeforeBooking,
                                                        [StartDate] = @StartDate,
                                                        [EndDate] = @EndDate,
                                                        [DiscountValue] = @DiscountValue 
                                        WHERE [Id] = @Id";
                            Affected = _DbManager.SetCommand(_SqlQuery,
                           _DbManager.Parameter("@Id", discountMaster.Id),
                           _DbManager.Parameter("@Name", discountMaster.Name),
                           _DbManager.Parameter("@IsPercentage", discountMaster.IsPercentage),
                           _DbManager.Parameter("@IsEBO", discountMaster.IsEBO),
                           _DbManager.Parameter("@DaysBeforeBooking", discountMaster.DaysBeforeBooking),
                           _DbManager.Parameter("@StartDate", dtstart),
                           _DbManager.Parameter("@EndDate", dtend),
                           _DbManager.Parameter("@DiscountValue", discountMaster.DiscountValue)).ExecuteNonQuery();
                        }
                        else
                        {
                            _SqlQuery = @"
                                        INSERT INTO [dbo].[DiscountMaster](
                                                                    [Name],
                                                                    [IsPercentage],
                                                                    [IsEBO],
                                                                    [DaysBeforeBooking],
                                                                    [StartDate],
                                                                    [EndDate],
                                                                    [DiscountValue]
                                                                    ) OUTPUT Inserted.Id 
                                                                    VALUES 
                                                                    (
                                                                    @Name,
                                                                    @IsPercentage,
                                                                    @IsEBO,
                                                                    @DaysBeforeBooking,
                                                                    @StartDate,
                                                                    @EndDate,
                                                                    @DiscountValue)";

                            Affected = _DbManager.SetCommand(_SqlQuery,
                           _DbManager.Parameter("@Id", discountMaster.Id),
                           _DbManager.Parameter("@Name", discountMaster.Name),
                           _DbManager.Parameter("@IsPercentage", discountMaster.IsPercentage),
                           _DbManager.Parameter("@IsEBO", discountMaster.IsEBO),
                           _DbManager.Parameter("@DaysBeforeBooking", discountMaster.DaysBeforeBooking),
                           _DbManager.Parameter("@StartDate", dtstart),
                           _DbManager.Parameter("@EndDate", dtend),
                           _DbManager.Parameter("@DiscountValue", discountMaster.DiscountValue)).ExecuteScalar<int>();
                        }
                        #endregion

                        if (Affected > 0)
                        {
                            if (discountMaster.Id > 0)
                            {
                                Affected = discountMaster.Id;
                            }

                            AddDiscountHistory(Affected, discountMaster.SelectedFarmStays, _DbManager);
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private void AddDiscountHistory(long DiscountId, string FarmsIds, DbManager _DbManager)
        {
            try { _DbManager.SetCommand("Delete from DiscountHistory where DiscountId =" + DiscountId).ExecuteNonQuery(); }
            catch (Exception) { }

            if (FarmsIds != null)
            {
                string[] arr = FarmsIds.Split(',');

                foreach (var item in arr)
                {
                    string _SqlQuery = "insert into DiscountHistory (DiscountId,FarmstaysId)values(" + DiscountId + "," + item + ")";
                    try { _DbManager.SetCommand(_SqlQuery).ExecuteNonQuery(); }
                    catch (Exception) { }
                }
            }
        }

        private bool CheckNameForDuplicate(DbManager _DbManager, DiscountMaster discountMaster)
        {
            try
            {
                String _SqlQuery = @"SELECT COUNT([Id]) FROM [dbo].[DiscountMaster] 
                                    WHERE [Id] != @Id AND [Name] = @Name AND EndDate >= getdate() ";

                int _ExistCount = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", discountMaster.Id),
                    _DbManager.Parameter("@Name", discountMaster.Name)).ExecuteScalar<int>();

                if (_ExistCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DiscountMaster GetDiscountMasterById(long Id)
        {
            DiscountMaster discountMaster = new DiscountMaster();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT [Id],[Name],[IsPercentage],[IsEBO],[DaysBeforeBooking],[StartDate],[EndDate],[DiscountValue] FROM [dbo].[DiscountMaster] where [Id] = @Id ";

                    discountMaster = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<DiscountMaster>().FirstOrDefault();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return discountMaster;
        }

        public List<DiscountMasterResponse> GetAllDiscountMastersForDropDown()
        {
            List<DiscountMasterResponse> objList = new List<DiscountMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" SELECT [Id],[Name],[IsPercentage],[IsEBO],[DaysBeforeBooking],[StartDate],[EndDate],[DiscountValue] FROM [dbo].[DiscountMaster]";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<DiscountMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public void DeleteDiscountMaster(int Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"DELETE FROM [dbo].[DiscountHistory] WHERE [DiscountId] = @Id";
                    _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                    _SqlQuery = @"DELETE FROM [dbo].[DiscountMaster] WHERE [Id] = @Id";
                    _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public GridDataResponse GetAllDiscountMastersBySearchRequest(GridParams p_GridParams, DiscountMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" SELECT *, RecordsTotal = COUNT(*) OVER() from [dbo].[DiscountMaster] where IsEBO=0";

                    bool IsWhereClauseEmpty = false;

                    if (p_Request.Name != null && p_Request.Name != string.Empty)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" [Name] LIKE '%" + p_Request.Name + @"%'";
                    }

                    if (p_Request.IsPreviousDiscounts == false)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @" EndDate >= getdate()";
                    }
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<DiscountMasterResponse>();
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

        public List<DiscountHistoryResponse> GetDiscountHistoryByid(long DiscountId)
        {
            List<DiscountHistoryResponse> objList = new List<DiscountHistoryResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" select f.id as FarmId,F.Name as FarmsName ,F.CategoryId ,IsApplied = (SELECT CASE WHEN EXISTS (select id from DiscountHistory where DiscountId = " + DiscountId + @" and FarmstaysId = f.id)THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END ) from FarmStays f where IsActive = 1 ";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<DiscountHistoryResponse>();
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