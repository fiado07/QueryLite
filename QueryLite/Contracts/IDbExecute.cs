using System;
using System.Collections.Generic;


namespace QueryLite.Contracts
{
    public interface IDbExecute
    {

        void ExecuteSql(Parameters.SqlAndParameters sqlParameter);


        T ExecuteSqlGetObject<T>(Parameters.SqlAndParameters sqlParameter) where T : new();


        bool any(Parameters.SqlAndParameters sqlParameter);


        IEnumerable<T> ExecuteSqlGetObjectList<T>(Parameters.SqlAndParameters sqlParameter) where T : new();


        System.Data.DataTable ExecuteSqlGetDataTable(Parameters.SqlAndParameters sqlParameter);


        void Add<T>(T dataObject, List<string> excludeProperties = null, bool isStoredProcedure = false);

        void Update<T>(T dataObject, string predicate, List<string> excludeProperties = null, bool isStoredProcedure = false);

        void Delete<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();

        T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();

        IEnumerable<T> GetList<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();

        bool any<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class, new();



    }
}
