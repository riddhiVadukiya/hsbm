using HSBM.Common.Enums;
using HSBM.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.FarmStays
{
    public class FarmStaysRooms
    {
        public int Id { get; set; }
        public long FarmStaysId { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public int MaxPerson { get; set; }
        public bool IsSolo { get; set; }
        public string TypeName { get; set; }
        //public string TypeName
        //{
        //    get
        //    {
        //        return Helper.GetEnumDescription((RoomType)Type);
        //    }
        //}
    }
}
