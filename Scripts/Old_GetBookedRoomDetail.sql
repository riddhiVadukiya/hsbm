USE [HSBM]
GO
/****** Object:  UserDefinedFunction [dbo].[GetBookedRoomDetail]    Script Date: 18-Dec-18 12:13:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
--select * from GetBookedRoomDetail(1,4,'05/May/2018','06/May/2018',2,0)
-- =============================================
ALTER FUNCTION [dbo].[GetBookedRoomDetail]
(
    @FarmStaysId bigint,	
	@RoomId bigint,
	@CheckInDate DATE,
	@CheckOutDate DATE,
	@NoOfGuest INT,
	@IsSolo BIT,
	@CurrencyCode nvarchar(10)
)       
returns @MinPriceTable TABLE (FarmStayName nvarchar(100),Address nvarchar(max),CancellationPolicyIsNonRefundable bit,RefundablePercentage decimal(18,2),RefundableBeforDays int,MarkupPercentage decimal(18,2),CheckInTime Time,CheckOutTime Time, RoomId bigint,Name nvarchar(50),Type int,MaxPerson int, Price decimal(18,2), DiscountPrice decimal(18,2),IsMale bit,IsShared bit)       
AS
begin 
	Set @CheckOutDate= DateAdd(DAY,-1,@CheckOutDate)

DECLARE @TempTable TABLE (RoomId bigint,Name nvarchar(50),Type int,MaxPerson int, Price decimal(18,2), DiscountPrice decimal(18,2),IsMale bit,IsShared bit)

if 	@IsSolo = 0 
	begin 	
		insert into @TempTable
		select Id,Name,Type,MaxPerson, 		
		(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 	 as Price,
		 ((dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 
		- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson),0))
		)
		 as DiscountPrice
		,IsMale
		,0 as IsShared  from (select
		Id, 
		Name,
		Type,
		MaxPerson, 
		isnull((select sum(NumberOfPerson) from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId),0) as booked,
		(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) as Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price
		,null as IsMale 
		from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest
	end 
	else 
	begin
			insert into @TempTable		
			select Id,Name,Type,MaxPerson, 
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 	 as Price,
			(
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 
			- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest),0))
			)
			 as DiscountPrice
			 ,IsMale
			 ,1 as IsShared from 
			( select ID,Name,Type,MaxPerson, 
			isnull((select sum(Booked) from ( select case when (MaxPerson - sum(NumberOfPerson)) >= @NoOfGuest then 0 else 1 end as Booked  from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId  group by BookingDate,MaxPerson) as t),0) as booked,
			(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price,
			(select top 1 AvailableFor as AvailableFor from (  select  (case when om.IsMale = 1 then 1 else 0 end) as AvailableFor  from  InventoryMaster im inner join OrderMaster om on im.OrderId = om.Id  where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId =@RoomId  group by BookingDate,MaxPerson,om.IsMale) as t group by AvailableFor) as IsMale  

			from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest and IsMale is not null


		
		IF EXISTS (SELECT * FROM @TempTable WHERE IsMale is not null)
		BEGIN
			insert into @TempTable
			select Id,Name,Type,	MaxPerson, 	
			(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 	 as Price,
			((dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 
			- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson),0))
			)
			 as DiscountPrice
			,IsMale
			,0 as IsShared from (select
			Id, 
			Name,
			Type,
			MaxPerson, 
			isnull((select sum(NumberOfPerson) from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId),0) as booked,
			(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) as Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price
			,null as IsMale 
			from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest
			and Id not in (SELECT RoomId FROM @TempTable)

		END	
		ELSE
		BEGIN

			insert into @TempTable		
			select Id,Name,Type,MaxPerson, 
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 	 as Price,
			(
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 
			- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest),0))
			)
			 as DiscountPrice
			 ,IsMale
			 ,1 as IsShared  from 
			( select ID,Name,Type,MaxPerson, 
			isnull((select sum(Booked) from ( select case when (MaxPerson - sum(NumberOfPerson)) >= @NoOfGuest then 0 else 1 end as Booked  from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId  group by BookingDate,MaxPerson) as t),0) as booked,
			(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price,
			(select top 1 AvailableFor as AvailableFor from (  select  (case when om.IsMale = 1 then 1 else 0 end) as AvailableFor  from  InventoryMaster im inner join OrderMaster om on im.OrderId = om.Id  where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId =@RoomId  group by BookingDate,MaxPerson,om.IsMale) as t group by AvailableFor) as IsMale    
			from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest 


		END	

	end 


	insert into @MinPriceTable 
	SELECT 
	FS.Name as 'FarmStayName',
	FS.Address,
	FS.CancellationPolicyIsNonRefundable ,
	FS.RefundablePercentage,
	FS.RefundableBeforDays,
	FS.MarkupPercentage,
	FS.CheckInTime ,
	FS.CheckOutTime ,
	T.RoomId,
	T.Name,
	T.Type,
	T.MaxPerson, 
	dbo.CurrecncyConversion(@CurrencyCode,T.Price )as Price,
	dbo.CurrecncyConversion(@CurrencyCode,T.DiscountPrice)as DiscountPrice,
	T.IsMale,
	T.IsShared 
	from @TempTable T
	inner join Farmstays FS on FS.Id=@FarmStaysId

return       
end

