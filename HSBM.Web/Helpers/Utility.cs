using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using HSBM.Common;
using System.IO;
using HSBM.Common.Utils;
using System.Collections.Specialized;
using HSBM.Common.Enums;
using System.Web.UI;
using System.Text;
using System.Reflection;
using System.Net;

using System.Xml;
using HSBM.EntityModel.Common;



namespace HSBM.Web.Helpers
{
    public static class Utility
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static GridParams GetGridParams(NameValueCollection p_Request)
        {
            try
            {
                GridParams p_GridParams = new GridParams();
                p_GridParams.draw = Convert.ToInt16(p_Request["draw"]);
                p_GridParams.pageSize = Convert.ToInt16(p_Request["length"]);
                p_GridParams.skip = Convert.ToInt16(p_Request["start"]);
                p_GridParams.take = Convert.ToInt16(p_Request["length"]);

                if (Convert.ToInt16(p_Request["order[0][column]"]) != 0)
                {
                    int ColumnIndex = Convert.ToInt16(p_Request["order[0][column]"]);
                    p_GridParams.DefaultOrderBy = p_Request["columns[" + ColumnIndex + "][data]"] + " " + p_Request["order[0][dir]"];
                }
                else
                {
                    p_GridParams.DefaultOrderBy = "CreatedDate Desc";
                }
                return p_GridParams;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static bool GetVideoThumbnail(string p_VideoPath, string p_ImagePath)
        {
            try
            {
                var _VideoConverter = new NReco.VideoConverter.FFMpegConverter();
                _VideoConverter.GetVideoThumbnail(p_VideoPath, p_ImagePath, 5);

                if (System.IO.File.Exists(p_ImagePath))
                {
                    Image _ThumbnailImage;

                    _ThumbnailImage = ResizeImage(p_ImagePath, 300, 200);
                    System.IO.File.Delete(p_ImagePath);
                    _ThumbnailImage.Save(p_ImagePath);
                }
                return true;
            }
            catch (Exception _Exception)
            {
                _ILogger.Error(_Exception.Message);
                throw _Exception;
            }
        }

        public static Image ResizeImage(string p_File, int p_Width, int p_Height)
        {
            try
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(p_File);

                System.Drawing.Image thumbnail = new System.Drawing.Bitmap(p_Width, p_Height); // changed parm names
                System.Drawing.Graphics graphic = System.Drawing.Graphics.FromImage(thumbnail);

                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                /* ------------------ new code --------------- */

                // Now calculate the X,Y position of the upper-left corner 
                // (one of these will always be zero)
                int posX = 0;// Convert.ToInt32((canvasWidth - (originalWidth * ratio)) / 2);
                int posY = 0;// Convert.ToInt32((canvasHeight - (originalHeight * ratio)) / 2);

                //graphic.Clear(System.Drawing.Color.White); // white padding                
                graphic.DrawImage(image, posX, posY, p_Width, p_Height);

                image.Dispose();
                return thumbnail;
            }
            catch (Exception _Exception)
            {
                _ILogger.Error(_Exception.Message);
                throw _Exception;
            }
        }

        public static List<KeyValueDto> GetEditionIPAddress(string p_IpAddress)
        {
            //string url = "http://freegeoip.net/xml/";

            string url = String.Format("http://freegeoip.net/xml/{0}", p_IpAddress);
            WebClient wc = new WebClient();
            wc.Proxy = null;
            MemoryStream ms = new MemoryStream(wc.DownloadData(url));
            XmlTextReader rdr = new XmlTextReader(url);
            XmlDocument doc = new XmlDocument();
            ms.Position = 0;
            doc.Load(ms);
            ms.Dispose();
            List<KeyValueDto> retval = new List<KeyValueDto>();
            foreach (XmlElement el in doc.ChildNodes[0].ChildNodes)
            {
                retval.Add(new KeyValueDto() { Key = el.Name, strValue = el.InnerText });
            }
            return retval;
        }


    }
}