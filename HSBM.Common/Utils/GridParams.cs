using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Common.Utils
{
    public class GridParams
    {
        public int draw { get; set; }

        public int skip { get; set; }

        public int take { get; set; }

        public int page { get; set; }

        public int pageSize { get; set; }

        public string DefaultOrderBy { get; set; }

        public List<GridSort> sort { get; set; }
    }

    public class GridSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
}
