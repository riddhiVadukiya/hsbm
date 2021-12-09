using BLToolkit.Data;
using HSBM.EntityModel.FrontReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Repository.Repositories
{
    public class FrontFarmStaysReviewRepository
    {
        public FarmStaysRatingsAndReviewResponse GetReviewDetailByOrderNo(string OrderNo)
        {
            FarmStaysRatingsAndReviewResponse _FarmStaysRatingsAndReviewResponse = new FarmStaysRatingsAndReviewResponse();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT * FROM FarmStaysRatingsAndReview
                                        where OrderNo = '" + OrderNo + "'";
                    _FarmStaysRatingsAndReviewResponse = _DbManager.SetCommand(_SqlQuery)
                    .ExecuteList<FarmStaysRatingsAndReviewResponse>().FirstOrDefault();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _FarmStaysRatingsAndReviewResponse;
        }

        public int AddRateAndReviews(FarmStaysRatingsAndReviewRequest obj)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
//                    String _SqlQuery = @"Update FarmStaysRatingsAndReview
//                                        set Ratings=@Ratings,Reviews=@Reviews,CreatedDate=@CreatedDate,IsApproved=@IsApproved,
//                                        Location=@Location, Cleanliness=@Cleanliness, 
//                                        ValueForMoney=@ValueForMoney, Hospitality=@Hospitality 
//                                        where FarmStaysRatingsAndReviewGUID = '" + obj.FarmStaysRatingsAndReviewGUID + "'";

                    string _InsertQuery = @"INSERT INTO [dbo].[FarmStaysRatingsAndReview]
           ([FarmStyasId]
           ,[Customerid]
           ,[Ratings]
           ,[Reviews]
           ,[OrderNo]
           ,[FarmStaysRatingsAndReviewGUID]
           ,[CreatedDate]
           ,[Location]
           ,[Cleanliness]
           ,[ValueForMoney]
           ,[Hospitality])
     VALUES
           (@FarmStyasId,
            @Customerid,
            @Ratings,
            @Reviews,
            @OrderNo,
            @FarmStaysRatingsAndReviewGUID,
            @CreatedDate,
            @Location,
            @Cleanliness,
            @ValueForMoney,
            @Hospitality)";

                    int Affected = _DbManager.SetCommand(_InsertQuery,
                            _DbManager.Parameter("@FarmStyasId", obj.FarmStyasId),
                            _DbManager.Parameter("@Customerid", obj.Customerid),
                            _DbManager.Parameter("@OrderNo", obj.OrderNo),
                            _DbManager.Parameter("@FarmStaysRatingsAndReviewGUID", obj.FarmStaysRatingsAndReviewGUID),
                            _DbManager.Parameter("@Ratings", obj.Ratings),
                            _DbManager.Parameter("@Reviews", obj.Reviews),
                            _DbManager.Parameter("@Location", obj.Location),
                            _DbManager.Parameter("@Cleanliness", obj.Cleanliness),
                            _DbManager.Parameter("@ValueForMoney", obj.ValueForMoney),
                            _DbManager.Parameter("@Hospitality", obj.Hospitality),
                            _DbManager.Parameter("@CreatedDate", DateTime.Now)
                            ).ExecuteNonQuery();

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

            return 0;
        }


        public string GetFarmStayNameById(long p_Id)
        {
            string FarmstayName = string.Empty;
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"select Name from FarmStays where Id = "+ p_Id;
                   FarmstayName = _DbManager.SetCommand(_SqlQuery).ExecuteScalar<string>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return FarmstayName;
        }
    }
}
