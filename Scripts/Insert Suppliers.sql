-- Insert System User
insert into SystemUsers (ParentId,UserType,RoleMasterId,UserName,Password,FirstName,LastName,MiddleName,Email,Telephone,Mobile,
Gender,Address,Address2,CountryMasterID,RegionMasterID,CityMasterID,CreatedBy,CreatedDate,
IsActive)

select 0 as ParentId, 6 as UserType, 1 as RoleMasterId, 'Sabre' as UserName, '9vW9W/naficcJEHNpTVihw==' as Password, 
'Sabre' as FirstName, '' as LastName, Null as MiddleName, 'test@Sabre.com' as  Email, null as Telephone, null as Mobile,
null as Gender, null as Address, null as Address2, 5 as CountryMasterId, 1 as RegionMasterId, 8274 as CityMasterId,
0 as CreatedBy, getdate() as CreatedDate, 1 as IsActive

union

select 0 as ParentId, 6 as UserType, 1 as RoleMasterId, 'RoomsXML' as UserName, '9vW9W/naficcJEHNpTVihw==' as Password, 
'RoomsXML' as FirstName, '' as LastName, Null as MiddleName, 'test@roomsxml.com' as  Email, null as Telephone, null as Mobile,
null as Gender, null as Address, null as Address2, 5 as CountryMasterId, 1 as RegionMasterId, 8274 as CityMasterId,
0 as CreatedBy, getdate() as CreatedDate, 1 as IsActive

union

select 0 as ParentId, 6 as UserType, 1 as RoleMasterId, 'HotelBeds' as UserName, '9vW9W/naficcJEHNpTVihw==' as Password, 
'HotelBeds' as FirstName, '' as LastName, Null as MiddleName, 'test@HotelBeds.com' as  Email, null as Telephone, null as Mobile,
null as Gender, null as Address, null as Address2, 5 as CountryMasterId, 1 as RegionMasterId, 8274 as CityMasterId,
0 as CreatedBy, getdate() as CreatedDate, 1 as IsActive

union

select 0 as ParentId, 6 as UserType, 1 as RoleMasterId, 'Amadeus' as UserName, '9vW9W/naficcJEHNpTVihw==' as Password, 
'Amadeus' as FirstName, '' as LastName, Null as MiddleName, 'test@Amadeus.com' as  Email, null as Telephone, null as Mobile,
null as Gender, null as Address, null as Address2, 5 as CountryMasterId, 1 as RegionMasterId, 8274 as CityMasterId,
0 as CreatedBy, getdate() as CreatedDate, 1 as IsActive

union

select 0 as ParentId, 6 as UserType, 1 as RoleMasterId, 'Galileo' as UserName, '9vW9W/naficcJEHNpTVihw==' as Password, 
'Galileo' as FirstName, '' as LastName, Null as MiddleName, 'test@Galileo.com' as  Email, null as Telephone, null as Mobile,
null as Gender, null as Address, null as Address2, 5 as CountryMasterId, 1 as RegionMasterId, 8274 as CityMasterId,
0 as CreatedBy, getdate() as CreatedDate, 1 as IsActive

------------------
-- Insert SupplierMaster
insert into SupplierMaster (UserId,SupplierType,CompanyName,Address,PhoneNumber,MobileNumber,CityId,MarkUp,MarkupType,
IsActive,CreatedBy,CreatedDate)

select 56 as UserId, 1 as SupplierType, 'Sabre' as CompanyName, 'S G Highway' as Address, null as PhoneNumber, null as MobileNumber,
8274 as cityid, 10 as markup, 1 as markuptype, 1 as isactive, 0 as createdby, getdate() as createddate
union
select 55 as UserId, 2 as SupplierType, 'Rooms XML' as CompanyName, 'C G Road' as Address, null as PhoneNumber, null as MobileNumber,
8274 as cityid, 10 as markup, 1 as markuptype, 1 as isactive, 0 as createdby, getdate() as createddate
union
select 54 as UserId, 3 as SupplierType, 'HotelBeds' as CompanyName, 'Ellis Bridge' as Address, null as PhoneNumber, null as MobileNumber,
8274 as cityid, 10 as markup, 1 as markuptype, 1 as isactive, 0 as createdby, getdate() as createddate
union
select 52 as UserId, 5 as SupplierType, 'Amadeus' as CompanyName, 'Ashram Road' as Address, null as PhoneNumber, null as MobileNumber,
8274 as cityid, 10 as markup, 1 as markuptype, 1 as isactive, 0 as createdby, getdate() as createddate
union
select 53 as UserId, 4 as SupplierType, 'Galilio' as CompanyName, 'Usman Pura' as Address, null as PhoneNumber, null as MobileNumber,
8274 as cityid, 10 as markup, 1 as markuptype, 1 as isactive, 0 as createdby, getdate() as createddate


