using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.FarmStays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Repository.Repositories
{
    public class FarmStaysRatingsAndReviewRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public GridDataResponse GetAllFarmStaysRatingsAndReviewBySearchRequest(GridParams p_GridParams, FarmStaysRatingsAndReviewRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @" select 
                                            Id,
                                            (select FS.Name from FarmStays FS where FS.Id=FarmStyasId) as FarmStyasName,
                                            (select SU.UserName from SystemUsers SU where SU.Id=CustomerId) as ReviewBy,
                                            CreatedDate as ReviewDate,
                                            CreatedDate as strReviewDate,
                                            (select SU.UserName from SystemUsers SU where SU.Id=ApprovedById) as ApprovedBy,
                                            ApprovedDate,
                                            ApprovedDate as strApprovedDate,
                                            IsApproved, RecordsTotal = COUNT(*) OVER() from  FarmStaysRatingsAndReview";

                    bool IsWhereClauseEmpty = true;

                    if (p_Request.FarmStyasId != null && p_Request.FarmStyasId >0)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;

                        _SqlQuery += @"FarmStyasId=" + p_Request.FarmStyasId + @"";
                    }
                           
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    var objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStaysRatingsAndReviewResponse>();
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

        public FarmStaysRatingsAndReviewResponse GetRatingsAndReviewDetailByKey(long Id)
        {
            FarmStaysRatingsAndReviewResponse objList = new FarmStaysRatingsAndReviewResponse();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select 
                                        (select FS.Name from FarmStays FS where FS.Id=FarmStyasId) as FarmStyasName,
                                        (select SU.UserName from SystemUsers SU where SU.Id=CustomerId) as ReviewBy,
                                        CreatedDate as ReviewDate,
                                        Reviews,
                                        Ratings,
                                        Location,
                                        Cleanliness,
                                        ValueForMoney,
                                        Hospitality,
                                        IsApproved
                                        from FarmStaysRatingsAndReview where Id=" + Id;
                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteObject<FarmStaysRatingsAndReviewResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public bool AprroveAndUnapproveRatingsAndReview(long Id, long ApprovedById)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"update FarmStaysRatingsAndReview set IsApproved=( case when IsApproved=1 then 0 else 1 end ),ApprovedById=" + ApprovedById + ",ApprovedDate='" + DateTime.Now.ToString("yyyy-MMM-dd") + "' where Id=" + Id;
                    if (_DbManager.SetCommand(_SqlQuery).ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return false;
        }
    }
}
