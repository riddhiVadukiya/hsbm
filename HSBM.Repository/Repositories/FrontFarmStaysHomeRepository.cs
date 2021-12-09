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
    public class FrontFarmStaysHomeRepository
    {
        public List<FarmStaysHomeResponse> GetAllBannerMasterBySearchRequest()
        {
            List<FarmStaysHomeResponse> objList = new List<FarmStaysHomeResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Title],[ImageName],[Alt],[OrderIndex],[IsActive],RecordsTotal = COUNT(*) OVER() from [dbo].[BannerMaster]";
                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStaysHomeResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }
        public List<FarmStaysPopularFarmstayResponse> GetAllPopularFarmStay()
        {
            List<FarmStaysPopularFarmstayResponse> objList = new List<FarmStaysPopularFarmstayResponse>();
            List<FarmStaysDetailForHome> _ListFarmStaysDetailForHome = new List<FarmStaysDetailForHome>();
            FarmStaysPopularFarmstayRequest _FarmStaysPopularFarmstayRequest = new FarmStaysPopularFarmstayRequest();
            FarmStaysPopularFarmstayResponse _FarmStaysPopularFarmstayResponse = new FarmStaysPopularFarmstayResponse();

            _FarmStaysPopularFarmstayRequest.CheckInDate = DateTime.Now.AddDays(1).Date;
            _FarmStaysPopularFarmstayRequest.CheckOutDate = DateTime.Now.AddDays(2).Date;
            _FarmStaysPopularFarmstayRequest.NoOfGuest = 2;
            _FarmStaysPopularFarmstayRequest.IsSolo = 0;
            string CurrencyCode = Helper.GetCurrentCurrency();

            try
            {


                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"select *
                                            ,(select Top 1 ImageName from FarmStaysImages FSI where FSI.FarmStaysId=t.Id ) as ImageName 
                                            from (
                                            select top 6 dbo.CurrecncyConversion('" + CurrencyCode + @"',min(Price * fsr.MaxPerson)) as Price, FS.Id,FS.Name as FarmStayName from FarmStaysSeasons fss 
                                            inner join FarmStaysRooms fsr on fss.roomid = fsr.Id
                                            inner join FarmStays fs on fs.id = fsr.FarmStaysId
                                            where bookingdate > cast(getdate() as date) and FS.IsActive = 1 
                                            group by  FS.Id,FS.Name 
                                            order by newid()
                                            ) as t";

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStaysPopularFarmstayResponse>();
                    
                    objList.ForEach(t =>
                    {
                        t.NoOfGuest = _FarmStaysPopularFarmstayRequest.NoOfGuest;
                        t.IsSolo = _FarmStaysPopularFarmstayRequest.IsSolo;
                        t.CheckInDate = _FarmStaysPopularFarmstayRequest.CheckInDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                        t.CheckOutDate = _FarmStaysPopularFarmstayRequest.CheckOutDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);

                    });
                    return objList;


                    //string _SqlQuery = @"Select FS.Id,FS.Name,(select Top 1 ImageName from FarmStaysImages FSI where FSI.FarmStaysId=FS.Id )as ImageName From FarmStays FS where FS.IsActive = 1";
                    //_ListFarmStaysDetailForHome = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStaysDetailForHome>();
                    //if (_ListFarmStaysDetailForHome != null)
                    //{
                    //    int count = 1;
                    //    for (int i = 0; i < count; i++)
                    //    {
                    //        foreach (var item in _ListFarmStaysDetailForHome)
                    //        {
                    //            if (objList.Where(x => x.Id == item.Id).Count() == 0)
                    //            {
                    //                _SqlQuery = @"select top 1 RoomId,Name,Type,dbo.CurrecncyConversion('" + CurrencyCode + "',Price) as Price,dbo.CurrecncyConversion('" + CurrencyCode + "',DiscountPrice) as DiscountPrice,AvailableFor from GetAvailabilityAndPrice(" + item.Id + ",'" + _FarmStaysPopularFarmstayRequest.CheckInDate.ToString("dd/MMM/yyyy") + "','" + _FarmStaysPopularFarmstayRequest.CheckOutDate.ToString("dd/MMM/yyyy") + "'," + _FarmStaysPopularFarmstayRequest.NoOfGuest + "," + _FarmStaysPopularFarmstayRequest.IsSolo + ") order by Price";
                    //                _FarmStaysPopularFarmstayResponse = _DbManager.SetCommand(_SqlQuery).ExecuteObject<FarmStaysPopularFarmstayResponse>();
                    //                if (_FarmStaysPopularFarmstayResponse != null)
                    //                {
                    //                    _FarmStaysPopularFarmstayResponse.ImageName = item.ImageName;
                    //                    _FarmStaysPopularFarmstayResponse.FarmStayName = item.Name;
                    //                    _FarmStaysPopularFarmstayResponse.NoOfGuest = _FarmStaysPopularFarmstayRequest.NoOfGuest;
                    //                    _FarmStaysPopularFarmstayResponse.IsSolo = _FarmStaysPopularFarmstayRequest.IsSolo;
                    //                    _FarmStaysPopularFarmstayResponse.CheckInDate = _FarmStaysPopularFarmstayRequest.CheckInDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    //                    _FarmStaysPopularFarmstayResponse.CheckOutDate = _FarmStaysPopularFarmstayRequest.CheckOutDate.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                    //                    _FarmStaysPopularFarmstayResponse.Id = item.Id;
                    //                    objList.Add(_FarmStaysPopularFarmstayResponse);
                    //                    if (objList.Count() >= 6)
                    //                    {
                    //                        break;
                    //                    }
                    //                }
                    //            }

                    //        }
                    //        if (objList.Count() < 6)
                    //        {
                    //          _FarmStaysPopularFarmstayRequest.CheckInDate=  _FarmStaysPopularFarmstayRequest.CheckInDate.AddDays(1);
                    //           _FarmStaysPopularFarmstayRequest.CheckOutDate= _FarmStaysPopularFarmstayRequest.CheckOutDate.AddDays(1);
                    //            count++;
                    //        }
                    //    }
                    //    return objList.OrderBy(x=>x.Price).ToList();
                    //}
                    //return objList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        public List<FarmStaysDiscountResponse> GetAllDiscountOnFarmstays()
        {
            List<FarmStaysDiscountResponse> objList = new List<FarmStaysDiscountResponse>();
            FarmStaysDiscountResponse _FarmStaysDiscountResponse = new FarmStaysDiscountResponse();
            FarmStaysDiscountRequest _FarmStaysDiscountRequest = new FarmStaysDiscountRequest();
            List<FarmStaysDetailForHome> _ListFarmStaysDetailForHome = new List<FarmStaysDetailForHome>();
            try
            {
                _FarmStaysDiscountRequest.CheckInDate = DateTime.Now.AddDays(1);//.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                _FarmStaysDiscountRequest.CheckOutDate = DateTime.Now.AddDays(2);//.ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                _FarmStaysDiscountRequest.NoOfGuest = 2;
                _FarmStaysDiscountRequest.Flag = 0;
                string CurrencyCode = Helper.GetCurrentCurrency();
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"Select FS.Id,FS.Name,(select Top 1 ImageName from FarmStaysImages FSI where FSI.FarmStaysId=FS.Id )as ImageName From FarmStays FS where FS.IsActive = 1";
                    _ListFarmStaysDetailForHome = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStaysDetailForHome>();
                    if (_ListFarmStaysDetailForHome != null)
                    {
                        foreach (var item in _ListFarmStaysDetailForHome)
                        {
                            //                            _SqlQuery = @"declare @price decimal
                            //set @price = (select Top 1 Price from GetAvailabilityAndPrice(" + item.Id + ",'" + _FarmStaysDiscountRequest.CheckInDate.ToString("dd/MMM/yyyy") + "','" + _FarmStaysDiscountRequest.CheckOutDate.ToString("dd/MMM/yyyy") + "'," + _FarmStaysDiscountRequest.NoOfGuest + "," + _FarmStaysDiscountRequest.IsSolo + @"))
                            //declare @totalDiscount decimal
                            //set @totalDiscount = (select SUM(DiscountAmount) as FinalDiscountAmount from GetAppliedDiscountOnFarmstays(" + item.Id + ",'" + _FarmStaysDiscountRequest.CheckInDate.ToString("dd/MMM/yyyy") + "','" + _FarmStaysDiscountRequest.CheckOutDate.ToString("dd/MMM/yyyy") + "',@price," + _FarmStaysDiscountRequest.Flag + @"))
                            //declare @discountedPrice decimal
                            //set @discountedPrice = (@price - @totalDiscount)
                            //
                            //select @price as OrigionalPrice,@totalDiscount as TotalDiscount,@discountedPrice as DiscountedPrice
                            //where (@price <> 0 and @price is not null) and (@totalDiscount <> 0 and @totalDiscount is not null) and (@discountedPrice <> 0 and @discountedPrice is not null)
                            //
                            //";


                            _SqlQuery = "select top 1 dbo.CurrecncyConversion('" + CurrencyCode + "',Price) as OrigionalPrice,dbo.CurrecncyConversion('" + CurrencyCode + "',(Price-DiscountPrice)) as TotalDiscount,dbo.CurrecncyConversion('" + CurrencyCode + "',DiscountPrice) as DiscountedPrice from GetAvailabilityAndPrice(" + item.Id + ",'" + _FarmStaysDiscountRequest.CheckInDate.ToString("dd/MMM/yyyy") + "','" + _FarmStaysDiscountRequest.CheckOutDate.ToString("dd/MMM/yyyy") + "'," + _FarmStaysDiscountRequest.NoOfGuest + ",1) where DiscountPrice > 0 order by Price";

                            //_SqlQuery = @"select * from GetAppliedDiscountOnFarmstays(" + item.Id + ",'" + _FarmStaysDiscountRequest.CheckInDate.ToString("dd/MMM/yyyy") + "','" + _FarmStaysDiscountRequest.CheckOutDate.ToString("dd/MMM/yyyy") + "'," + _FarmStaysDiscountRequest.Price + "," + _FarmStaysDiscountRequest.Flag + ")";
                            _FarmStaysDiscountResponse = _DbManager.SetCommand(_SqlQuery).ExecuteObject<FarmStaysDiscountResponse>();
                            if (_FarmStaysDiscountResponse != null)
                            {
                                _FarmStaysDiscountResponse.ImageName = item.ImageName;
                                _FarmStaysDiscountResponse.FarmStayName = item.Name;
                                _FarmStaysDiscountResponse.NoOfGuest = _FarmStaysDiscountRequest.NoOfGuest;
                                _FarmStaysDiscountResponse.IsSolo = _FarmStaysDiscountRequest.IsSolo;
                                _FarmStaysDiscountResponse.CheckInDate = DateTime.Now.AddDays(1).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                                _FarmStaysDiscountResponse.CheckOutDate = DateTime.Now.AddDays(2).ToString(Helper.DefaultDateFormat(), CultureInfo.InvariantCulture);
                                _FarmStaysDiscountResponse.Id = item.Id;
                                objList.Add(_FarmStaysDiscountResponse);
                            }
                        }
                        return objList.Take(8).ToList(); ;
                    }
                    return objList;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
