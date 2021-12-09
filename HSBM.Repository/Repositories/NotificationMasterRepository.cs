using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSBM.EntityModel.Notification;
using HSBM.Common.Utils;
using BLToolkit.Data;
using HSBM.Common.Enums;
using HSBM.EntityModel.SystemUsers;

namespace HSBM.Repository.Repositories
{
    public class NotificationMasterRepository
    {
        public List<NotificationResponse> GetAllNotification(GridParams p_GridParams)
        {
            List<NotificationResponse> objList = new List<NotificationResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Title],[Message],RecordsTotal = COUNT(*) OVER() from [dbo].[NotificationMaster]";

                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<NotificationResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddNotification(Notification _Notification)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    _SqlQuery = @"insert into NotificationMaster
                                                        (UserType
                                                        ,NotificationType
                                                        ,Title
                                                        ,Message
                                                        ,CreatedBy
                                                        ,CreatedDate)
                                                    OUTPUT Inserted.Id
                                                   values
                                                        (@UserType
                                                        ,@NotificationType
                                                        ,@Title
                                                        ,@Message
                                                        ,@CreatedBy
                                                        ,@CreatedDate)";

                    int Affected = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@UserType", UserTypes.Customer),
                        _DbManager.Parameter("@NotificationType", NotificationType.Admin),
                        _DbManager.Parameter("@Title", _Notification.Title),
                        _DbManager.Parameter("@Message", _Notification.Message),
                        _DbManager.Parameter("@CreatedBy", _Notification.CreatedBy),
                        _DbManager.Parameter("@CreatedDate", _Notification.CreatedDate)).ExecuteScalar<int>();

                    List<SystemUsers> _SystemUserWebList = null;
                    if (_Notification.UserType == Convert.ToInt32(UserTypes.Customer))
                    {
                        string _User = @"select Id,FirstName,UserType from SystemUsers where UserType = @UserType AND IsActive=1";

                        _SystemUserWebList = _DbManager.SetCommand(_User,
                                                                    _DbManager.Parameter("@UserType", _Notification.UserType)).ExecuteList<SystemUsers>();
                    }

                    List<DeviceNotificationData> _NotificationDeviceData = new List<DeviceNotificationData>();

                    if (_SystemUserWebList.Count > 0)
                    {
                        foreach (SystemUsers _SystemUserWeb in _SystemUserWebList)
                        {
                            string _DeviceDetail = @"select Id,SystemUserId,DeviceId,GCMRegistrationId,IsNotificationAllow,Provider,CreatedDate
                                                            from UserDeviceDetail where SystemUserId = @SystemUserId";

                            List<UserDeviceDetail> listOfUserDeviceDetail = _DbManager.SetCommand(_DeviceDetail,
                                                         _DbManager.Parameter("@SystemUserId", _SystemUserWeb.Id))
                                                         .ExecuteList<UserDeviceDetail>();

                            if (listOfUserDeviceDetail != null && listOfUserDeviceDetail.Count > 0)
                            {
                                foreach (UserDeviceDetail _UserDeviceDetail in listOfUserDeviceDetail)
                                {
                                    _NotificationDeviceData.Add(new DeviceNotificationData() { DeviceId = _UserDeviceDetail.DeviceId, GCMRegistrationId = _UserDeviceDetail.GCMRegistrationId, Provider = _UserDeviceDetail.Provider });
                                }
                            }

                            string _NotificationDetail = @"insert into NotificationDetail
                                                                      (NotificationMasterId
                                                                      ,SystemUserId
                                                                      ,IsDeleted
                                                                      ,IsViewed
                                                                      ,CreatedDate)
                                                                 values
                                                                      (@NotificationMasterId
                                                                      ,@SystemUserId
                                                                      ,@IsDeleted
                                                                      ,@IsViewed
                                                                      ,@CreatedDate)";
                            int _NotificationDetailAffected = _DbManager.SetCommand(_NotificationDetail,
                                            _DbManager.Parameter("@NotificationMasterId", Affected),
                                            _DbManager.Parameter("@SystemUserId", _SystemUserWeb.Id),
                                            _DbManager.Parameter("@IsDeleted", false),
                                            _DbManager.Parameter("@IsViewed", false),
                                            _DbManager.Parameter("@CreatedDate", DateTime.Now)).ExecuteNonQuery();
                        }
                    }

                    if (_NotificationDeviceData.Count > 0)
                    {
                        PushDeviceNotification _PushDeviceNotification = new PushDeviceNotification();
                        _PushDeviceNotification.InitDevicePushService(_Notification.UserType);
                        _PushDeviceNotification.SendDeviceNotification(_NotificationDeviceData, _Notification.Message, 0, 1, _Notification.Title);
                    }
                    return 1;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public Notification GetNotificationById(long Id)
        {
            Notification _Notification = new Notification();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select Id, Title, Message from NotificationMaster where Id = @Id";

                    _Notification = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<Notification>().FirstOrDefault();

                    return _Notification;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void DeleteNotificationById(long Id)
        {
            try
            {
                String _SqlQuery = string.Empty;
                using (DbManager _DbManager = new DbManager())
                {
                    _SqlQuery = @"Delete NotificationDetail where NotificationMasterId = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                       _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                    _SqlQuery = @"Delete NotificationMaster where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                       _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
