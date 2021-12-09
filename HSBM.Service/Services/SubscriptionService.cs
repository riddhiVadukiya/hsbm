using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.SubscriptionMaster;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class SubscriptionService
    {
        /// <summary>
        /// Active/Inactive Subscription
        /// </summary>
        /// <param name="subscriptionMaster">SubscriptionMaster</param>
        public void ActiveAndInactiveSwitchUpdateForSC(SubscriptionMaster subscriptionMaster)
        {
            SubscriptionMasterRepository _SubscriptionMasterRepository = new SubscriptionMasterRepository();

            _SubscriptionMasterRepository.ActiveAndInactiveSwitchUpdateForSC(subscriptionMaster);
        }

        /// <summary>
        /// Add / Update Subscription
        /// </summary>
        /// <param name="subscriptionMaster">SubscriptionMaster</param>
        /// <returns> integer </returns>
        public int AddUpdateSubscription(SubscriptionMaster subscriptionMaster)
        {
            SubscriptionMasterRepository _SubscriptionMasterRepository = new SubscriptionMasterRepository();

            return _SubscriptionMasterRepository.AddUpdateSubscription(subscriptionMaster);
        }

        /// <summary>
        /// Delete Subscription ById
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteSubscriptionById(long Id)
        {
            SubscriptionMasterRepository _SubscriptionMasterRepository = new SubscriptionMasterRepository();

            _SubscriptionMasterRepository.DeleteSubscriptionById(Id);
        }

        /// <summary>
        /// Get All Subscription
        /// </summary>
        /// <returns> List of SubscriptionMaster </returns>
        public List<SubscriptionMaster> GetAllSubscription()
        {
            SubscriptionMasterRepository _SubscriptionMasterRepository = new SubscriptionMasterRepository();

            List<SubscriptionMaster> _SubscriptionMasterList = new List<SubscriptionMaster>();

            _SubscriptionMasterList = _SubscriptionMasterRepository.GetAllSubscription();

            return _SubscriptionMasterList;
        }

        /// <summary>
        /// Grid of Subscription
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">SubscriptionMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllSubscriptionBySearchRequest(GridParams p_GridParams, SubscriptionMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            SubscriptionMasterRepository _SubscriptionMasterRepository = new SubscriptionMasterRepository();

            var _Country = _SubscriptionMasterRepository.GetAllSubscriptionMasterBySearchRequest(p_GridParams, p_SearchRequest);

            if (_Country != null && _Country.Count > 0)
            // if (_Country != null)
            {
                _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                _GridDataResponse.data = _Country;
            }
            else
            {
                //_GridDataResponse.recordsTotal = 0;
                //_GridDataResponse.recordsFiltered = 0;
                _GridDataResponse.data = _Country;
            }
            return _GridDataResponse;
        }

        /// <summary>
        /// Unsubscribe User
        /// </summary>
        /// <param name="Id">Id</param>
        public void UnsubscribeUser(long Id)
        {
            SubscriptionMasterRepository _SubscriptionMasterRepository = new SubscriptionMasterRepository();

            _SubscriptionMasterRepository.UnsubscribeUser(Id);
        }
    }
}
