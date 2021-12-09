using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSBM.Web.Areas.Admin.Models
{
    public class ExportToCSV : ActionResult
    {
        public string CSVData { get; set; }
        public string fileName { get; set; }


        public ExportToCSV(List<object> ListToConvert, List<string> Header, string pFileName )
        {
            List<List<string>> dataList = new List<List<string>>();

            dataList.Add(Header);


            //iterate through list items
            foreach (var item in ListToConvert)
            {
                //get properties and values 
                System.Reflection.PropertyInfo[] props = item.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                List<string> itemValues = new List<string>();

                //iterate through properties
                foreach (var prop in props)
                    itemValues.Add(prop.GetValue(item).ToString());

                dataList.Add(itemValues);
            }

            //flatten out lists and return results
            CSVData= string.Join(Environment.NewLine, dataList.Select(i => string.Join(",", i.Select(v => v.ToString()))));
            fileName = pFileName + ".csv";
        }

        public override void ExecuteResult(ControllerContext context)
        {

           //Create a response stream to create and write the Excel file
            HttpContext curContext = HttpContext.Current;
            curContext.Response.Clear();
            curContext.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            curContext.Response.Charset = "";
            curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            curContext.Response.ContentType = "text/csv";

            curContext.Response.Output.Write(CSVData);
            curContext.Response.End();

        }
    }
}