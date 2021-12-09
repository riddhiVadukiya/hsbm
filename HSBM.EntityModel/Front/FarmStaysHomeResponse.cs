using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel.Front
{
    public class FarmStaysHomeResponse
    {
        public long Id { get; set; }

        public string ImageName { get; set; }

        public string ImageOrignalName { get; set; }

        public string Alt { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public long OrderIndex { get; set; }

        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }
        
        public bool IsActive { get; set; }
    }
}
