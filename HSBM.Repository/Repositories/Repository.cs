using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using HSBM.EntityModel;
using HSBM.Repository.Contracts;
using HSBM.Common.Enums;
using HSBM.Common.Utils;
using HSBM.Common.Static;
namespace HSBM.Repository.Repositories
{

    public class Repository<T> : IRepository<T>, IRepository
        where T : EntityBase
    {
        protected DbManager GetDbManager()
        {
            return new DbManager();
        }

        public Table<T> GetTable(DbManager dbManager)
        {
            return GetTableCore(dbManager, false);
        }

        private Table<T> GetTableCore(DbManager dbManager, bool closeConnection)
        {
            return dbManager.GetTable<T>(closeConnection);
        }

        public Table<T> GetTable()
        {
            try
            {
                using (var db = GetDbManager())
                {
                    return GetTableCore(db, true);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Get table for {0} error", typeof(T).Name), ex);
            }
        }

        public virtual T SelectById(long id)
        {
            try
            {
                return (from item in GetTable()
                        where item.Id == id
                        select item).FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Select by id {0} error", typeof(T).Name), ex);
            }
        }

        public long Insert(object entity)
        {
            return Insert((T)entity);
        }

        public long Insert(DbManager db, object entity)
        {
            return Insert(db, (T)entity);
        }

        public long InsertWithIdentity(object entity)
        {
            return InsertWithIdentity((T)entity);
        }

        public long InsertWithIdentity(DbManager db, object entity)
        {
            return InsertWithIdentity(db, (T)entity);
        }

        public long Save(object entity)
        {
            return Save((T)entity);
        }

        public long Save(DbManager db, object entity)
        {
            return Save(db, (T)entity);
        }

        public long Update(object entity)
        {
            return Update((T)entity);
        }

        public long Update(DbManager db, object entity)
        {
            return Update(db, (T)entity);
        }

        public void Delete(object entity)
        {
            Delete((T)entity);
        }

        public void Delete(DbManager db, object entity)
        {
            Delete(db, (T)entity);
        }

        public virtual void Delete(IEnumerable<T> entity)
        {
            using (var db = GetDbManager())
            {
                Delete(db, entity);
            }
        }

        public virtual long Insert(T entity)
        {
            using (var db = GetDbManager())
            {
                var result = Insert(db, entity);
                return result;
            }
        }

        public virtual long Insert(DbManager db, T entity)
        {
            entity.Validate();

            int result;

            try
            {
                result = db.Insert(entity);

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Insert {0} error", typeof(T).Name), ex);
            }

            entity.AcceptChanges();

            return result;
        }

        public virtual long InsertWithIdentity(T entity)
        {
            using (var db = GetDbManager())
            {
                var result = InsertWithIdentity(db, entity);
                return result;
            }
        }

        public virtual long InsertWithIdentity(DbManager db, T entity)
        {
            entity.Validate();

            long result;

            try
            {
                result = Convert.ToInt64(db.InsertWithIdentity(entity));

                entity.Id = result;

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Insert with identity {0} error", typeof(T).Name), ex);
            }

            entity.AcceptChanges();

            return result;
        }

        public long Save(T entity)
        {
            return entity.IsNew
                           ? InsertWithIdentity(entity)
                           : Update(entity);
        }

        public long Save(DbManager db, T entity)
        {
            return entity.IsNew
                           ? InsertWithIdentity(db, entity)
                           : Update(db, entity);
        }

        public long UpdateField<TV>(long id, Expression<Func<T, TV>> extract, TV value)
        {
            using (var db = GetDbManager())
            {
                var result = UpdateField(db, id, extract, value);
                return result;
            }
        }

        public long UpdateField<TV>(DbManager db, long id, Expression<Func<T, TV>> extract, TV value)
        {
            var query = db.GetTable<T>()
                          .Where(e => e.Id == id)
                          .Set(extract, value);

            return query.Update();
        }

        public virtual long Update(T entity)
        {
            using (var db = GetDbManager())
            {
                var result = Update(db, entity);
                return result;
            }
        }

        public virtual long Update(DbManager db, T entity)
        {
            entity.Validate();

            int result;

            try
            {
                result = db.Update(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Update {0} error", typeof(T).Name), ex);
            }

            entity.AcceptChanges();

            return result;
        }

        public virtual void Delete(T entity)
        {
            using (var db = GetDbManager())
            {
                Delete(db, entity);
            }
        }

        public virtual void Delete(DbManager db, T entity)
        {
            try
            {
                db.Delete(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Delete {0} error", typeof(T).Name), ex);
            }
        }

        public virtual void Delete(DbManager db, IEnumerable<T> entity)
        {
            try
            {
                db.Delete(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Delete {0} error", typeof(T).Name), ex);
            }
        }

        object IRepository.SelectById(long id)
        {
            return SelectById(id);
        }

        public void DeleteById(long id)
        {
            using (var db = GetDbManager())
            {
                DeleteById(db, id);
            }
        }

        public void DeleteById(DbManager db, long id)
        {
            var entity = db.GetTable<T>().Where(e => e.Id == id).FirstOrDefault();

            try
            {
                db.Delete(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Delete by id {0} error", typeof(T).Name), ex);
            }

            entity.AcceptChanges();

            //try
            //{

            //    var entity = CreateNewEntity();
            //    entity.Id = id;

            //    db.Delete(entity);

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(string.Format("Delete by id {0} error", typeof(T).Name), ex);
            //}
        }

        object IRepository.CreateNewEntity()
        {
            return CreateNewEntity();
        }

        IQueryable IRepository.SelectBy(FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return SelectBy(filterType, parameters);
        }

        IQueryable IRepository.SelectBy(DbManager db, FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            return SelectBy(db, filterType, parameters);
        }

        public IQueryable<T> Select(DbManager db, Expression<Func<T, bool>> predicate)
        {
            return GetTableCore(db, true).Where(predicate).Select(x => x);
        }

        public IQueryable<T> Select(Expression<Func<T, bool>> predicate)
        {
            using (var db = GetDbManager())
            {
                return Select(db, predicate);
            }
        }

        public IQueryable Select()
        {
            return GetTable();
        }

        public IQueryable<T> SelectBy(FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            using (var db = GetDbManager())
            {
                var result = SelectBy(db, filterType, parameters);
                return result;
            }
        }

        public IQueryable<T> SelectBy(DbManager db, FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            try
            {
                var type = GetTypeEntity();

                var fields = GetFields();

                var query = GetTableCore(db, true) as IQueryable<T>;

                foreach (var param in parameters)
                {
                    if (!fields.Contains(param.Key))
                        throw new Exception(string.Format("Сутность {0} не содержит поля {1}",
                                                                    GetType().Name,
                                                                    param.Key), null);


                    var propertyName = GetPropertyName(param.Key);
                    var property = type.GetProperty(propertyName);

                    var propertyType = property.PropertyType;

                    var expressionParam = Expression.Parameter(type, type.Name);
                    var right = Expression.Constant(Convert.ChangeType(param.Value, propertyType), propertyType);
                    var left = Expression.Property(expressionParam, property);

                    LambdaExpression predicate;

                    switch (filterType)
                    {
                        case FilterType.Contains:
                            {
                                var filterContains = Expression.Call(left, MethodInfoHelpers.MethodContains, new Expression[] { right });
                                predicate = Expression.Lambda(filterContains, expressionParam);
                            }
                            break;
                        default:
                            {
                                var expr = Expression.Equal(left, right);
                                predicate = Expression.Lambda(expr, expressionParam);
                            }
                            break;
                    }

                    var expression = Expression.Call(typeof(Queryable), "Where", new[] { query.ElementType },
                             query.Expression,
                             predicate);

                    query = query.Provider.CreateQuery<T>(expression);
                }

                return query;

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Select by {0} error", typeof(T).Name), ex);
            }
        }

        public T CreateNewEntity()
        {
            var entity = TypeAccessor<T>.CreateInstanceEx();
            entity.Id = -1;
            return entity;
        }

        public Type GetTypeEntity()
        {
            return typeof(T);
        }

        public string GetPropertyName(string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
                return dbName;

            var properties = GetTypeEntity()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var property = (from p in properties
                            let mapFieldAttr =
                                 (MapFieldAttribute)Attribute.GetCustomAttribute(p, typeof(MapFieldAttribute))
                            where mapFieldAttr != null
                                  && mapFieldAttr.MapName.Equals(dbName, StringComparison.OrdinalIgnoreCase)
                            select p)
                            .FirstOrDefault();

            return property == null
                       ? dbName
                       : property.Name;
        }

        private static string GetDbPropertyName(PropertyInfo property)
        {
            var mapFieldAttribute = property.GetCustomAttributes(typeof(MapFieldAttribute), false)
                                                 .OfType<MapFieldAttribute>()
                                                 .FirstOrDefault();

            return mapFieldAttribute != null
                   ? string.IsNullOrEmpty(mapFieldAttribute.MapName)
                       ? property.Name
                       : mapFieldAttribute.MapName
                   : property.Name;
        }


        public string GetDbPropertyName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return propertyName;

            var propertyInfo = GetTypeEntity().GetProperty(propertyName);

            return GetDbPropertyName(propertyInfo);
        }

        private string[] GetFields()
        {
            var objMapper = Map.GetObjectMapper(GetTypeEntity());
            return objMapper.FieldNames;
        }

        public IQueryable<T> ApplyPagingSorting<T>(GridParams gridParams, IQueryable<T> entity)
        {
            try
            {
                if (gridParams.sort != null && gridParams.sort.Count > 0)
                {
                    foreach (var _sort in gridParams.sort)
                    {
                        entity = entity.OrderBy(_sort.Field + " " + _sort.Dir);
                    }
                }
                else
                {
                    entity = entity.OrderBy(gridParams.DefaultOrderBy);
                }

                return (IQueryable<T>)entity.Skip(gridParams.skip).Take(gridParams.take);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
