using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.EntityModel
{
    public interface IEntity
    {
        long Id { get; set; }
        bool IsDirty { get; }
        bool IsNew { get; }        
        Dictionary<string, object> Parameters { get; }

        IEntity CopyTo(IEntity entity);
        object Clone();
        object GetValue(string fieldName);
        void SetValue(string fieldName, object value);
    }
}
