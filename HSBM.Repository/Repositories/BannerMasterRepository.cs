using HSBM.Repository.Contracts;
using HSBM.EntityModel.BannerMaster;
using HSBM.Common.Utils;
using System.Collections.Generic;
using BLToolkit.Data;
using System.Linq;
using System;

namespace HSBM.Repository.Repositories
{
    public class BannerMasterRepository
    {
        public List<BannerMasterResponse> GetAllBannerMasterBySearchRequest(GridParams p_GridParams, BannerMasterRequest p_SearchRequest)
        {
            List<BannerMasterResponse> objList = new List<BannerMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Title],[ImageName],[Alt],[OrderIndex],[IsActive],RecordsTotal = COUNT(*) OVER() from [dbo].[BannerMaster]";

                    bool IsWhereClauseEmpty = true;

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " [IsActive] = 1 ";
                    //}
                    //if (p_GridParams != null)
                    //{
                    //    _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    //}

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<BannerMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public List<BannerMasterResponse> GetAllBanners()
        {
            List<BannerMasterResponse> objList = new List<BannerMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"
SELECT 
    [Id], [ImageOrignalName], [ImageName], [Alt], [OrderIndex], [IsActive],
    RecordsTotal = COUNT(*) OVER() 
FROM [dbo].[BannerMaster]
WHERE [IsActive] = 'true'";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<BannerMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public BannerMaster GetBannerRecordById(long Id)
        {
            BannerMaster _BannerMaster = new BannerMaster();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Title],[ImageName],[Alt],[OrderIndex],[IsActive],CreatedDate,CreatedBy, RecordsTotal = COUNT(*) OVER() from [dbo].[BannerMaster] where Id = @Id";

                    _BannerMaster = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<BannerMaster>().FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _BannerMaster;
        }

        public int AddorUpdateBanner(List<BannerMaster> ListofBannerMaster)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    //bool _IsExistCatAName = CheckNameForDuplicate(_DbManager, categoryA);

                    //if (_IsExistCatAName)
                    //{
                    //    return 2;
                    //}
                    //else
                    //{
                    foreach (var bannerMaster in ListofBannerMaster)
                    {
                        if (bannerMaster.Id != 0)
                        {
                            _SqlQuery = @"
                    update BannerMaster set ImageName=@ImageName                   
                    ,Title = @Title                  
                    ,IsActive = @IsActive
                    ,UpdatedBy = @UpdatedBy
                    ,UpdateDate = @UpdateDate
                     where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"
                        insert into BannerMaster
                            (ImageName
                            ,Title                           
                            ,IsActive
                            ,CreatedBy
                            ,CreatedDate
                            )
                        values
                            (@ImageName
                            ,@Title
                            ,1
                            ,@CreatedBy
                            ,@CreatedDate)";
                        }
                        //foreach (var bannerMaster in ListofBannerMaster)
                        //{
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@ImageName", bannerMaster.ImageName),
                            _DbManager.Parameter("@Title", bannerMaster.Title),
                            _DbManager.Parameter("@IsActive", bannerMaster.IsActive),
                            _DbManager.Parameter("@CreatedBy", bannerMaster.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", bannerMaster.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", bannerMaster.UpdatedBy),
                            _DbManager.Parameter("@UpdateDate", bannerMaster.UpdateDate),
                            _DbManager.Parameter("@Id", bannerMaster.Id)).ExecuteNonQuery();

                        if (Affected <= 0)
                        {
                            return 0;
                        }
                    }
                  //  }

                    return 1;

                    //}
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void DeleteBannerById(long Id)
        {
            try
            {
                using (DbManager _DbMAnager = new DbManager())
                {
                    String _SqlQuery = @"Delete BannerMaster where Id = @Id";

                    _DbMAnager.SetCommand(_SqlQuery,
                        _DbMAnager.Parameter("@Id", Id)).ExecuteNonQuery();
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        public void ActiveAndInactiveSwitchUpdateForBanner(BannerMaster bannerMaster)
        {
            try
            {
                using (DbManager _DbMAnager = new DbManager())
                {
                    String _SqlQuery = @"Update BannerMaster set IsActive = @IsActive where Id = @Id";

                    _DbMAnager.SetCommand(_SqlQuery,
                        _DbMAnager.Parameter("@Id", bannerMaster.Id),
                        _DbMAnager.Parameter("@IsActive", bannerMaster.IsActive)).ExecuteNonQuery();
                }

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}