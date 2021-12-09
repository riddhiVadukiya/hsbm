using HSBM.Repository.Contracts;
using HSBM.EntityModel.CMSPageMaster;
using System.Collections.Generic;
using HSBM.Common.Utils;
using BLToolkit.Data;
using System;
using System.Linq;

namespace HSBM.Repository.Repositories
{
    public class CMSPageMasterRepository
    {
        /// <summary>
        /// Grid of CMSPageMaster
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">CMSPageMasterRequest</param>
        /// <returns> List of CMSPageMasterResponse </returns>
        public List<CMSPageMasterResponse> GetAllCMSPageMasterBySearchRequest(GridParams p_GridParams, CMSPageMasterRequest p_SearchRequest)
        {
            List<CMSPageMasterResponse> objList = new List<CMSPageMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[CMSPageId],[Name],[Title],[Description],[MetaTitle],[MetaKeywords],[MetaDescription],[IsActive],RecordsTotal = COUNT(*) OVER() from [dbo].[CMSPageMaster]";

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

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<CMSPageMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        /// <summary>
        /// Add or Update CMSPage
        /// </summary>
        /// <param name="cmsPageMaster">CMSPageMaster</param>
        /// <returns> integer </returns>
        public int AddorUpdateCMSPage(CMSPageMaster cmsPageMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistCMSPageName = CheckNameForDuplicate(_DbManager, cmsPageMaster);

                    if (_IsExistCMSPageName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (cmsPageMaster.Id != 0)
                        {
                            _SqlQuery = @"
update CMSPageMaster set Name=@Name
, CMSPageId = @CMSPageId
,Title = @Title
,Description = @Description
,MetaTitle = @MetaTitle
,MetaKeywords = @MetaKeywords
,MetaDescription = @MetaDescription
,IsActive = @IsActive
,UpdatedBy = @UpdatedBy
,UpdateDate = @UpdateDate
 where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"
insert into CMSPageMaster
    (Name
    ,CMSPageId    
    ,Title
    ,Description
    ,MetaTitle
    ,MetaKeywords
    ,MetaDescription
    ,IsActive
    ,CreatedBy
    ,CreatedDate
    ,UpdatedBy
    ,UpdateDate)
values
    (@Name
    ,@CMSPageId
    ,@Title
    ,@Description
    ,@MetaTitle
    ,@MetaKeywords
    ,@MetaDescription
    ,@IsActive
    ,@CreatedBy
    ,@CreatedDate
    ,@UpdatedBy
    ,@UpdateDate)";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Name", cmsPageMaster.Name),
                            _DbManager.Parameter("@CMSPageId", cmsPageMaster.CMSPageId),
                            _DbManager.Parameter("@Title", cmsPageMaster.Title),
                            _DbManager.Parameter("@Description", cmsPageMaster.Description),
                            _DbManager.Parameter("@MetaTitle", cmsPageMaster.MetaTitle),
                            _DbManager.Parameter("@MetaKeywords", cmsPageMaster.MetaKeywords),
                            _DbManager.Parameter("@MetaDescription", cmsPageMaster.MetaDescription),
                            _DbManager.Parameter("@IsActive", cmsPageMaster.IsActive),
                            _DbManager.Parameter("@CreatedBy", cmsPageMaster.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", cmsPageMaster.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", cmsPageMaster.UpdatedBy),
                            _DbManager.Parameter("@UpdateDate", cmsPageMaster.UpdateDate),
                            _DbManager.Parameter("@Id", cmsPageMaster.Id)).ExecuteNonQuery();

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

        /// <summary>
        /// Duplication check for CMS Page Name
        /// </summary>
        /// <param name="_DbManager">DbManager</param>
        /// <param name="cmsPageMaster">CMSPageMaster</param>
        /// <returns> true/false </returns>
        public bool CheckNameForDuplicate(DbManager _DbManager, CMSPageMaster cmsPageMaster)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from CMSPageMaster where Id!=@Id and Name=@Name";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", cmsPageMaster.Id),
                    _DbManager.Parameter("@Name", cmsPageMaster.Name)).ExecuteScalar<int>();

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

        /// <summary>
        /// Get CMS PageRecord By Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of CMSPageMaster </returns>
        public CMSPageMaster GetCMSPageRecordById(long Id)
        {
            CMSPageMaster _CMSPageMaster = new CMSPageMaster();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT [Id],[CMSPageId],[Name],[Title],[Description],[MetaTitle],[MetaKeywords],[MetaDescription],[IsActive],CreatedDate,CreatedBy,RecordsTotal = COUNT(*) OVER() from [dbo].[CMSPageMaster] where Id = @Id";

                    _CMSPageMaster = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<CMSPageMaster>().FirstOrDefault();

                    return _CMSPageMaster;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Delete CMSPage By Id
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteCMSPageById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Delete CMSPageMaster where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Active/Inactive CMSPage
        /// </summary>
        /// <param name="cMSPageMaster"> Object of CMSPageMaster </param>
        public void ActiveAndInactiveSwitchUpdateForCMSPage(CMSPageMaster cMSPageMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update CMSPageMaster set IsActive = @IsActive where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", cMSPageMaster.Id),
                        _DbManager.Parameter("@IsActive", cMSPageMaster.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public CMSPageMaster GetCMSPageRecordByIdForFront(long Id)
        {
            CMSPageMaster _CMSPageMaster = new CMSPageMaster();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT [Id],[CMSPageId],[Name],[Title],[Description],[MetaTitle],[MetaKeywords],[MetaDescription],[IsActive],CreatedDate,CreatedBy,RecordsTotal = COUNT(*) OVER() from [dbo].[CMSPageMaster] where CMSPageId = @Id";

                    _CMSPageMaster = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<CMSPageMaster>().FirstOrDefault();

                    return _CMSPageMaster;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}