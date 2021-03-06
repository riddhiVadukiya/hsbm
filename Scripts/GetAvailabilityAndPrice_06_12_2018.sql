ALTER FUNCTION [dbo].[GetAvailabilityAndPrice]
(
    @FarmStaysId bigint,
	@CheckInDate DATE,
	@CheckOutDate DATE,
	@NoOfGuest INT,
	@IsSolo BIT
)       

returns @MinPriceTable TABLE (RoomId bigint,Name nvarchar(50),Type int, Price decimal(18,2), DiscountPrice decimal(18,2),AvailableFor nvarchar(100),IsShared bit)       
as       
begin       
	Set @CheckOutDate= DateAdd(DAY,-1,@CheckOutDate)
	
	DECLARE @TempTable TABLE (RoomId bigint,Name nvarchar(50),Type int, Price decimal(18,2), DiscountPrice decimal(18,2),AvailableFor nvarchar(100),IsShared bit)
	
	DECLARE @RoomId BIGINT
	DECLARE RoomCursor CURSOR LOCAL FAST_FORWARD FOR SELECT Id FROM FarmStaysRooms where FarmStaysId = @FarmStaysId
	OPEN RoomCursor
	FETCH NEXT FROM RoomCursor INTO @RoomId
	WHILE @@FETCH_STATUS = 0 BEGIN
	
	        
	if 	@IsSolo = 0 
	begin 	
		insert into @TempTable
		select Id,Name,Type,		
		(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 	 as Price,
		 ((dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 
		- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson),0))
		)
		 as DiscountPrice
		,AvailableFor
		,0 as IsShared from (select
		Id, 
		Name,
		Type,
		MaxPerson, 
		isnull((select sum(NumberOfPerson) from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId),0) as booked,
		(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) as Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price
		,null as AvailableFor 
		from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest
	end 
		else 
	begin
			insert into @TempTable		
			select Id,Name,Type,
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 	 as Price,
			(
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 
			- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest),0))
			)
			 as DiscountPrice
			 ,AvailableFor
			 ,1 as IsShared from 
			( select ID,Name,Type,MaxPerson, 
			isnull((select sum(Booked) from ( select case when (MaxPerson - sum(NumberOfPerson)) >= @NoOfGuest then 0 else 1 end as Booked  from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId  group by BookingDate,MaxPerson) as t),0) as booked,
			(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price,
			(select top 1 (cast(min(NoOfAvailable) as nvarchar(10)) +' '+ AvailableFor) as AvailableFor from ( select (MaxPerson - sum(NumberOfPerson)) as NoOfAvailable,(case when om.IsMale = 1 then 'Availability for Male' else 'Availability for Female' end) as AvailableFor  from  InventoryMaster im inner join OrderMaster om on im.OrderId = om.Id  where im.IsSoloBooking = 1 and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId =@RoomId  group by BookingDate,MaxPerson,om.IsMale) as t group by AvailableFor) as AvailableFor  
			from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest and AvailableFor is not null

	end

	FETCH NEXT FROM RoomCursor INTO @RoomId
	END
	CLOSE RoomCursor
	--DEALLOCATE RoomCursor

	if 	@IsSolo = 1
	begin
	 
		set @RoomId = 0 
		OPEN RoomCursor
		FETCH NEXT FROM RoomCursor INTO @RoomId
		WHILE @@FETCH_STATUS = 0 BEGIN
		
		IF EXISTS (SELECT * FROM @TempTable WHERE AvailableFor is not null)
		BEGIN
			insert into @TempTable
			select Id,Name,Type,		
			(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 	 as Price,
			((dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson) 
			- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * MaxPerson),0))
			)
			 as DiscountPrice
			,AvailableFor
			,0 as IsShared from (select
			Id, 
			Name,
			Type,
			MaxPerson, 
			isnull((select sum(NumberOfPerson) from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId),0) as booked,
			(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) as Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price
			,null as AvailableFor 
			from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest
			and Id not in (SELECT RoomId FROM @TempTable)

		END	
		ELSE
		BEGIN

			insert into @TempTable		
			select Id,Name,Type,
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 	 as Price,
			(
			(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest) 
			- (select  sum(DiscountAmount) from GetAppliedDiscountOnFarmstays(@FarmStaysId,@CheckInDate,@CheckOutDate,(dbo.ApplyMarkup(@FarmStaysId,Price) * @NoOfGuest),0))
			)
			 as DiscountPrice
			 ,AvailableFor 
			 ,1 as IsShared from 
			( select ID,Name,Type,MaxPerson, 
			isnull((select sum(Booked) from ( select case when (MaxPerson - sum(NumberOfPerson)) >= @NoOfGuest then 0 else 1 end as Booked  from  InventoryMaster where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId = @RoomId  group by BookingDate,MaxPerson) as t),0) as booked,
			(select isnull(sum(Price),-1) from ( select count(*) as dayscount,sum(Price) Price from FarmStaysSeasons where RoomId = @RoomId and BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate ) as t where dayscount = (DATEDIFF(DAY, @CheckInDate, @CheckOutDate) + 1)) as price,
			(select top 1 (cast(min(NoOfAvailable) as nvarchar(10)) +' '+ AvailableFor) as AvailableFor from ( select (MaxPerson - sum(NumberOfPerson)) as NoOfAvailable,(case when om.IsMale = 1 then 'Availability for Male' else 'Availability for Female' end) as AvailableFor  from  InventoryMaster im inner join OrderMaster om on im.OrderId = om.Id  where BookingDate >= @CheckInDate and BookingDate <= @CheckOutDate and RoomId =@RoomId  group by BookingDate,MaxPerson,om.IsMale) as t group by AvailableFor) as AvailableFor  
			from FarmStaysRooms  where id = @RoomId ) as t where booked = 0 and price <> -1 and isnull(MaxPerson,0) >= @NoOfGuest 


		END


		FETCH NEXT FROM RoomCursor INTO @RoomId
		END
		CLOSE RoomCursor
	end 
	DEALLOCATE RoomCursor


	insert into @MinPriceTable
	select * from @TempTable order by Price asc
	  
return       
end 


