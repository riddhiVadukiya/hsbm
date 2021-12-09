namespace HSBM.EntityModel.SystemUsers
{
    using System;

    public class SystemUsersResponse : SystemUsers
    {

        public int RecordsTotal { get; set; }

        public string RoleName { get; set; }
    }
}
