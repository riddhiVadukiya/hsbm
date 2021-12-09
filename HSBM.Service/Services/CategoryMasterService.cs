using HSBM.Common.Logging;
using HSBM.Common.Utils;
using HSBM.EntityModel.CategoryMaster;
using HSBM.EntityModel.Common;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HSBM.Service.Services
{
    public class CategoryMasterService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Category master by search request
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="p_GridParams"></param>
        /// <param name="p_Request"></param>
        /// <returns></returns>
        public GridDataResponse GetAllCategoryMastersBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, CategoryMasterRequest p_Request)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            CategoryMasterRepository _CategoryMasterRepository = new CategoryMasterRepository();
            try
            {
                _GridDataResponse = _CategoryMasterRepository.GetAllCategoryMastersBySearchRequest(p_GridParams, p_Request);
                if (_GridDataResponse.data != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _GridDataResponse;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CategoryMasterService=>GetAllCategoryMastersBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        /// <summary>
        /// Add or update Category master
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        public int AddOrUpdateCategoryMaster(RequestResponseServiceContext requestResponseServiceContext, CategoryMaster Category)
        {
            CategoryMasterRepository _CategoryMasterRepository = new CategoryMasterRepository();
            int result = 0;

            try
            {
                result = _CategoryMasterRepository.AddOrUpdateCategoryMaster(Category);
                if (result == 1)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                }
                else if (result == 2)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.IS_EXIST;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Category already exist." };
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not found." };
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CategoryMasterService=>AddOrUpdateCategoryMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }

            return result;
        }

        /// <summary>
        /// Get Category master by id
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CategoryMaster GetCategoryMasterById(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            CategoryMaster _CategoryMaster = new CategoryMaster();
            CategoryMasterRepository _CategoryMasterRepository = new CategoryMasterRepository();
            try
            {
                _CategoryMaster = _CategoryMasterRepository.GetCategoryMasterById(Id);
                if (_CategoryMaster != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _CategoryMaster;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CategoryMasterService=>GetAllCategoryMastersBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }

        }

        /// <summary>
        /// Active or inactive Category master
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <param name="Category"></param>
        public void ActiveAndInactiveCategoryMaster(RequestResponseServiceContext requestResponseServiceContext, CategoryMaster Category)
        {
            CategoryMasterRepository _CategoryMasterRepository = new CategoryMasterRepository();
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _CategoryMasterRepository.ActiveAndInactiveCategoryMaster(Category);
            }
            catch (Exception ex)
            {
                _ILogger.Error("CategoryMasterService=>ActiveAndInactiveCategoryMaster: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Get all Category master for drop down
        /// </summary>
        /// <param name="requestResponseServiceContext"></param>
        /// <returns></returns>
        public List<SelectListItem> GetAllCategoryMastersForDropDown(RequestResponseServiceContext requestResponseServiceContext)
        {
            CategoryMasterRepository _CategoryMasterRepository = new CategoryMasterRepository();
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                //list.Add(new SelectListItem() { Text = "Select", Value = "" });
                list.AddRange(_CategoryMasterRepository.GetAllCategoryMastersForDropDown());
                if (list.Any())
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return list;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return null;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("CategoryMasterService=>GetAllCategoryMastersForDropDown: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

    }
}
