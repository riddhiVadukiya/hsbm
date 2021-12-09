namespace HSBM.EntityModel.SystemUsers
{
    using System;

    public class SystemUsersRequest
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Gender { get; set; }
        public Nullable<long> CountryMasterID { get; set; }
        public Nullable<long> StateMasterID { get; set; }
        public Nullable<long> CityMasterID { get; set; }
        public int UserType { get; set; }

        public long ParentId { get; set; }

        //Custom
        public bool IncludeIsDeleted { get; set; }

    }
}
