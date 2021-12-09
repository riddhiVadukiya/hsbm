using HSBM.Common.Utils;
using HSBM.EntityModel.RegionMaster;
using System.Collections.Generic;
namespace HSBM.Repository.Contracts
{
    public interface IRegionMasterRepository : IRepository<RegionMaster>
    {
        List<RegionMasterResponse> GetAllRegionBySearchRequest(GridParams p_GridParams, RegionMasterRequest p_SearchRequest);
    }
}