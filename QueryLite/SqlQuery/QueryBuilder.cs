using System;
using System.Collections.Generic;
using System.Data;
using QueryLite.Contracts;
using QueryLite.Parameters;
using QueryLite.Repository;

namespace QueryLite.SqlQuery
{
    /// <summary>
    /// Build Query API
    /// </summary>
    /// <seealso cref="QueryLite.Contracts.IDbExecute" />
    public class QueryBuilder : IDbExecute
    {

        private static IDbConnectionSql DbConnection { set; get; }
        private static readonly Lazy<QueryBuilder> lazyLogging = new Lazy<QueryBuilder>(() => new QueryBuilder());
        private DbExecuteQuery dbQuery;

        private QueryBuilder()
        {
            dbQuery = new DbExecuteQuery(DbConnection);
        }

        public static QueryBuilder GetQueryBuilder(IDbConnectionSql dbConnection)
        {

            DbConnection = dbConnection;

            return lazyLogging.Value;

        }

        public void ExecuteSql(SqlAndParameters sqlParameter)
        {
            dbQuery.ExecuteSql(sqlParameter);
        }

        public T ExecuteSqlGetObject<T>(SqlAndParameters sqlParameter) where T : new()
        {
            return dbQuery.ExecuteSqlGetObject<T>(sqlParameter);
        }

        public bool any(SqlAndParameters sqlParameter)
        {
            return dbQuery.any(sqlParameter);
        }

        public IEnumerable<T> ExecuteSqlGetObjectList<T>(SqlAndParameters sqlParameter) where T : new()
        {
            return dbQuery.ExecuteSqlGetObjectList<T>(sqlParameter);
        }

        public DataTable ExecuteSqlGetDataTable(SqlAndParameters sqlParameter)
        {
            return dbQuery.ExecuteSqlGetDataTable(sqlParameter);
        }
    }
}
