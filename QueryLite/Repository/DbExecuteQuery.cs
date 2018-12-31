using QueryLite.Common;
using QueryLite.Contracts;
using QueryLite.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace QueryLite.Repository
{
    public class DbExecuteQuery : IDbExecute
    {
        private IDbConnectionSql DbConnection;

        public DbExecuteQuery(IDbConnectionSql dbConnection)
        {
            DbConnection = dbConnection;
        }

        private void ValidateConnection()
        {
            if (DbConnection?.DbConnectionBase?.ConnectionString == null)
                throw new Exception("Object connection or connectionString is null.");

            if (DbConnection.DbConnectionBase.State == ConnectionState.Closed)
                DbConnection.DbConnectionBase.Open();
        }

        public bool any(SqlAndParameters sqlParameter)
        {
            bool result = false;
            IDataReader readerResult = null;

            try
            {
                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // get and set values
                    readerResult = sqlComand.ExecuteReader();

                    while (readerResult.Read())
                    {
                        if (readerResult[0] != null) result = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (readerResult != null && !readerResult.IsClosed) readerResult.Close();

                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return result;
        }

        public void ExecuteSql(SqlAndParameters sqlParameter)
        {
            try
            {
                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql

                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // execute query
                    sqlComand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose)
                    DbConnection.DbConnectionBase.Close();
            }
        }

        public DataTable ExecuteSqlGetDataTable(SqlAndParameters sqlParameter)
        {
            DataSet dstable = new DataSet();
            IDbCommand sqlComand;

            try
            {
                ValidateConnection();

                if (DbConnection?.SetAdapter == null)
                    throw new Exception("Object SetAdapter can not be null, initialize!");

                // initialize sqlcommand
                sqlComand = DbConnection.DbConnectionBase.CreateCommand();

                if (DbConnection?.DbTransaction != null)
                    sqlComand.Transaction = DbConnection.DbTransaction;

                if (sqlComand.Parameters != null)
                    sqlComand.SetParameters(sqlParameter);

                // set sql
                sqlComand.CommandText = sqlParameter.Sql;

                // set command type
                sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                // set command on adapter
                DbConnection.SetAdapter.SelectCommand = sqlComand;

                // set get data
                DbConnection.SetAdapter.Fill(dstable);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return dstable.Tables[0];
        }

        public T ExecuteSqlGetObject<T>(SqlAndParameters sqlParameter) where T : new()
        {
            T objectType = new T();
            IDataReader readerResult = null;

            try
            {
                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand?.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // get and set values
                    readerResult = sqlComand.ExecuteReader(CommandBehavior.SingleRow);

                    // map data from reader to new object
                    objectType = Mappers.Mapper.Map<T>(readerResult).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (readerResult != null && !readerResult.IsClosed) readerResult.Close();
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return objectType;
        }

        public IEnumerable<T> ExecuteSqlGetObjectList<T>(SqlAndParameters sqlParameter) where T : new()
        {
            IEnumerable<T> objectTypeList;

            try
            {
                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;

                    // set command type
                    sqlComand.CommandType = sqlParameter.isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                    // get values from reader
                    IDataReader readerResult = sqlComand.ExecuteReader();

                    // get list of object from reader
                    objectTypeList = Mappers.Mapper.Map<T>(readerResult);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return objectTypeList;
        }

        public void Add<T>(T dataObject, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            string propertyFieldNames = string.Empty;
            string paramsFieldKeys = string.Empty;

            List<Parameter> parameters = new List<Parameter>();

            try
            {
                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    // set parameters and sql
                    sqlComand.SetParameters(dataObject, DbConnection.DbProvider, excludeProperties);

                    // execute
                    sqlComand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }
        }

        /// <summary>
        /// Updates the specified data object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataObject">The data object.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="excludeProperties">The exclude properties.</param>
        public void Update<T>(T dataObject, string predicate, List<string> excludeProperties = null, bool isStoredProcedure = false)
        {
            try
            {
                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    // set parameters and sql
                    sqlComand.SetParameters(dataObject, DbConnection.DbProvider, predicate, excludeProperties);

                    // execute
                    sqlComand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            T entity = new T();

            try
            {
                entity = GetList(predicate).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        public IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            IEnumerable<T> entityList = Enumerable.Empty<T>();
            T entity = new T();
            List<Parameter> parameters = new List<Parameter>();
            string stringPredicate = string.Empty;
            string fieldNames = string.Empty;

            try
            {
                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    stringPredicate = predicate.GetStringFromExpression();

                    //fieldNames = entity.GetPropertyNames();

                    // set parameters and sql
                    sqlComand.CommandText = $"select * from { typeof(T).Name  }  where {stringPredicate} ";

                    // execute and get list
                    entityList = Mappers.Mapper.Map<T>(sqlComand.ExecuteReader()).ToList();

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return entityList;
        }

        public bool any<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {


            T entity = new T();
            string stringPredicate = string.Empty;
            string fieldNames = string.Empty;
            bool any = false;


            try
            {

                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    stringPredicate = predicate.GetStringFromExpression();

                    //fieldNames = entity.GetPropertyNames();

                    // set parameters and sql
                    sqlComand.CommandText = $"select * from { typeof(T).Name  }  where {stringPredicate} ";

                    // execute and get list
                    any = sqlComand.ExecuteReader().Read();



                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

            return any;

        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
           
            string stringPredicate = string.Empty;
            string fieldNames = string.Empty;
         

            try
            {

                ValidateConnection();

                using (var sqlComand = DbConnection.DbConnectionBase?.CreateCommand())
                {
                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;

                    stringPredicate = predicate.GetStringFromExpression();

                  
                    // set parameters and sql
                    sqlComand.CommandText = $"delete from { typeof(T).Name  }  where {stringPredicate} ";

                    // execute 
                    sqlComand.ExecuteNonQuery();
                    

                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection.canClose) DbConnection.DbConnectionBase.Close();
            }

          
        }
    }
}