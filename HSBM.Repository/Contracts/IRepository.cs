using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using HSBM.EntityModel;
using HSBM.Common.Enums;
using HSBM.Common.Utils;

namespace HSBM.Repository.Contracts
{
    public interface IRepository<T>
        where T : EntityBase
    {
        Table<T> GetTable();

        T SelectById(long id);

        long Insert(T entity);

        long Insert(DbManager db, T entity);

        long InsertWithIdentity(T entity);

        long InsertWithIdentity(DbManager db, T entity);

        long Save(T entity);

        long Update(T entity);

        long Update(DbManager db, T entity);
                
        long UpdateField<TV>(long id, Expression<Func<T, TV>> extract, TV value);
                
        long UpdateField<TV>(DbManager db, long id, Expression<Func<T, TV>> extract, TV value);

        void Delete(T entity);

        void Delete(DbManager db, T entity);

        void DeleteById(long id);

        void DeleteById(DbManager db, long id);

        T CreateNewEntity();

        IQueryable<T> SelectBy(FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters);

        IQueryable<T> SelectBy(DbManager db, FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters);

        IQueryable<T> Select(DbManager db, Expression<Func<T, bool>> predicate);

        IQueryable<T> Select(Expression<Func<T, bool>> predicate);

        Type GetTypeEntity();

        IQueryable<T> ApplyPagingSorting<T>(GridParams gridParams, IQueryable<T> entity);
    }


    public interface IRepository
    {
        object SelectById(long id);

        long Insert(object entity);

        long Insert(DbManager db, object entity);

        long InsertWithIdentity(object entity);

        long InsertWithIdentity(DbManager db, object entity);

        long Save(object entity);

        long Save(DbManager db, object entity);

        long Update(object entity);

        long Update(DbManager db, object entity);

        void Delete(object entity);

        void Delete(DbManager db, object entity);

        void DeleteById(long id);

        void DeleteById(DbManager db, long id);

        object CreateNewEntity();

        IQueryable SelectBy(FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters);

        IQueryable SelectBy(DbManager dbManager, FilterType filterType, IEnumerable<KeyValuePair<string, object>> parameters);

        Type GetTypeEntity();

        string GetPropertyName(string dbName);

        string GetDbPropertyName(string propertyName);
    }
}
