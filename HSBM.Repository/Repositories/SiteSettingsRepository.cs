using HSBM.Repository.Contracts;
using HSBM.EntityModel.SiteSettings;
using System.Collections.Generic;
using BLToolkit.Data;
using HSBM.Common.Utils;
using System.Linq;
using System;

namespace HSBM.Repository.Repositories
{
    public class SiteSettingsRepository
    {
        public List<SiteSettingResponse> GetAllSiteSettingBySearchRequest(GridParams p_GridParams, SiteSettingsRequest p_SearchRequest)
        {
            List<SiteSettingResponse> objList = new List<SiteSettingResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Name],[SiteSettingId],[Value],[IsActive],RecordsTotal = COUNT(*) OVER() from [dbo].[SiteSettings]";

                    bool IsWhereClauseEmpty = true;

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " [IsActive] = 1 ";
                    //}
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<SiteSettingResponse>();
                    if (objList.Any()) 
                    {
                        objList.ForEach(t => 
                        {
                            t.Name = Helper.GetEnumDescription((Common.Enums.SiteSettingEnum)t.SiteSettingId);
                        });
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public SiteSettings GetSiteSettingRecordById(long Id)
        {
            SiteSettings _SiteSettings = new SiteSettings();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Name],[SiteSettingId],[Value],[IsActive],CreatedDate,CreatedBy, RecordsTotal = COUNT(*) OVER() from [dbo].[SiteSettings] where Id = @Id";
                    _SiteSettings = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", Id)).ExecuteList<SiteSettings>().FirstOrDefault();

                    if (_SiteSettings != null)
                    {
                        _SiteSettings.Name = Helper.GetEnumDescription((Common.Enums.SiteSettingEnum)_SiteSettings.SiteSettingId);
                    }

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _SiteSettings;
        }

        public int AddorUpdateSiteSetting(SiteSettings siteSettings)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistSSName = CheckNameForDuplicate(_DbManager, siteSettings);

                    if (_IsExistSSName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (siteSettings.Id != 0)
                        {
                            _SqlQuery = @"
update SiteSettings set 
SiteSettingId = @SiteSettingId
,Value = @Value
,IsActive = @IsActive
,UpdatedBy = @UpdatedBy
,UpdateDate = @UpdateDate
 where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"
insert into SiteSettings
     (SiteSettingId
     ,Value
     ,IsActive
     ,CreatedBy
     ,CreatedDate
     ,UpdatedBy
     ,UpdateDate)
values
     (@SiteSettingId
     ,@Value
     ,@IsActive
     ,@CreatedBy
     ,@CreatedDate
     ,@UpdatedBy
     ,@UpdateDate)";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@SiteSettingId", siteSettings.SiteSettingId),
                            _DbManager.Parameter("@Value", siteSettings.Value),
                            _DbManager.Parameter("@IsActive", siteSettings.IsActive),
                            _DbManager.Parameter("@CreatedBy", siteSettings.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", siteSettings.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", siteSettings.UpdatedBy),
                            _DbManager.Parameter("@UpdateDate", siteSettings.UpdateDate),
                            _DbManager.Parameter("@Id", siteSettings.Id)).ExecuteNonQuery();

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
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool CheckNameForDuplicate(DbManager _DbManager, SiteSettings siteSettings)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from SiteSettings where Id!=@Id and Name=@Name";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", siteSettings.Id),
                    _DbManager.Parameter("@Name", siteSettings.Name)).ExecuteScalar<int>();

                if (Affected > 0)
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

        public void DeleteSiteSettingById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Delete from SiteSettings where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                         _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void ActiveAndInactiveSwitchUpdateForSS(SiteSettings siteSettings)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update SiteSettings set IsActive = @IsActive where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                         _DbManager.Parameter("@Id", siteSettings.Id),
                         _DbManager.Parameter("IsActive", siteSettings.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<SiteSettings> GetSiteSettingRecord()
        {
            List<SiteSettings> _SiteSettings = new List<SiteSettings>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Name],[SiteSettingId],[Value],[IsActive],CreatedDate,CreatedBy, RecordsTotal = COUNT(*) OVER() from [dbo].[SiteSettings] where IsActive = 1";
                    _SiteSettings = _DbManager.SetCommand(_SqlQuery).ExecuteList<SiteSettings>().ToList();

                    if (_SiteSettings != null)
                    {                        
                        return _SiteSettings;
                    }

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _SiteSettings;
        }

        public SiteSettings GetSiteSettingRecordForFront(long Id)
        {
            SiteSettings _SiteSettings = new SiteSettings();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Name],[SiteSettingId],[Value],[IsActive],CreatedDate,CreatedBy, RecordsTotal = COUNT(*) OVER() from [dbo].[SiteSettings] where SiteSettingId = @SiteSettingId";
                    _SiteSettings = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@SiteSettingId", Id)).ExecuteList<SiteSettings>().FirstOrDefault();

                    if (_SiteSettings != null)
                    {
                        _SiteSettings.Name = Helper.GetEnumDescription((Common.Enums.SiteSettingEnum)_SiteSettings.SiteSettingId);
                    }

                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _SiteSettings;
        }

    
    }
}