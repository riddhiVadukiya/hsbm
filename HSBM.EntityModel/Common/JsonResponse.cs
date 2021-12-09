using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Common
{
    public class JsonResponse
    {
        public string Errors { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }
    }

    public class ToastrMSG
    {
        public string ErrorTitle { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }    
    }

}
