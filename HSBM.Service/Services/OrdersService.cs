using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.OrdersMaster;
using HSBM.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class OrdersService
    {
        /// <summary>
        /// Grid of Orders
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">OrdersMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllOrdersBySearchRequest(GridParams p_GridParams, OrdersMasterRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();

            OrdersMasterRepository _OrdersMasterRepository = new OrdersMasterRepository();

            var _Orders = _OrdersMasterRepository.GetAllOrdersMasterBySearchRequest(p_GridParams, p_SearchRequest);
            
            if (_Orders .Any())
            {
                //_GridDataResponse.recordsTotal = _Orders.FirstOrDefault().RecordsTotal;
                //_GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                //_GridDataResponse.data = _Orders;
                _GridDataResponse.recordsTotal = _Orders.FirstOrDefault().RecordsTotal;
                _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;

            }
            //else
            //{
            //    _GridDataResponse.data = _Orders;
            //}

            _GridDataResponse.data = _Orders;

            return _GridDataResponse;
        }

        /// <summary>
        /// Grid of Orders
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">OrdersMasterRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public OrdersMaster GetOrderDetailByKey(long id)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            OrdersMaster _OrdersMaster = new OrdersMaster();
            OrdersMasterRepository _OrdersMasterRepository = new OrdersMasterRepository();

            _OrdersMaster = _OrdersMasterRepository.GetOrderDetailByKey(id);

            if (_OrdersMaster != null)
            {
                return _OrdersMaster;
            }            
            return null;
        }
    }
}
