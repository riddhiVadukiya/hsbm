using HSBM.Common.Enums;
using HSBM.EntityModel.BookingPassengerDetail;
using HSBM.Repository.Repositories;
using HSBM.Service.ServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Service.Services
{
    public class HotelBookingService
    {
        public static readonly HSBM.Common.Logging.ILogger _ILogger = HSBM.Common.Logging.LogWrapper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        
        /// <summary>
        /// Get Date from string
        /// </summary>
        /// <param name="dt">dt</param>
        /// <returns> DateTime </returns>
        public DateTime GetDate(string dt)
        {
            string[] arr = dt.Split(Convert.ToChar("-"));
            return new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[1]), Convert.ToInt32(arr[0]));
        }


    }
}
