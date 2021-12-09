using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.Front;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class FrontOrdersService
    {
        /// <summary>
        /// Grid of Orders
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">OrdersMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllOrdersBySearchRequest(GridParams p_GridParams, FrontOrdersMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            FrontOrdersMasterRepository _OrdersMasterRepository = new FrontOrdersMasterRepository();

            var _Orders = _OrdersMasterRepository.GetAllOrdersMasterBySearchRequest(p_GridParams, p_SearchRequest);

            if (_Orders != null)
            {
                _GridDataResponse.recordsTotal = _Orders.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                _GridDataResponse.data = _Orders;
            }
            else
            {
                _GridDataResponse.data = _Orders;
            }
            return _GridDataResponse;
        }

        /// <summary>
        /// Grid of Orders
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">OrdersMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public FrontOrdersMaster GetOrderDetailByKey(long id, string CurrencyCode)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            FrontOrdersMaster _OrdersMaster = new FrontOrdersMaster();
            FrontOrdersMasterRepository _OrdersMasterRepository = new FrontOrdersMasterRepository();

            _OrdersMaster = _OrdersMasterRepository.GetOrderDetailByKey(id, CurrencyCode);

            if (_OrdersMaster != null)
            {
                return _OrdersMaster;
            }
            return null;
        }
    }
}
