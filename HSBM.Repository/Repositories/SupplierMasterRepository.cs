using BLToolkit.Data;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.EntityModel.SystemUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Repository.Repositories
{
    public class SupplierMasterRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                
        public List<SelectListItem> GetCountryListFromSupplierLocationsForDropDown(long id)
        {
            List<SelectListItem> _List = new List<SelectListItem>();
            try
            {
                string _Query = @"
SELECT cm.Id AS Value, cm.CountryName AS Text FROM CountryMaster cm
INNER JOIN SupplierLocations sl ON sl.CountryId = cm.Id
WHERE sl.CreatedBy = @Id AND sl.IsActive = 'true' 
GROUP BY cm.Id, cm.CountryName
";
                using (DbManager _DbManager = new DbManager())
                {
                    _List = _DbManager.SetCommand(_Query, _DbManager.Parameter("Id", id)).ExecuteList<SelectListItem>();
                    if (_List.Any())
                    {
                        return _List;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> GetAirportsFromSupplierLocations(long id)
        {
            List<SelectListItem> _List = new List<SelectListItem>();
            try
            {
                string _Query = @"
SELECT am.Id AS Value, am.AirportCode + ' - ' + am.AirportName AS [Text] FROM AirportsMaster am
INNER JOIN SupplierLocations sl ON sl.CountryId = am.Id
WHERE sl.CreatedBy = @Id AND sl.IsActive = 'true' 
GROUP BY am.Id, am.AirportName, am.AirportCode
";
                using (DbManager _DbManager = new DbManager())
                {
                    _List = _DbManager.SetCommand(_Query, _DbManager.Parameter("Id", id)).ExecuteList<SelectListItem>();
                    if (_List.Any())
                    {
                        return _List;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
