using HSBM.Repository.Contracts;
using HSBM.EntityModel.EmailTemplates;
using System.Collections.Generic;
using HSBM.Common.Utils;
using BLToolkit.Data;
using System.Linq;
using System;
using System.Web.Mvc;

namespace HSBM.Repository.Repositories
{
    public class EmailTemplatesRepository
    {
        public List<EmailTemplatesResponse> GetAllEmailTemplatesBySearchRequest(GridParams p_GridParams, EmailTemplatesRequest p_SearchRequest)
        {
            List<EmailTemplatesResponse> objList = new List<EmailTemplatesResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT *,RecordsTotal = COUNT(*) OVER() from [dbo].[EmailTemplates]";

                    bool IsWhereClauseEmpty = true;

                    if (p_SearchRequest.IncludeIsDeleted==false)
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " [IsActive] = 1 ";
                    }
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<EmailTemplatesResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }

        public int AddOrUpdateEmailTemplates(EmailTemplates emailTemplates)
        {
            try
            {
                String _SqlQuery;
                using (DbManager _DbManager = new DbManager())
                {
                    bool _IsExistEmailTempName = CheckNameForDuplicate(_DbManager, emailTemplates);

                    if (_IsExistEmailTempName)
                    {
                        return 2;
                    }
                    else
                    {
                        if (emailTemplates.Id != 0)
                        {
                            _SqlQuery = @"update EmailTemplates set Name=@Name
                                                                       ,TemplateType = @TemplateType
                                                                       ,Subject = @Subject
                                                                       ,TemplatesHtml = @TemplatesHtml
                                                                       ,IsActive = @IsActive
                                                                       ,UpdatedBy = @UpdatedBy
                                                                       ,UpdateDate = @UpdateDate
                                                                        where (Id = @Id)";
                        }
                        else
                        {
                            _SqlQuery = @"insert into EmailTemplates
                                                        (Name
                                                        ,TemplateType
                                                        ,Subject
                                                        ,TemplatesHtml
                                                        ,IsActive
                                                        ,CreatedBy
                                                        ,CreatedDate
                                                        ,UpdatedBy
                                                        ,UpdateDate)
                                                   values
                                                        (@Name
                                                        ,@TemplateType
                                                        ,@Subject
                                                        ,@TemplatesHtml
                                                        ,@IsActive
                                                        ,@CreatedBy
                                                        ,@CreatedDate
                                                        ,@UpdatedBy
                                                        ,@UpdateDate)";
                        }
                        int Affected = _DbManager.SetCommand(_SqlQuery,
                            _DbManager.Parameter("@Name", emailTemplates.Name),
                            _DbManager.Parameter("@TemplateType", emailTemplates.TemplateType),
                            _DbManager.Parameter("@Subject", emailTemplates.Subject),
                            _DbManager.Parameter("@TemplatesHtml", emailTemplates.TemplatesHtml),
                            _DbManager.Parameter("@IsActive", emailTemplates.IsActive),
                            _DbManager.Parameter("@CreatedBy", emailTemplates.CreatedBy),
                            _DbManager.Parameter("@CreatedDate", emailTemplates.CreatedDate),
                            _DbManager.Parameter("@UpdatedBy", emailTemplates.UpdatedBy),
                            _DbManager.Parameter("@UpdateDate", emailTemplates.UpdateDate),
                            _DbManager.Parameter("@Id", emailTemplates.Id)).ExecuteNonQuery();

                        if (Affected > 0)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public bool CheckNameForDuplicate(DbManager _DbManager, EmailTemplates emailTemplates)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from EmailTemplates where Id!=@Id and Name=@Name";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", emailTemplates.Id),
                    _DbManager.Parameter("@Name", emailTemplates.Name)).ExecuteScalar<int>();

                if (Affected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void ActiveAndInactiveSwitchUpdateForET(EmailTemplates emailTemplates)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Update EmailTemplates set IsActive = @IsActive where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", emailTemplates.Id),
                        _DbManager.Parameter("@IsActive", emailTemplates.IsActive)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public EmailTemplates GetEmailTemplatesById(long Id)
        {
            EmailTemplates _EmailTemplatesModel = new EmailTemplates();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Name],[TemplateType],[Subject],[TemplatesHtml],[IsActive],CreatedDate,CreatedBy, RecordsTotal = COUNT(*) OVER() from [dbo].[EmailTemplates] where Id = @Id";

                    _EmailTemplatesModel = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<EmailTemplates>().FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _EmailTemplatesModel;
        }

        public void DeleteEmailTemplatesById(long Id)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Delete EmailTemplates where Id = @Id";

                    _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteNonQuery();

                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<SelectListItem> EmailTemplatesDropDown()
        {
            List<SelectListItem> _EmailTemplateList = new List<SelectListItem>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select Name as Text, Id as Value from EmailTemplates";

                    _EmailTemplateList = _DbManager.SetCommand(_SqlQuery).ExecuteList<SelectListItem>();

                    if (_EmailTemplateList.Count > 0)
                    {
                        return _EmailTemplateList;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public EmailTemplates GetEmailTemplateByTypeId(int Id)
        {
            EmailTemplates _EmailTemplatesModel = new EmailTemplates();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[Name],[TemplateType],[Subject],[TemplatesHtml] from [dbo].[EmailTemplates] where TemplateType = @Id and IsActive=1";

                    _EmailTemplatesModel = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Id", Id)).ExecuteList<EmailTemplates>().FirstOrDefault();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return _EmailTemplatesModel;
        }
    }
}