using HSBM.Common.Utils;
using HSBM.EntityModel.CityMaster;
using System.Collections.Generic;
using System.IO;
namespace HSBM.Repository.Contracts
{
    public interface ICityMasterRepository : IRepository<CityMaster>
    {
        List<CityMasterResponse> GetAllCityBySearchRequest(GridParams p_GridParams, CityMasterRequest p_SearchRequest);

    }
}