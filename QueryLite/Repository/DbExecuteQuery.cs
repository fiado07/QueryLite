using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using QueryLite.Contracts;
using QueryLite.Parameters;
using QueryLite.Common;

namespace QueryLite.Repository
{
    public class DbExecuteQuery : IDbExecute
    {

        IDbConnectionSql DbConnection;

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



            ValidateConnection();


            try
            {


                using (var sqlComand = DbConnection.DbConnectionBase.CreateCommand())
                {



                    if (DbConnection?.DbTransaction != null)
                        sqlComand.Transaction = DbConnection.DbTransaction;



                    if (sqlComand.Parameters != null)
                        sqlComand.SetParameters(sqlParameter);

                    

                    // set sql
                    sqlComand.CommandText = sqlParameter.Sql;


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
    }
}
