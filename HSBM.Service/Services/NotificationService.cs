using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.Notification;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class NotificationService
    {
        /// <summary>
        /// Add Notification
        /// </summary>
        /// <param name="_Notification">Notification</param>
        /// <returns> integer </returns>
        public int AddNotification(Notification _Notification)
        {
            NotificationMasterRepository _NotificationMasterRepository = new NotificationMasterRepository();
            return _NotificationMasterRepository.AddNotification(_Notification);
        }

        /// <summary>
        /// Delete Notification ById
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteNotificationById(long Id)
        {
            NotificationMasterRepository _NotificationMasterRepository = new NotificationMasterRepository();

            _NotificationMasterRepository.DeleteNotificationById(Id);
        }

        /// <summary>
        /// Get All Notification
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllNotification(GridParams p_GridParams)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            NotificationMasterRepository _NotificationMasterRepository = new NotificationMasterRepository();

            var _Notification = _NotificationMasterRepository.GetAllNotification(p_GridParams);

            if (_Notification != null && _Notification.Count > 0)
            {
                _GridDataResponse.recordsTotal = _Notification.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                _GridDataResponse.data = _Notification;
            }

            return _GridDataResponse;
        }

        /// <summary>
        /// Get Notification ById
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of Notification </returns>
        public Notification GetNotificationById(long Id)
        {
            NotificationMasterRepository _NotificationMasterRepository = new NotificationMasterRepository();

            Notification _Notification = new Notification();

            _Notification = _NotificationMasterRepository.GetNotificationById(Id);

            return _Notification;
        }
    }
}
