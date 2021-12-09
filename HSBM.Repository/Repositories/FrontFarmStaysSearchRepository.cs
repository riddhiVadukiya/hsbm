using BLToolkit.Data;
using HSBM.Common.Utils;
using HSBM.EntityModel.Front;
using HSBM.EntityModel.FrontEnd;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Repository.Repositories
{
    public class FrontFarmStaysSearchRepository
    {

        public SearchFarmStaysResult GetFarmStayslist(SearchFarmStaysRequest req)
        {
            

            SearchFarmStaysResult _searchFarmStaysResult = new SearchFarmStaysResult();

            List<SearchFarmStays> lst = new List<SearchFarmStays>();
            try
            {
                DateTime dtCheckIn = DateTime.ParseExact(req.CheckIn, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                DateTime dtCheckOut = DateTime.ParseExact(req.CheckOut, Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);

                using (DbManager _DbManager = new DbManager())
                {
                    lst = _DbManager.SetSpCommand("spSearchFarmstays",
                         _DbManager.Parameter("@CheckInDate", dtCheckIn),
                         _DbManager.Parameter("@CheckOutDate", dtCheckOut),
                         _DbManager.Parameter("@NoOfGuest", req.Guests),
                         _DbManager.Parameter("@IsSolo", req.IsSolo),
                         _DbManager.Parameter("@IsPackage", req.IsPackage),
                         _DbManager.Parameter("@CurrencyCode", req.CurrencyCode),
                         _DbManager.Parameter("@NoOfChild", req.Child)
                         ).ExecuteList<SearchFarmStays>();

                    if (lst.Any())
                    {
                        _searchFarmStaysResult.SearchFarmStays = lst;
                        _searchFarmStaysResult.AmenityFilter = GetAmenityWithCount(req.IsPackage);
                        _searchFarmStaysResult.CategoryFilter = GetCategoryWithCount(req.IsPackage);
                        _searchFarmStaysResult.PriceFilter = GetPricesWithCount(lst);
                        _searchFarmStaysResult.RatingsFilter = GetRatingWithCount(lst);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _searchFarmStaysResult;

        }

        private List<AmenityFilter> GetAmenityWithCount(bool IsPackage)
        {
            List<AmenityFilter> lst = new List<AmenityFilter>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    lst = _DbManager.SetCommand(@"select AmenityId as Id,AmenityName as Name,count(AmenityId) as  [Count],
                                                        STUFF((SELECT ', ' + CAST(FarmStaysId AS VARCHAR(10)) FROM FarmStaysAmenities 
                                                                 WHERE AmenityId = fa.AmenityId
                                                                 FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'),1,2,' ') as FamrstaysId 
                                                        from FarmStaysAmenities fa 
                                                        inner join FarmStays fs on fa.FarmStaysId = fs.id and fs.IsPackage='"+IsPackage+
                                                        @"' inner join AmenityMaster am on fa.AmenityId = am.Id 
                                                        where fs.IsActive = 1 group by AmenityId,AmenityName").ExecuteList<AmenityFilter>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }

        private List<AmenityFilter> GetCategoryWithCount(bool IsPackage)
        {
            List<AmenityFilter> lst = new List<AmenityFilter>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    lst = _DbManager.SetCommand(@"select fc.CategoryId as Id,CategoryName as Name,count(fc.CategoryId) as  [Count],
                                                        STUFF((SELECT ', ' + CAST(FarmStaysId AS VARCHAR(10)) FROM FarmStaysCategories 
                                                                 WHERE CategoryId = fc.CategoryId
                                                                 FOR XML PATH(''), TYPE).value('.','NVARCHAR(MAX)'),1,2,' ') as FamrstaysId 
                                                        from FarmStaysCategories fc 
                                                        inner join FarmStays fs on fc.FarmStaysId = fs.id and fs.IsPackage='" + IsPackage +
                                                        @"' inner join CategoryMaster cm on fc.CategoryId = cm.Id 
                                                        where fs.IsActive = 1 group by fc.CategoryId,CategoryName").ExecuteList<AmenityFilter>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }

        private List<PriceFilter> GetPricesWithCount(List<SearchFarmStays> farmstaysList)
        {
            List<PriceFilter> lst = new List<PriceFilter>();

            try
            {
                //0 to 1000
                lst.Add(new PriceFilter() { Name = "Under ||999 per day", StartPrice = 0, EndPrice = 999, Count = farmstaysList.Where(t =>( t.NetPrice > 0 ? (t.NetPrice <= 999 && t.NetPrice != 0) : (t.Price <= 999 && t.Price != 0))).Count() });
                lst.Add(new PriceFilter() { Name = "||1000 to ||4999 per day", StartPrice = 1000, EndPrice = 4999, Count = farmstaysList.Where(t => (t.NetPrice > 0 ? (t.NetPrice >= 1000 && t.NetPrice <= 4999 && t.NetPrice != 0) : (t.Price >= 1000 && t.Price <= 4999 && t.Price != 0))).Count() });
                lst.Add(new PriceFilter() { Name = "||5000 to ||9999 per day", StartPrice = 5000, EndPrice = 9999, Count = farmstaysList.Where(t => (t.NetPrice > 0 ? (t.NetPrice >= 5000 && t.NetPrice <= 9999 && t.NetPrice != 0) : (t.Price >= 5000 && t.Price <= 9999 && t.Price != 0))).Count() });
                lst.Add(new PriceFilter() { Name = "||10000 to  ||19999 per day", StartPrice = 10000, EndPrice = 19999, Count = farmstaysList.Where(t => (t.NetPrice > 0 ? (t.NetPrice >= 10000 && t.NetPrice <= 19999 && t.NetPrice != 0) : (t.Price >= 10000 && t.Price <= 19999 && t.Price != 0))).Count() });
                lst.Add(new PriceFilter() { Name = "Above ||20000 per day", StartPrice = 20000, EndPrice = int.MaxValue, Count = farmstaysList.Where(t => (t.NetPrice > 0 ? (t.NetPrice >= 20000 && t.NetPrice != 0) : (t.Price >= 20000 && t.Price != 0))).Count() });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst;
        }

        private List<RatingsFilter> GetRatingWithCount(List<SearchFarmStays> farmstaysList)
        {
            List<RatingsFilter> lst = new List<RatingsFilter>();

            try
            {
                //0 to 1000                
                lst.Add(new RatingsFilter() { Name = "1", Rating = 1, FullName = "1 Star", Count = farmstaysList.Where(t => t.Ratings == 1).Count() });
                lst.Add(new RatingsFilter() { Name = "2", Rating = 2, FullName = "2 Star", Count = farmstaysList.Where(t => t.Ratings == 2).Count() });
                lst.Add(new RatingsFilter() { Name = "3", Rating = 3, FullName = "3 Star", Count = farmstaysList.Where(t => t.Ratings == 3).Count() });
                lst.Add(new RatingsFilter() { Name = "4", Rating = 4, FullName = "4 Star", Count = farmstaysList.Where(t => t.Ratings == 4).Count() });
                lst.Add(new RatingsFilter() { Name = "5", Rating = 5, FullName = "5 Star", Count = farmstaysList.Where(t => t.Ratings == 5).Count() });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lst;
        }

        public List<SelectListItem> GetFarmStaysForDropDown()
        {
            List<SelectListItem> ListFarmStays = new List<SelectListItem>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"SELECT Id as Value,Name as Text FROM [dbo].[FarmStays] WHERE IsActive = 1 ";
                    ListFarmStays = _DbManager.SetCommand(_SqlQuery).ExecuteList<SelectListItem>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return ListFarmStays;
        }
    }
}
