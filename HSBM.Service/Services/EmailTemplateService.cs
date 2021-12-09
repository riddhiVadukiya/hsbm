using HSBM.Common.Utils;
using HSBM.EntityModel.Common;
using HSBM.EntityModel.EmailTemplates;
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
    public class EmailTemplateService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Active Inactive EmailTemplate
        /// </summary>
        /// <param name="emailTemplates"> Object of EmailTemplates </param>
        public void ActiveAndInactiveSwitchUpdateForET(RequestResponseServiceContext requestResponseServiceContext, EmailTemplates emailTemplates)
        {
            EmailTemplatesRepository _EmailTemplatesRepository = new EmailTemplatesRepository();
            try
            {
                _EmailTemplatesRepository.ActiveAndInactiveSwitchUpdateForET(emailTemplates);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                _ILogger.Error("EmailTemplateService=>ActiveAndInactiveSwitchUpdateForET: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Add Or Update EmailTemplates
        /// </summary>
        /// <param name="emailTemplates"> EmailTemplates </param>
        /// <returns> integer </returns>
        public int AddOrUpdateEmailTemplates(RequestResponseServiceContext requestResponseServiceContext, EmailTemplates emailTemplates)
        {
            EmailTemplatesRepository _EmailTemplatesRepository = new EmailTemplatesRepository();
            int result = 0;

            try
            {
                result = _EmailTemplatesRepository.AddOrUpdateEmailTemplates(emailTemplates);

                if (result > 0)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return result;
                }
                else
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.NOT_FOUND;
                    requestResponseServiceContext.Response.StatusParameters = new string[] { "Not Found." };
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _ILogger.Error("EmailTemplateService=>AddOrUpdateEmailTemplates: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return 0;
            }
        }

        /// <summary>
        /// Delete EmailTemplate By Id
        /// </summary>
        /// <param name="Id">Id</param>
        public void DeleteEmailTemplatesById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            EmailTemplatesRepository _EmailTemplatesRepository = new EmailTemplatesRepository();
            try
            {
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                _EmailTemplatesRepository.DeleteEmailTemplatesById(Id);
            }
            catch (Exception ex)
            {
                _ILogger.Error("EmailTemplateService=>DeleteEmailTemplatesById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
            }
        }

        /// <summary>
        /// Get EmailTemplates DropDown
        /// </summary>
        /// <returns> List of SelectListItem </returns>
        public List<SelectListItem> EmailTemplatesDropDown(RequestResponseServiceContext requestResponseServiceContext)
        {
            List<SelectListItem> _EmailTemplateList = new List<SelectListItem>();
            EmailTemplatesRepository _EmailTemplatesRepository = new EmailTemplatesRepository();

            try
            {
                _EmailTemplateList = _EmailTemplatesRepository.EmailTemplatesDropDown();
                if (_EmailTemplateList.Any())
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _EmailTemplateList;
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
                _ILogger.Error("EmailTemplateService=>EmailTemplatesDropDown: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        /// <summary>
        /// Grid of EmailTempalates
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">EmailTemplatesRequest</param>
        /// <returns> Object of GridDataResponse </returns>
        public GridDataResponse GetAllEmailTempalatesBySearchRequest(RequestResponseServiceContext requestResponseServiceContext, GridParams p_GridParams, EmailTemplatesRequest p_SearchRequest)
        {
            GridDataResponse _GridDataResponse = new GridDataResponse();
            EmailTemplatesRepository _EmailTemplatesRepository = new EmailTemplatesRepository();

            try
            {
                var _Country = _EmailTemplatesRepository.GetAllEmailTemplatesBySearchRequest(p_GridParams, p_SearchRequest);

                if (_Country != null && _Country.Count > 0)
                {
                    _GridDataResponse.recordsTotal = _Country.FirstOrDefault().RecordsTotal;
                    _GridDataResponse.recordsFiltered = _GridDataResponse.recordsTotal;
                    _GridDataResponse.data = _Country;

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
                _ILogger.Error("EmailTemplateService=>GetAllEmailTempalatesBySearchRequest: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        /// <summary>
        /// Get EmailTemplate By Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of EmailTemplate </returns>
        public EmailTemplates GetEmailTemplatesById(RequestResponseServiceContext requestResponseServiceContext, long Id)
        {
            EmailTemplatesRepository _EmailTemplatesRepository = new EmailTemplatesRepository();
            EmailTemplates _EmailTemplates = new EmailTemplates();

            try
            {
                _EmailTemplates = _EmailTemplatesRepository.GetEmailTemplatesById(Id);
                if (_EmailTemplates != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _EmailTemplates;
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
                _ILogger.Error("EmailTemplateService=>GetEmailTemplatesById: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }

        /// <summary>
        /// Get EmailTemplate By TypeId
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns> Object of EmailTemplate </returns>
        public EmailTemplates GetEmailTemplateByTypeId(RequestResponseServiceContext requestResponseServiceContext, int Id)
        {
            EmailTemplatesRepository _EmailTemplatesRepository = new EmailTemplatesRepository();
            EmailTemplates _EmailTemplates = new EmailTemplates();

            try
            {
                _EmailTemplates = _EmailTemplatesRepository.GetEmailTemplateByTypeId(Id);
                if (_EmailTemplates != null)
                {
                    requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.SUCCESS;
                    return _EmailTemplates;
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
                _ILogger.Error("EmailTemplateService=>GetEmailTemplateByTypeId: Error :- ", ex);
                requestResponseServiceContext.Response.StatusCode = StandardStatusCodes.INTERNAL_SERVER_ERROR;
                requestResponseServiceContext.Response.StatusParameters = new string[] { "Internal Server Error." };
                return null;
            }
        }
    }
}
