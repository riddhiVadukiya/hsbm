using HSBM.Repository.Contracts;
using HSBM.EntityModel.CountryMaster;
using System.Collections.Generic;
using HSBM.Common.Utils;
using BLToolkit.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Linq;
using System.IO;
using HSBM.EntityModel.CityMaster;

namespace HSBM.Repository.Repositories
{
    public class CountryMasterRepository : Repository<CountryMaster>, ICountryMasterRepository
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string Country_Name = "country name";
        public const string Code = "code";
        public const int Name_Length = 50;
        public const int Code_Length = 50;

        /// <summary>
        /// Grid of Countries
        /// </summary>
        /// <param name="p_GridParams">GridParams</param>
        /// <param name="p_SearchRequest">CountryMasterRequest</param>
        /// <returns> List of CountryMasterResponse </returns>
        public List<CountryMasterResponse> GetAllCountriesBySearchRequest(GridParams p_GridParams, CountryMasterRequest p_SearchRequest)
        {
            List<CountryMasterResponse> objList = new List<CountryMasterResponse>();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    string _SqlQuery = @"SELECT [Id],[CountryName],[Code],[IsActive],RecordsTotal = COUNT(*) OVER() FROM [dbo].[CountryMaster]";

                    bool IsWhereClauseEmpty = true;
                    if (!string.IsNullOrEmpty(p_SearchRequest.CountryName))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " [CountryName] like '%" + p_SearchRequest.CountryName + "%' ";
                    }

                    if (!string.IsNullOrEmpty(p_SearchRequest.Code))
                    {
                        if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                        IsWhereClauseEmpty = false;
                        _SqlQuery += " [Code] like '%" + p_SearchRequest.Code + "%' ";
                    }

                    //if (!p_SearchRequest.IncludeIsDeleted)
                    //{
                    //    if (IsWhereClauseEmpty) { _SqlQuery += " WHERE "; } else { _SqlQuery += " AND "; }
                    //    IsWhereClauseEmpty = false;
                    //    _SqlQuery += " [IsActive] = 1 ";
                    //}
                    if (p_GridParams != null)
                    {
                        _SqlQuery += @" ORDER BY " + p_GridParams.DefaultOrderBy + " OFFSET " + p_GridParams.skip + " ROWS FETCH NEXT " + p_GridParams.take + " ROWS ONLY ";
                    }

                    objList = _DbManager.SetCommand(_SqlQuery).ExecuteList<CountryMasterResponse>();
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return objList;
        }


        /// <summary>
        /// Get All Country Master
        /// </summary>
        /// <returns> List of CountryMaster </returns>
        public List<CountryMaster> GetAllCountryMaster()
        {
            List<CountryMaster> listOfCountryMaster = new List<CountryMaster>();

            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Select CountryName, Code, IsActive, Id from CountryMaster";

                    listOfCountryMaster = _DbManager.SetCommand(_SqlQuery).ExecuteList<CountryMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return listOfCountryMaster;
        }

        /// <summary>
        /// Import Country Document
        /// </summary>
        /// <param name="p_File">Stream of Country</param>
        /// <returns> String Response </returns>
        public string ImportCountryDocument(Stream p_File)
        {
            string _Eroor = string.Empty;

            List<CountryMaster> _CountryMasterList = new List<CountryMaster>();
            List<CountryMaster> _CountryList = new List<CountryMaster>();

            List<CountryMaster> _UpdateCountryList = new List<CountryMaster>();
            try
            {
                String _SqlQuery;
                int Affected = 0;
                using (DbManager _DbManager = new DbManager())
                {

                    SpreadsheetDocument _SpreadsheetDocument = SpreadsheetDocument.Open(p_File, false);

                    WorkbookPart _WorkbookPart = _SpreadsheetDocument.WorkbookPart;
                    IEnumerable<Sheet> _ListOfSheet = _SpreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                    string _relationshipId = _ListOfSheet.First().Id.Value;
                    WorksheetPart _WorksheetPart = (WorksheetPart)_SpreadsheetDocument.WorkbookPart.GetPartById(_relationshipId);
                    Worksheet _Worksheet = _WorksheetPart.Worksheet;
                    SheetData _SheetData = _Worksheet.GetFirstChild<SheetData>();
                    IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Row> _ListOfRow = _SheetData.Descendants<DocumentFormat.OpenXml.Spreadsheet.Row>();
                    SharedStringTablePart _SharedStringTablePart = _SpreadsheetDocument.WorkbookPart.SharedStringTablePart;

                    int _RowCount = 0;
                    List<List<string>> _RowList = new List<List<string>>();

                    foreach (var _row in _SheetData.Elements<DocumentFormat.OpenXml.Spreadsheet.Row>())
                    {
                        //if (_RowCount > 101)
                        //{
                        //    break;
                        //}
                        var _Count = 0;

                        string[] _Letters = new string[2] { "A", "B" };

                        List<string> _cellsList = new List<string>();

                        int _CellCount = _row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().ToArray().Count();
                        if (_CellCount != 2)
                        {
                            //_Response.StatusCode = (HttpStatusCode)StandardStatusCodes.INTERNAL_SERVER_ERROR;
                            //_Response.StatusMessage = Helper.CultureMessage(p_Lang, ResourceConstant.File_is_not_in_proper_format);
                            //return _Response;
                            return "File is not in proper format";
                        }


                        #region read data from Excel and Bind Row list

                        foreach (var _cell in _row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>().ToArray())
                        {
                            while (_cell.CellReference.ToString()[0] != Convert.ToChar(_Letters[_Count]))
                            {
                                //accounts for multiple consecutive blank cells
                                _cellsList.Add("");
                                _Count++;
                            }

                            if (_cell.CellValue == null)
                            {
                                _cellsList.Add("");
                            }
                            else
                            {
                                if (_cell.DataType != null && _cell.DataType.Value == CellValues.SharedString)
                                {
                                    _cellsList.Add(_SharedStringTablePart.SharedStringTable.ChildElements[Int32.Parse(_cell.CellValue.InnerText.ToString())].InnerText);
                                }
                                else
                                {
                                    _cellsList.Add(_cell.CellValue.InnerText);
                                }
                            }
                            _Count++;
                        }

                        if (_RowCount == 0)
                        {
                            if (_CellCount < 2 || _cellsList[0].ToString().ToLower().Trim() != Country_Name || _cellsList[1].ToString().ToLower().Trim() != Code)
                            {
                                //_Response.StatusCode = (HttpStatusCode)StandardStatusCodes.INTERNAL_SERVER_ERROR;
                                //_Response.StatusMessage = Helper.CultureMessage(p_Lang, ResourceConstant.File_is_not_in_proper_format_Please_Check_Cell_Name_Or_Count);
                                //return _Response;
                                return "File is not in proper format Please Check Cell Name Or Count";
                            }
                            _RowCount++;
                        }
                        else
                        {
                            if (_cellsList.Count() < 2)
                            {
                                int _intCellCount = 2 - _cellsList.Count();

                                for (int _cell = 0; _cell < _intCellCount; _cell++)
                                {
                                    _cellsList.Add("");
                                }
                            }
                            if (_cellsList[0].ToString() == "" && _cellsList[1].ToString() == "") { }
                            else
                            {
                                _RowList.Add(_cellsList);
                                _RowCount++;
                            }
                        }
                        #endregion
                    }

                    if (_RowCount > 0)
                    {
                        #region read data from row list and Bind job list

                        for (int _row = 0; _row < _RowList.Count(); _row++)
                        {


                            // set Country Object 
                            CountryMaster _CountryMaster = new CountryMaster();

                            if (_RowList[_row][0].Length > Name_Length || _RowList[_row][1].Length > Code_Length)
                            {
                                if (_RowList[_row][0].Length > Name_Length)
                                    _ILogger.Error(_RowList[_row][0] + "(Country Name) length is more then " + Name_Length);
                                if (_RowList[_row][1].Length > Code_Length)
                                    _ILogger.Error(_RowList[_row][1] + "(Country Code) length is more then " + Code_Length);
                            }

                            else
                            {
                                _CountryMaster.CountryName = _RowList[_row][0];
                                _CountryMaster.Code = _RowList[_row][1].ToString().Trim();
                                _CountryMasterList.Add(_CountryMaster);
                            }



                        }
                        if (_CountryMasterList != null && _CountryMasterList.Count > 0)
                        {

                            foreach (CountryMaster _Country in _CountryMasterList)
                            {
                                var _Temp = _CountryMasterList.Where(x => x.CountryName.ToLower() == _Country.CountryName.ToLower());

                                if (_Temp != null && _Temp.Count() > 1)
                                {
                                    _ILogger.Error(_Country.CountryName + "(Country Name) is exists in sheet more then one time");
                                }
                                else
                                {
                                    bool _IsExistCountryName = CheckCountryNameForDuplicate(_DbManager, _Country);

                                    if (!_IsExistCountryName)
                                        _CountryList.Add(_Country);

                                    else if (_IsExistCountryName)
                                        _UpdateCountryList.Add(_Country);
                                }

                                //if (_Temp != null && _Temp.Count() > 1)
                                //{
                                //    _Country.HasError = true;
                                //    _CountryList.Add(_Country);
                                //}
                                //else
                                //{
                                //    bool _IsExistCountryName = CheckCountryNameForDuplicate(_DbManager, _Country);

                                //    if (!_IsExistCountryName)
                                //    {
                                //        _CountryList.Add(_Country);
                                //    }

                                //    if (_IsExistCountryName)
                                //    {
                                //        _Country.HasError = true;
                                //        _CountryList.Add(_Country);
                                //    }
                                //}

                            }


                        }

                        if (_CountryList.Count > 0 || _UpdateCountryList.Count > 0)
                        {
                            //CountryMaster _CountryMasterListResponse = new CountryMaster();
                            //_CountryMasterListResponse.ListOfCountryMaster = _CountryList;
                            //_Response.StatusCode = (HttpStatusCode)StandardStatusCodes.SUCCESS;
                            //_Response.ResponseData = _CountryMasterListResponse;
                            //return _Response;

                            if (_CountryList.Count > 0)
                            {
                                foreach (CountryMaster _Country in _CountryList)
                                {
                                    try
                                    {
                                        string _CountryQuery = @"insert into CountryMaster(
                                                               CountryName
                                                              ,Code
                                                              ,IsActive)
                                                       values
                                                            (@CountryName
                                                            ,@Code
                                                            ,@IsActive)";

                                        _DbManager.SetCommand(_CountryQuery,
                                             _DbManager.Parameter("@CountryName", _Country.CountryName),
                                             _DbManager.Parameter("@Code", _Country.Code),
                                             _DbManager.Parameter("@IsActive", true)).ExecuteNonQuery();
                                    }
                                    catch (Exception _Exception)
                                    {
                                        _ILogger.Error("Error in saving country (" + _Country.CountryName + ").");
                                        _Eroor = _Eroor + " ," + _Country.CountryName;
                                    }
                                }
                            }
                            if (_UpdateCountryList.Count > 0)
                            {
                                foreach (CountryMaster _Country in _UpdateCountryList)
                                {
                                    try
                                    {
                                        string _CountryQuery = @" update CountryMaster set Code='" + _Country.Code + "' where CountryName='" + _Country.CountryName + "'";

                                        _DbManager.SetCommand(_CountryQuery).ExecuteNonQuery();
                                    }
                                    catch (Exception _Exception)
                                    {
                                        _ILogger.Error("Error in updating country (" + _Country.CountryName + ").");
                                        _Eroor = _Eroor + " ," + _Country.CountryName;
                                    }
                                }
                            }

                            if (_Eroor != string.Empty)
                                return "Country " + _Eroor + " are not changed";

                            return _Eroor;
                        }
                        else
                        {
                            return "Please import proper file with valid input";
                            //_Response.StatusCode = (HttpStatusCode)StandardStatusCodes.INTERNAL_SERVER_ERROR;
                            //_Response.StatusMessage = Helper.CultureMessage(p_Lang, ResourceConstant.Please_import_proper_file_with_valid_input);
                        }

                        #endregion
                    }

                }
            }
            catch (Exception _Exception)
            {
                _ILogger.Error(_Exception.Message);
                //_Response.StatusCode = (HttpStatusCode)StandardStatusCodes.INTERNAL_SERVER_ERROR;
                //_Response.StatusMessage = Helper.CultureMessage(p_Lang, ResourceConstant.Unexpected_Error);
                return "Error";
            }
            // return _Response;
            return "No data found";
        }

        /// <summary>
        /// Duplication check of country name
        /// </summary>
        /// <param name="_DbManager">DbManager</param>
        /// <param name="_CountryMaster">CountryMaster</param>
        /// <returns> true/false </returns>
        private bool CheckCountryNameForDuplicate(DbManager _DbManager, CountryMaster _CountryMaster)
        {
            String _SqlQuery = @" select COUNT(*) from CountryMaster where CountryName= @CountryName";
            int _CountryName = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@CountryName", _CountryMaster.CountryName)).ExecuteScalar<int>();

            if (_CountryName > 0)
                return true;

            return false;
        }

        #region Active Inactive Country ById
        /// <summary>
        /// Active Inactive Country ById
        /// </summary>
        /// <param name="p_CountryMaster">Object of CountryMaster</param>
        /// <returns> true/false </returns>
        public bool ActiveAndInactiveSwitchForCountryUpdate(CountryMaster p_CountryMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Update CountryMaster set IsActive = @IsActive Where Id = @Id";

                    int _Result = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@IsActive", p_CountryMaster.IsActive),
                         _DbManager.Parameter("@Id", p_CountryMaster.Id)).ExecuteNonQuery();

                    if (_Result > 0)
                    {
                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion

        #region AddUpdate CountryMaster
        /// <summary>
        /// Add Update Country
        /// </summary>
        /// <param name="p_CountryMaster">Object Of Country Master</param>
        /// <returns> true/false </returns>
        public bool AddUpdateCountry(CountryMaster p_CountryMaster)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = string.Empty;

                    bool _IsExistCountryName = CheckNameForDuplicate(_DbManager, p_CountryMaster);

                    if (_IsExistCountryName)
                    {
                        return false;
                    }
                    else
                    {
                        if (p_CountryMaster.Id > 0)
                        {
                            _SqlQuery = @"Update CountryMaster set CountryName=@CountryName, 
                                                Code=@Code, 
                                                Latitude=@Latitude, 
                                                Longitude=@Longitude,
                                                IsActive = @IsActive
                                                Where Id=@Id";
                        }
                        else
                        {
                            _SqlQuery = @"Insert into CountryMaster (CountryName, Code, IsActive, Latitude, Longitude)
                                            Values (@CountryName, @Code, @IsActive, @Latitude, @Longitude)";
                        }

                        int _Result = _DbManager.SetCommand(_SqlQuery,
                             _DbManager.Parameter("@CountryName", p_CountryMaster.CountryName)
                            , _DbManager.Parameter("@Code", p_CountryMaster.Code)
                            , _DbManager.Parameter("@IsActive", p_CountryMaster.IsActive)
                            , _DbManager.Parameter("@Latitude", p_CountryMaster.Latitude)
                            , _DbManager.Parameter("@Longitude", p_CountryMaster.Longitude)
                            , _DbManager.Parameter("@Id", p_CountryMaster.Id)).ExecuteNonQuery();

                        //String _SqlQuery = "Delete from CountryMaster Where Id = @Id";

                        if (_Result > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return false;
        }
        
        /// <summary>
        /// Duplication check of country name HotelBeds
        /// </summary>
        /// <param name="code">code</param>
        /// <returns> true/false </returns>
        public bool CheckCountryExistsForHotelBeds(string code)
        {
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = @"Select Count(Id) from CountryMaster where Code = @Code";

                    int count = _DbManager.SetCommand(_SqlQuery,
                        _DbManager.Parameter("@Code", code)).ExecuteScalar<int>();

                    if (count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        
        
        /// <summary>
        /// Duplication check of country name
        /// </summary>
        /// <param name="_DbManager">DbManager</param>
        /// <param name="p_CountryMaster">CountryMaster</param>
        /// <returns> true/false value </returns>
        private bool CheckNameForDuplicate(DbManager _DbManager, CountryMaster p_CountryMaster)
        {
            try
            {
                String _SqlQuery = @"Select Count(Id) from CountryMaster where Id != @Id and CountryName = @CountryName";

                int Affected = _DbManager.SetCommand(_SqlQuery,
                    _DbManager.Parameter("@Id", p_CountryMaster.Id),
                    _DbManager.Parameter("@CountryName", p_CountryMaster.CountryName)).ExecuteScalar<int>();

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
        #endregion

        #region Get Country ById
        /// <summary>
        /// Get Country By Id
        /// </summary>
        /// <param name="p_Id">Country Id</param>
        /// <returns>CountryMaster</returns>
        public CountryMaster GetCountryById(long p_Id)
        {
            CountryMaster countryMaster = new CountryMaster();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Select Id, CountryName, Code, Latitude, Longitude, IsActive from CountryMaster Where Id= @Id";

                    countryMaster = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Id", p_Id)).ExecuteObject<CountryMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return countryMaster;
        }
        #endregion

        #region Delete Country ById
        /// <summary>
        /// Delete Country ById
        /// </summary>
        /// <param name="p_Id">CountryId</param>
        /// <returns>bool</returns>
        public bool DeleteCountryById(long p_Id)
        {
            try
            {
                using (DbManager _DBManager = new DbManager())
                {
                    String _SqlQuery = "Delete From CountryMaster Where Id = @Id";
                    int _Result = _DBManager.SetCommand(_SqlQuery, _DBManager.Parameter("@Id", p_Id)).ExecuteNonQuery();

                    if (_Result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        #endregion


        #region GetCountry
        /// <summary>
        /// Get Country
        /// </summary>
        /// <returns> List of CountryMaster </returns>
        public List<CountryMaster> GetCountry(List<Tuple<string, string>> param)
        {
            List<CountryMaster> countryMaster = new List<CountryMaster>();
            try
            {
                string strWHERE = string.Empty;
                String _SqlQuery = string.Empty;


                using (DbManager _DbManager = new DbManager())
                {
                    if (param != null && param.Count > 0)
                    {
                        strWHERE = " where " + param[0].Item1 + "='" + param[0].Item2 + "'";
                        for (int i = 1; i < param.Count; i++)
                            strWHERE = strWHERE + "and " + param[i].Item1 + "='" + param[i].Item2 + "'";

                        _SqlQuery = "Select Id, CountryName, Code, Latitude, Longitude, IsActive from CountryMaster " + strWHERE;
                    }
                    else
                    {
                        _SqlQuery = "Select Id, CountryName, Code, Latitude, Longitude, IsActive from CountryMaster ";
                    }

                    countryMaster = _DbManager.SetCommand(_SqlQuery).ExecuteList<CountryMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return countryMaster;
        }
        #endregion

        public CountryMaster GetCountryByCountryCode(string p_Code)
        {
            CountryMaster countryMaster = new CountryMaster();
            try
            {
                using (DbManager _DbManager = new DbManager())
                {
                    String _SqlQuery = "Select Id from CountryMaster Where Code= @Code";

                    countryMaster = _DbManager.SetCommand(_SqlQuery, _DbManager.Parameter("@Code", p_Code)).ExecuteObject<CountryMaster>();
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return countryMaster;
        }
    }
}