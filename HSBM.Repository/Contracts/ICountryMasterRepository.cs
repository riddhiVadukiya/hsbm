using HSBM.Common.Utils;
using HSBM.EntityModel.CountryMaster;
using System.Collections.Generic;
using System.IO;
namespace HSBM.Repository.Contracts
{
    public interface ICountryMasterRepository : IRepository<CountryMaster>
    {
       // List<CountryMasterResponse> GetAllCountriesBySearchRequest(GridParams p_GridParams, CountryMasterRequest p_SearchRequest);

        string ImportCountryDocument(Stream p_File);
    }
}