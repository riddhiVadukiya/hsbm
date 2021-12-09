using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HSBM.EntityModel
{
    [Serializable]
    public class EntityBase : EditableObject, IEntity
    {
        #region IEntity Members

        [PrimaryKey, NonUpdatable]
        public long Id { get; set; }

        [MapIgnore]
        public virtual Dictionary<string, object> Parameters
        {
            get
            {
                var objMapper = Map.GetObjectMapper(GetType());

                var result = (from fieldName in objMapper.FieldNames
                              select new
                              {
                                  FieldName = fieldName,
                                  Value = objMapper.GetValue(this, fieldName)
                              })
                               .ToDictionary(o => o.FieldName, o => o.Value);

                return result;
            }
        }

        public object GetValue(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException();

            var objMapper = Map.GetObjectMapper(GetType());
            return objMapper.GetValue(this, fieldName);
        }

        public void SetValue(string fieldName, object value)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentNullException();
            var objMapper = Map.GetObjectMapper(GetType());
            objMapper.SetValue(this, fieldName, value);
        }

        public IEntity CopyTo(IEntity entity)
        {
            return CopyTo((EntityBase)entity);
        }

        object IEntity.Clone()
        {
            return Clone();
        }

        #endregion
               
        
        [MapIgnore]
        public bool IsNew
        {
            get
            {
                return Id <= 0;
            }
        }
        
        

        public virtual EntityBase CopyTo(EntityBase obj)
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var info in properties)
            {
                if (!info.CanRead)
                    continue;

                var propDest = obj.GetType().GetProperty(info.Name);
                if (propDest == null || !propDest.CanWrite)
                    continue;

                if (info.PropertyType != propDest.PropertyType)
                    continue;

                var value = info.GetValue(this, null);
                propDest.SetValue(obj, value, null);
            }

            return obj;
        }

        public virtual EntityBase Clone()
        {
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream) as EntityBase;
            }
        }

        public static T Copy<T>(T entity)
            where T : EntityBase
        {
            return TypeAccessor<T>.Copy(entity, TypeAccessor<T>.CreateInstance());
        }
    }
}
