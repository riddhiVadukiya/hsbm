using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Repository.Contracts
{
    public interface ISystemUsersRepository : IRepository<SystemUsers>
    {

         List<SystemUsers> GetAllSystemUsers(SystemUsersRequest filter);

         List<SystemUsers> GetAllSystemUsers(SystemUsersRequest filter, GridParams p_GridParams);

        // SystemUsers GetSystemUserById(long SystemUserKey);

         SystemUsers GetSystemUserByLoginCredentials(string UserName, string Password);

        // long AddOrUpdateSystemUser(SystemUsers p_SystemUser);

        // long ChangePassword(long SystemUserKey, string OldPassword, string NewPassword);

        // long DeleteSystemUser(long SystemUsersKey);

         List<SystemUsersResponse> GetAllSubuserByParentIdForGrid(SystemUsersRequest p_SearchRequest, GridParams p_GridParams);

        //List<SystemUsersResponse> GetAllSystemUsers();

        //List<SystemUsersResponse> GetAllSystemUsers(SystemUsersRequest filter);

        //List<SystemUsersResponse> GetAllSystemUsers(SystemUsersRequest filter, GridParams p_GridParams);

        //SystemUsersResponse GetSystemUserById(Guid SystemUserKey);

        //Guid AddOrUpdateSystemUser(SystemUsers p_SystemUser);

        //Guid DeleteSystemUser(Guid SystemUsersKey);

        //SystemUsersResponse GetSystemUserByLoginCredentials(string UserName, string Password);

        //Guid ChangePassword(Guid SystemUserKey, string OldPassword, string NewPassword);

    }
}
