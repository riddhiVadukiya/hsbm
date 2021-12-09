using BLToolkit.Data;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.HomeMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Repository.Repositories
{
    public class HomeRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<NewCustomersResponse> GetNewCustomersList()
        {
            List<NewCustomersResponse> result = new List<NewCustomersResponse>();

            try
            {
//                string query = @"
//        SELECT concat((Convert(char(3), CreatedDate, 0) + ' '),DATEPART(YEAR, CreatedDate)) monthyear,
//	           Convert(char(3), CreatedDate, 0) AS Month,
//	           DATEPART(YEAR, CreatedDate) AS Year,
//	           count(Id) TotalCount	   
//        FROM [dbo].[SystemUsers]
//        where UserType = "+ (int)UserTypes.Customer+ @"
//        GROUP BY DATEPART(YEAR, CreatedDate),convert(char(3), CreatedDate, 0)
//        order by DATEPART(YEAR, CreatedDate),convert(char(3), CreatedDate, 0)";

                string query = @"
        DECLARE  @start DATE = DATEADD(Month,-11,getdate()),
                                    @end DATE = DATEADD(day,1,getdate())  
  
                                    ;WITH cte AS   
                                    (  
                                        SELECT dt = DATEADD(DAY, -(DAY(@start) - 1), @start)  
                                        UNION ALL  
                                        SELECT DATEADD(Month, 1, dt)  
                                        FROM cte  
                                        WHERE dt < DATEADD(DAY, -(DAY(@end) - 1), @end)  
                                    ) 
									Select xx.monthyear,sum(xx.Year) as Year,sum(xx.Month) as Month,sum(xx.TotalCount) as TotalCount from (
									
									SELECT CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as monthyear,									
									DATEPART(YEAR, dt) as Year,DATEPART(MONTH, dt) as Month,TotalCount=0 FROM cte  c 
									union
									SELECT concat((Convert(char(3), CreatedDate, 0) + ' '),DATEPART(YEAR, CreatedDate)) as monthyear,	           
							    		   DATEPART(YEAR, CreatedDate) AS Year,										   
										   DATEPART(MONTH, CreatedDate) as Month,
										   count(Id) TotalCount	   
									FROM [dbo].[SystemUsers]
                                    where UserType = " + (int)UserTypes.Customer + @" and IsVerify = 1
                                    and CreatedDate > = @start and CreatedDate < = @end
				                    GROUP BY DATEPART(YEAR, CreatedDate),DATEPART(MONTH, CreatedDate),concat((Convert(char(3), CreatedDate, 0) + ' '),DATEPART(YEAR, CreatedDate))									
				                    ) as xx
				                    group by xx.monthyear,xx.Year,xx.Month
				                    order by xx.Year,xx.Month,xx.monthyear";

                using (DbManager _DbManager = new DbManager())
                {
                    result = _DbManager.SetCommand(query).ExecuteList<NewCustomersResponse>();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> GetNewCustomersList", ex);
                return null;
            }
        }

        public List<RecentOrder> RecentOrders()
        {
            List<RecentOrder> result = new List<RecentOrder>();

            try
            {
                string _SqlQuery = @"select Top 10 Id,OrderNo,OrderDate,(GuestFirstName + ' ' + GuestLastName) as Customer,GuestMobile, FarmStaysName,Amount,Status from [dbo].[OrderMaster]  ORDER BY OrderDate desc";                
                
                using (DbManager _DbManager = new DbManager())
                {
                    result = _DbManager.SetCommand(_SqlQuery).ExecuteList<RecentOrder>();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> RecentOrder", ex);
                return null;
            }
        }
        public List<HighestBookedFarmStay> HighestBooked()
        {
            List<HighestBookedFarmStay> result = new List<HighestBookedFarmStay>();

            try
            {
                string query = @"select top 5 FarmStaysName as label,COUNT(Farmstaysid) as value from OrderMaster om
				inner join FarmStays fs on om.Farmstaysid = fs.id
	            where status = " + (int)BookingStatus.CONFIRMED + @"
	            group by FarmStaysName,Farmstaysid
				order by COUNT(Farmstaysid) desc";

                using (DbManager _DbManager = new DbManager())
                {
                    result = _DbManager.SetCommand(query).ExecuteList<HighestBookedFarmStay>();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> HighestBooked", ex);
                return null;
            }
        }

        public List<FarmStaySummary> FarmStaySummary(int FarmstayId)
        {
            List<FarmStaySummary> result = new List<FarmStaySummary>();

            try
            {
                bool IsWhereClauseEmpty = false;
//                string _SqlQuery = @"DECLARE  @start DATE = DATEADD(YEAR,-1,getdate()),
//                                    @end DATE = getdate()  
//  
//                                    ;WITH cte AS   
//                                    (  
//                                        SELECT dt = DATEADD(DAY, -(DAY(@start) - 1), @start)  
//                                        UNION ALL  
//                                        SELECT DATEADD(Month, 1, dt)  
//                                        FROM cte  
//                                        WHERE dt < DATEADD(DAY, -(DAY(@end) - 1), @end)  
//                                    ) 
//                                    Select xx.MonthYear,sum(xx.Year) as Year,sum(xx.Month) as Month,sum(xx.Pending) as Pending,sum(xx.Confirm) as Confirm,sum(xx.Complete) as Complete,sum(xx.Cancel) as Cancel from (
//									
//                                    SELECT CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as MonthYear,DATEPART(YEAR, dt) AS Year,DATEPART(Month, dt) AS Month,Pending=0,Confirm=0,Complete=0,Cancel=0 FROM cte  c 
//                                    union
//                                    select CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120) as MonthYear,
//	                                DATEPART(YEAR, OrderDate) AS Year,
//	                                DATEPART(Month, OrderDate) AS Month,
//	                                count(case when Status = 1 then 1 end) as Pending,
//	                                count(case when Status = 2 then 1 end) as Confirm,
//	                                count(case when Status = 3 then 1 end) as Complete,
//	                                count(case when Status = 4 then 1 end) as Cancel
//	                                from [dbo].[OrderMaster]";

//                if (FarmstayId > 0)
//                {
//                    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
//                    IsWhereClauseEmpty = false;

//                    _SqlQuery += @" Farmstaysid =" + FarmstayId;
//                }

//                _SqlQuery += @" GROUP BY DATEPART(YEAR, OrderDate),
//	                                DATEPART(Month, OrderDate),
//	                                CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120)	                                
//									) as xx
//									group by xx.MonthYear,xx.Year,xx.Month
//				                    order by xx.Year,xx.Month,xx.MonthYear";

                string _SqlQuery = @"DECLARE  @StartDate DATE = DATEADD(Month,-11,getdate()),
		 @EndDate DATE = DATEADD(day,1,getdate()) 
  
         ;WITH cte AS   
         (  										
             SELECT dt = DATEADD(DAY, -(DAY(@StartDate) - 1), @StartDate)  
             UNION ALL  
             SELECT DATEADD(Month, 1, dt)  
             FROM cte  
             WHERE dt < DATEADD(DAY, -(DAY(@EndDate) - 1), @EndDate)  
         ) 

         Select xx.MonthYear,sum(xx.Year) as Year,sum(xx.Month) as Month,sum(xx.TotalEarning) as TotalEarning,sum(xx.TotalComplete) as TotalComplete,sum(xx.TotalCancel) as TotalCancel from 
		 (
		
					 SELECT 
					CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as MonthYear,
					DATEPART(YEAR, dt) AS Year,DATEPART(Month, dt) AS Month,
					 TotalEarning=0,
					TotalComplete=0,
					TotalCancel=0 
					FROM cte  c 
			union

				select MonthYear,Year,Month,sum(profit) as TotalEarning,Sum(Complated) as TotalComplete,sum(Canceled) as TotalCancel from 
				(
					select CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120) as MonthYear,DATEPART(YEAR, OrderDate) as Year,DATEPART(MONTH, OrderDate) as Month,Farmstaysid,Amount,MarkupPercentage,
					case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit,
					case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else 0 end as Complated,
					case when RefundAmount is null then 0 else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as Canceled
					from OrderMaster where ([Status] = 3 or [Status] = 4) AND  OrderDate  >= @StartDate AND  OrderDate  <= @EndDate";	
		                                   
                                        if (FarmstayId > 0)
                                            {
                                                if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                                                IsWhereClauseEmpty = false;
                                              _SqlQuery += @" Farmstaysid =" + FarmstayId;
                                            }

                                        _SqlQuery += @") as t group by Year,Month,MonthYear 
	                                                ) as xx 
	                                                Group by xx.Year,xx.Month,xx.MonthYear";


                using (DbManager _DbManager = new DbManager())
                {
                    result = _DbManager.SetCommand(_SqlQuery).ExecuteList<FarmStaySummary>();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> FarmStaySummary", ex);
                return null;
            }
        }

        public List<TotalBooking> BindTotalBooking(int Frequency, DateTime? FromDate, DateTime? ToDate)
        {
            List<TotalBooking> result = new List<TotalBooking>();
            string _SqlQuery = string.Empty;
            if (Frequency == (int)GraphFilter.Day)
            {

                _SqlQuery = @";with   Hours as
                                    (
                                    select  0 as [Hour],CONVERT(varchar(15),CAST('00:00:00' AS TIME),100) as Time
                             union all
                             select  [Hour] + 1, CONVERT(varchar(15),CAST(cast(([Hour] + 1) as nvarchar(3)) + ':00:00' AS TIME),100) as x
                             from  Hours
                             where   [Hour] < 23
                                    )
							Select xx.Hour,xx.Time,sum(xx.Cancelled) as Cancelled,sum(xx.Booked) as Booked,1 as IsDay from (
                            select  [Hour] as [Hour],Time,Cancelled=0,Booked=0,IsDay=1
                            from    Hours
                            union
                            SELECT CONVERT(varchar(2),OrderDate, 108) as [Hour],
                            CONVERT(varchar(15),CAST(DATEADD(Hour, DATEDIFF(Hour, 0, OrderDate), 0) AS TIME),100)  as Time,                            
                            count(case when [Status] = " + (int)BookingStatus.Cancel + @" then 1 end) as Cancelled,
                            count(case when [Status] = " + (int)BookingStatus.CONFIRMED + @" then 1 end) as Booked,
                            IsDay=1
                            FROM OrderMaster
                            where OrderDate >= CAST(Getdate() AS date) and OrderDate <= DATEADD(day,1,CAST(Getdate() AS date))
                            GROUP BY CONVERT(varchar(2),OrderDate, 108),
                            CONVERT(varchar(15),CAST(DATEADD(Hour, DATEDIFF(Hour, 0, OrderDate), 0) AS TIME),100),
                            DATEPART(hour,OrderDate)                            
							) as xx
								group by xx.Time,xx.Hour
								order by xx.Hour";
            }
            else if (Frequency == (int)GraphFilter.Week)
            {
                _SqlQuery = @"DECLARE @fromDate DATETIME
                            set  @fromDate = Dateadd(WEEK,-1,GETDATE() +1) 
                            DECLARE @toDate DATETIME =Dateadd(week,1,@fromDate)  

                            ;WITH mycte
                                    AS (SELECT @fromDate AS dt
                                        UNION ALL
                                        SELECT dt + 1
                                        FROM   mycte
                                        WHERE  dt + 1 < @toDate) 
				select xx.date,xx.Week,sum(xx.Cancelled) as Cancelled,sum(xx.Booked) as Booked,1 as IsWeek from (
                SELECT Convert(date,dt) as date,DATENAME(weekday,dt) as [Week],Cancelled = 0,Booked=0,IsWeek=1 FROM mycte
                union 
	            SELECT CONVERT(date,OrderDate) as Date ,DATENAME(weekday,OrderDate) as [Week],
                                    count(case when [Status] = 4 then 1 end) as Cancelled,
                                    count(case when [Status] = 3 then 1 end) as Booked,IsWeek=1                
                from OrderMaster  
                where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + @") and OrderDate >= DATEADD(WEEK, -1, GetDate()) 
                group by CONVERT(date,OrderDate),DATENAME(weekday,OrderDate)
				) as xx
				group by xx.date,xx.Week
				order by xx.date";
            }
            else if (Frequency == (int)GraphFilter.Month)
            {
                _SqlQuery = @"DECLARE @fromDate DATETIME
                                    set  @fromDate = Dateadd(day,-30,CONVERT(date, getdate()+1))
                                    DECLARE @toDate DATETIME = CONVERT(date, getdate())

                                    ;WITH mycte
                                         AS (SELECT @fromDate AS dt
                                             UNION ALL
                                             SELECT dt + 1
                                             FROM   mycte
                                             WHERE  dt + 1 <= @toDate) 
                                    select xx.date,xx.Month,sum(xx.Cancelled) as Cancelled,sum(xx.Booked) as Booked,1 as IsMonth from (
                                    SELECT CONVERT(date,Dt) as Date,FORMAT(Dt,'D','en-gb') as Month,Cancelled = 0,Booked=0,IsMonth=1 FROM mycte
                                    union 
                                    SELECT CONVERT(date,OrderDate) as Date ,FORMAT(OrderDate,'D','en-gb') as Month,
                                    count(case when [Status] = " + (int)BookingStatus.Cancel + @" then 1 end) as Cancelled,
                                    count(case when [Status] = " + (int)BookingStatus.CONFIRMED + @" then 1 end) as Booked,
                                    IsMonth=1
                                     FROM [dbo].[OrderMaster] WHERE OrderDate >= DATEADD(month, -1, GetDate())
                                    group by CONVERT(date,OrderDate),FORMAT(OrderDate,'D','en-gb')
                                    ) as xx
									group by xx.date,xx.Month
									order by xx.date";
            }
            else if (Frequency == (int)GraphFilter.Year)
            {
                _SqlQuery = @"DECLARE  @start DATE = DATEADD(Month,-11,getdate()),
                                    @end DATE = DATEADD(day,1,getdate())   
  
                                    ;WITH cte AS   
                                    (  
                                        SELECT dt = DATEADD(DAY, -(DAY(@start) - 1), @start)  
                                        UNION ALL  
                                        SELECT DATEADD(Month, 1, dt)  
                                        FROM cte  
                                        WHERE dt < DATEADD(DAY, -(DAY(@end) - 1), @end)  
                                    )  									
									Select xx.Year,sum(xx.Yearname) as Yearname,sum(xx.Monthname) as Monthname,sum(xx.Cancelled) as Cancelled,sum(xx.Booked) as Booked,1 as IsYear from (
                                    SELECT CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as Year,DATEPART(YEAR, dt) as Yearname,DATEPART(MONTH, dt) as Monthname,							
									Cancelled=0,Booked=0, IsYear=1 FROM cte  c
                                    union

									select CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120) as Year,
												 DATEPART(YEAR, OrderDate) as Yearname,DATEPART(MONTH, OrderDate) as Monthname,	
	                                    count(case when [Status] = " + (int)BookingStatus.Cancel + @" then 1 end) as Cancelled,
                                        count(case when [Status] = " + (int)BookingStatus.CONFIRMED + @" then 1 end) as Booked
	                                    ,IsYear=1
	                                    from [dbo].[OrderMaster] where OrderDate >= DATEADD(Month,-11, CONVERT(date, getdate()))
	                                    GROUP BY DATEPART(YEAR, OrderDate),DATEPART(MONTH, OrderDate),CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120)                                   
										) as xx
										group by xx.Yearname,xx.Monthname,xx.Year";
            }
            else if (Frequency == (int)GraphFilter.Custom)
            {
//                _SqlQuery = @"DECLARE  @start DATE = DATEADD(YEAR,-1,getdate()),
//                                    @end DATE = getdate()  
//  
//                                    ;WITH cte AS   
//                                    (  
//                                        SELECT dt = DATEADD(DAY, -(DAY(@start) - 1), @start)  
//                                        UNION ALL  
//                                        SELECT DATEADD(Month, 1, dt)  
//                                        FROM cte  
//                                        WHERE dt < DATEADD(DAY, -(DAY(@end) - 1), @end)  
//                                    )  									
//									Select xx.Year,sum(xx.Yearname) as Yearname,sum(xx.Monthname) as Monthname,sum(xx.Cancelled) as Cancelled,sum(xx.Booked) as Booked,1 as IsYear from (
//                                    SELECT CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as Year,DATEPART(YEAR, dt) as Yearname,DATEPART(MONTH, dt) as Monthname,							
//									Cancelled=0,Booked=0, IsYear=1 FROM cte  c
//                                    union
//
//									select CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120) as Year,
//												 DATEPART(YEAR, OrderDate) as Yearname,DATEPART(MONTH, OrderDate) as Monthname,	
//	                                    count(case when [Status] = " + (int)BookingStatus.Cancel + @" then 1 end) as Cancelled,
//                                        count(case when [Status] = " + (int)BookingStatus.Complete + @" then 1 end) as Booked
//	                                    ,IsYear=1
//	                                    from [dbo].[OrderMaster]
//                                where OrderDate >= '" + (FromDate.HasValue ? FromDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + "' and OrderDate <= '" + (ToDate.HasValue ? ToDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + @"' 	                            
//	                            GROUP BY DATEPART(YEAR, OrderDate),DATEPART(MONTH, OrderDate),CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120)	                                    
//										) as xx
//										group by xx.Yearname,xx.Monthname,xx.Year";

                _SqlQuery = @"SELECT CONVERT(date,OrderDate) as Date ,FORMAT(OrderDate,'D','en-gb') as Month,	
	                                    count(case when [Status] = " + (int)BookingStatus.Cancel + @" then 1 end) as Cancelled,
                                        count(case when [Status] = " + (int)BookingStatus.CONFIRMED + @" then 1 end) as Booked
	                                    ,IsCustom=1
	                                    from [dbo].[OrderMaster]
                                where OrderDate >= '" + (FromDate.HasValue ? FromDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + "' and OrderDate <= '" + (ToDate.HasValue ? ToDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + @"' 	                            
	                            GROUP BY CONVERT(date,OrderDate),FORMAT(OrderDate,'D','en-gb')";
            }
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    result = _DbManager.SetCommand(_SqlQuery).ExecuteList<TotalBooking>();
                    return result;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindTotalBooking", ex);
                return null;
            }
        }


        public List<TotalEarnings> BindTotalEarning(int Frequency, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = string.Empty;
                    if (Frequency == (int)GraphFilter.Day)
                    {
                        _SqlQuery = @";with   Hours as
                            (
                             select  0 as [Hour],CONVERT(varchar(15),CAST('00:00:00' AS TIME),100) as Time
                             union all
                             select  [Hour] + 1, CONVERT(varchar(15),CAST(cast(([Hour] + 1) as nvarchar(3)) + ':00:00' AS TIME),100) as x
                             from  Hours
                             where   [Hour] < 23
                             )
                             Select xx.Hour,xx.Time,sum(xx.TotalBooking) as TotalBooking,sum(xx.TotalEarning) as TotalEarning,1 as IsDay from (
                             select  [Hour] as [Hour],Time,TotalBooking=0,TotalEarning=0, IsDay=1 from Hours
                             union
                             select tt.[Hour],tt.Time,tt.TotalBooking,tt.TotalEarning, IsDay=1 from (
                                select CONVERT(varchar(2),OrderDate, 108) as [Hour],
								CONVERT(varchar(15),CAST(DATEADD(Hour, DATEDIFF(Hour, 0, OrderDate), 0) AS TIME),100)  as Time,
								BookingYear,BookingMonth,
                                Count(Farmstaysid) as TotalBooking,sum(profit) as TotalEarning from (
                                select OrderDate,DATEPART(YEAR, OrderDate) as BookingYear,DATEPART(MONTH, OrderDate) as BookingMonth,Farmstaysid,Amount,MarkupPercentage,
                                case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit
                                from OrderMaster  
                                where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + @")
                                and CAST(OrderDate AS date) >= CAST(Getdate() AS date) and CAST(OrderDate AS date) <= DATEADD(day,1,CAST(Getdate() AS date)) ) as t 
                               group by CONVERT(varchar(2),OrderDate, 108),CONVERT(varchar(15),CAST(DATEADD(Hour, DATEDIFF(Hour, 0, OrderDate), 0) AS TIME),100),BookingYear,BookingMonth								
								) tt							
								) as xx
								group by xx.Time,xx.Hour
								order by xx.Hour";

                    }
                    else if (Frequency == (int)GraphFilter.Week)
                    {
                        _SqlQuery = @" DECLARE @StartDate DATETIME
                                        set  @StartDate = Dateadd(WEEK,-1,GETDATE()+1) 
                                        DECLARE @EndDate DATETIME =Dateadd(week,1,@StartDate)  
				                        ;WITH mycte AS
				                        (SELECT @StartDate AS dt
						                        UNION ALL
						                        SELECT dt + 1
						                        FROM   mycte
						                        WHERE  dt + 1 < @EndDate
				                        ) 

                                        Select xx.date,xx.Week,sum(xx.TotalEarning) as TotalEarning,1 as IsWeek from 
		                                    (
			                                SELECT 				                                    
				                                Convert(date,cast(dt as DATE)) as date,
						                        DATENAME(weekday,dt) as [Week],						
						                        TotalEarning=0,
						                        IsWeek=1					                                
				                                FROM mycte  c 
			                                union					
			                                select t.date,t.Week,sum(profit) as TotalEarning, IsWeek=1			
				                                from 
				                                (
					                                select 
							                        OrderDate,
							                        Convert(date,cast(OrderDate as DATE)) as date,
							                        DATENAME(weekday,OrderDate) as [Week],					                                    										                                    
					                                Farmstaysid,Amount,MarkupPercentage,					
					                                case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit
                                                    from OrderMaster 
                                                        where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + @") and OrderDate >= DATEADD(WEEK, -1, CONVERT(date, GETDATE())) 
                                                        ) as t group by date,[Week]
	                                            ) as xx 
	                                            group by xx.date,xx.Week
					                            order by xx.date";
                    }
                    else if (Frequency == (int)GraphFilter.Month)
                    {
                        _SqlQuery = @"  DECLARE @StartDate DATETIME
                                        set  @StartDate = Dateadd(day,-30,CONVERT(date, getdate()+1))
                                        DECLARE @EndDate DATETIME = CONVERT(date, getdate())
                                                ;WITH mycte AS
		                                        (SELECT @StartDate AS dt
                                                                UNION ALL
                                                                SELECT dt + 1
                                                                FROM   mycte
                                                                WHERE  dt + 1 <= @EndDate
		                                        ) 

                                            Select xx.MonthYear,date,sum(xx.Year) as Year,Month as Month,sum(xx.TotalEarning) as TotalEarning, IsMonth=1 from 
		                                        (
			                                    SELECT 
				                                    CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as MonthYear,
				                                    CONVERT(date,dt) as date,
				                                    DATEPART(YEAR, dt) AS Year,
				                                    FORMAT(dt,'D','en-gb') as Month,
					                                    TotalEarning=0, IsMonth=1				
				                                    FROM mycte  c 
			                                    union					
			                                    select MonthYear,date,Year,Month,sum(profit) as TotalEarning, IsMonth=1				
				                                    from 
				                                    (
					                                    select 
					                                    CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120) as MonthYear,
					                                    CONVERT(date,OrderDate) as date,
					                                    DATEPART(YEAR, OrderDate) as Year,					
					                                    FORMAT(OrderDate,'D','en-gb') as Month,
					                                    Farmstaysid,Amount,MarkupPercentage,					
					                                    case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit
                                                        from OrderMaster
                                    where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + @")AND  OrderDate >= DATEADD(month, -1, GetDate())
                                    ) as t group by date,Year,Month,MonthYear 
	                                ) as xx 
	                                Group by xx.date,xx.Year,xx.Month,xx.MonthYear";
                    }
                    else if (Frequency == (int)GraphFilter.Year)
                    {
                        _SqlQuery = @"DECLARE  @start DATE = DATEADD(Month,-11,getdate()),
                                    @end DATE = DATEADD(day,1,getdate())  
  
                                    ;WITH cte AS   
                                    (  
                                        SELECT dt = DATEADD(DAY, -(DAY(@start) - 1), @start)  
                                        UNION ALL  
                                        SELECT DATEADD(Month, 1, dt)  
                                        FROM cte  
                                        WHERE dt < DATEADD(DAY, -(DAY(@end) - 1), @end)  
                                    )  									
									Select xx.Year,sum(xx.BookingYear) as BookingYear,sum(xx.BookingMonth) as BookingMonth,sum(xx.TotalBooking) as TotalBooking,sum(xx.TotalEarning) as TotalEarning,1 as IsYear from (
                                    SELECT CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as Year,DATEPART(YEAR, dt) as BookingYear,DATEPART(Month, dt) as BookingMonth,									
									TotalBooking=0,TotalEarning=0, IsYear=1
									                                    FROM cte  c
                                    union

                                    select tt.*, IsYear=1 from (
									select Year,
									BookingYear,BookingMonth,
									Count(Farmstaysid) as TotalBooking,sum(profit) as TotalEarning from (
									select OrderDate,
									(CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120)) as Year, 
									DATEPART(YEAR, OrderDate) as BookingYear,DATEPART(Month, OrderDate) as BookingMonth,
									Farmstaysid,Amount,MarkupPercentage,
									case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit
									from OrderMaster where OrderDate >= DATEADD(Month,-11, CONVERT(date, getdate()))";

                        _SqlQuery += " and ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + ")";

                        _SqlQuery += @" )  as t group by BookingYear,BookingMonth,Year ) as tt 
								    ) as xx  
									group by xx.BookingYear,xx.BookingMonth,xx.Year ";

                    }
                    else if (Frequency == (int)GraphFilter.Custom)
                    {
                        //string fDate =  FromDate.ToString("dd/MMM/yyyy");
//                        _SqlQuery = @"DECLARE  @start DATE = DATEADD(YEAR,-1,getdate()),
//                                    @end DATE = getdate()  
//  
//                                    ;WITH cte AS   
//                                    (  
//                                        SELECT dt = DATEADD(DAY, -(DAY(@start) - 1), @start)  
//                                        UNION ALL  
//                                        SELECT DATEADD(Month, 1, dt)  
//                                        FROM cte  
//                                        WHERE dt < DATEADD(DAY, -(DAY(@end) - 1), @end)  
//                                    )  									
//									Select xx.Year,sum(xx.BookingYear) as BookingYear,sum(xx.BookingMonth) as BookingMonth,sum(xx.TotalBooking) as TotalBooking,sum(xx.TotalEarning) as TotalEarning,1 as IsYear from (
//                                    SELECT CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as Year,DATEPART(YEAR, dt) as BookingYear,DATEPART(Month, dt) as BookingMonth,									
//									TotalBooking=0,TotalEarning=0, IsYear=1
//									                                    FROM cte  c
//                                    union
//
//                                    select tt.*, IsYear=1 from (
//									select Year,
//									BookingYear,BookingMonth,
//									Count(Farmstaysid) as TotalBooking,sum(profit) as TotalEarning from (
//									select OrderDate,
//									(CONVERT(CHAR(4), OrderDate, 100) + CONVERT(CHAR(4), OrderDate, 120)) as Year, 
//									DATEPART(YEAR, OrderDate) as BookingYear,DATEPART(Month, OrderDate) as BookingMonth,
//									Farmstaysid,Amount,MarkupPercentage,
//									convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) as profit  
//									from OrderMaster";

//                        _SqlQuery += " where ([Status] = " + (int)BookingStatus.Complete + " or [Status] = " + (int)BookingStatus.Cancel + @") 
//                                    and OrderDate >= '" + (FromDate.HasValue ? FromDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + "' and OrderDate <= '" + (ToDate.HasValue ? ToDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + @"'";

//                        _SqlQuery += @" )  as t group by BookingYear,BookingMonth,Year ) as tt 
//								    ) as xx  
//									group by xx.BookingYear,xx.BookingMonth,xx.Year ";
//                        _SqlQuery = @"select tt.* from (
//                                    select ((datename(month,BookingMonth)) + ' ' + Convert(nvarchar(50),BookingYear)) as [Date],BookingYear,BookingMonth,
//                                    Count(Farmstaysid) as TotalBooking,ABS(sum(profit)) as TotalEarning from (
//                                    select OrderDate,DATEPART(YEAR, OrderDate) as BookingYear,DATEPART(MONTH, OrderDate) as BookingMonth,Farmstaysid,Amount,MarkupPercentage,
//                                    convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) as profit  
//                                    from OrderMaster 
//                                    where ([Status] = " + (int)BookingStatus.Complete + " or [Status] = " + (int)BookingStatus.Cancel + @") 
//                                    and OrderDate >= '" + (FromDate.HasValue ? FromDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + "' and OrderDate <= '" + (ToDate.HasValue ? ToDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + @"' ) as t 
//                                    group by BookingYear,BookingMonth ) tt";

                        _SqlQuery = @"select tt.*,IsCustom=1 from (
                                    select CONVERT(date,OrderDate) as Date,FORMAT(OrderDate,'D','en-gb') as Month,BookingYear,BookingMonth,
                                    Count(Farmstaysid) as TotalBooking,sum(profit) as TotalEarning from (
                                    select OrderDate,DATEPART(YEAR, OrderDate) as BookingYear,DATEPART(MONTH, OrderDate) as BookingMonth,Farmstaysid,Amount,MarkupPercentage,
                                    case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit
                                    from OrderMaster 
                                    where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + @") 
                                    and OrderDate >= '" + (FromDate.HasValue ? FromDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + "' and OrderDate <= '" + (ToDate.HasValue ? ToDate.Value.ToString("dd/MMM/yyyy") : String.Empty) + @"' ) as t 
                                    group by CONVERT(date,OrderDate),FORMAT(OrderDate,'D','en-gb'),BookingYear,BookingMonth ) tt";
                    }
                    return _DbManager.SetCommand(_SqlQuery).ExecuteList<TotalEarnings>();
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindTotalEarning", ex);
                return null;
            }
        }

        public GeneralCalculation GeneralCalculation()
        {
            GeneralCalculation _list = new GeneralCalculation();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = string.Empty;
                    _SqlQuery = @"select count(*) as ConfirmedBooking from ordermaster where [Status] = " + (int)BookingStatus.CONFIRMED;

                    _list.ConfirmedBooking = _DbManager.SetCommand(_SqlQuery).ExecuteScalar<int>();

                    _SqlQuery = @"select Count(*) as Totalcustomer from [dbo].[SystemUsers] where UserType = " + (int)UserTypes.Customer + " and IsVerify = 1 and IsActive =1";

                    _list.Totalcustomer = _DbManager.SetCommand(_SqlQuery).ExecuteScalar<int>();

                    _SqlQuery = @"select sum(Amount) as TotalTransaction from ordermaster where Status in (" + (int)BookingStatus.CONFIRMED + "," + (int)BookingStatus.Cancel + ")";

                    //_list.TotalTransaction = _DbManager.SetCommand(_SqlQuery).ExecuteScalar<decimal>().ToString("0.00");
                    _list.TotalTransaction = _DbManager.SetCommand(_SqlQuery).ExecuteScalar<int>();

                    _SqlQuery = @" select sum(tt.TotalEarning) as TotalEarning from (
	                                select BookingYear,BookingMonth,Farmstaysid,Count(Farmstaysid) as TotalBooking,sum(profit) as TotalEarning from (
	                                select OrderDate,DATEPART(YEAR, OrderDate) as BookingYear,DATEPART(MONTH, OrderDate) as BookingMonth,Farmstaysid,Amount,MarkupPercentage,
                                    case when RefundAmount is null then convert(decimal(18,2), (NetAmount- isnull(RefundAmount,0)) - ((Amount * 100)/(100+MarkupPercentage))) else convert(decimal(18,2), (NetAmount-RefundAmount) - (((Amount * 100)/(100+MarkupPercentage)) - (((Amount * 100)/(100+MarkupPercentage)) * RefundablePercentage / 100))) end as profit
                                    from OrderMaster";

                    _SqlQuery += " where ([Status] = " + (int)BookingStatus.CONFIRMED + " or [Status] = " + (int)BookingStatus.Cancel + ")";


                    _SqlQuery += @" ) as t group by Farmstaysid,BookingYear,BookingMonth ) tt  inner join FarmStays fs on tt.Farmstaysid = fs.id ";

                    //_list.TotalEarning = _DbManager.SetCommand(_SqlQuery).ExecuteScalar<decimal>().ToString("0.00");
                    _list.TotalEarning = _DbManager.SetCommand(_SqlQuery).ExecuteScalar<int>();
                    return _list;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> BindTotalEarning", ex);
                return null;
            }
        }

        public GridDataResponse ExportGetNewCustomersList()
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            List<NewCustomersResponse> result = new List<NewCustomersResponse>();

            try
            {
                string query = @" DECLARE  @start DATE = DATEADD(Month,-11,getdate()),
                                    @end DATE = DATEADD(day,1,getdate())  
  
                                    ;WITH cte AS   
                                    (  
                                        SELECT dt = DATEADD(DAY, -(DAY(@start) - 1), @start)  
                                        UNION ALL  
                                        SELECT DATEADD(Month, 1, dt)  
                                        FROM cte  
                                        WHERE dt < DATEADD(DAY, -(DAY(@end) - 1), @end)  
                                    ) 
									Select xx.monthyear,sum(xx.Year) as Year,sum(xx.Month) as Month,sum(xx.TotalCount) as TotalCount from (
									
									SELECT CONVERT(CHAR(4), dt, 100) + CONVERT(CHAR(4), dt, 120) as monthyear,									
									DATEPART(YEAR, dt) as Year,DATEPART(MONTH, dt) as Month,TotalCount=0 FROM cte  c 
									union
									SELECT concat((Convert(char(3), CreatedDate, 0) + ' '),DATEPART(YEAR, CreatedDate)) as monthyear,	           
							    		   DATEPART(YEAR, CreatedDate) AS Year,										   
										   DATEPART(MONTH, CreatedDate) as Month,
										   count(Id) TotalCount	   
									FROM [dbo].[SystemUsers]
                                    where UserType = " + (int)UserTypes.Customer + @" and IsVerify = 1
                                    and CreatedDate > = @start and CreatedDate < = @end
				                    GROUP BY DATEPART(YEAR, CreatedDate),DATEPART(MONTH, CreatedDate),concat((Convert(char(3), CreatedDate, 0) + ' '),DATEPART(YEAR, CreatedDate))									
				                    ) as xx
				                    group by xx.monthyear,xx.Year,xx.Month
				                    order by xx.Year,xx.Month,xx.monthyear";

                using (DbManager _DbManager = new DbManager())
                {
                    var objList = _DbManager.SetCommand(query).ExecuteList<NewCustomersResponse>();
                    if (objList.Any())
                    {
                        //_GridDataResponse.recordsTotal = objList.FirstOrDefault().RecordsTotal;
                        _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    }
                    _GridDataResponse.data = objList;
                    return _GridDataResponse;
                }

            }
            catch (Exception ex)
            {
                _ILogger.Error("Home=> GetNewCustomersList", ex);
                return null;
            }            
        }
    }
}
