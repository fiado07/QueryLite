using System.Data.SqlClient;
using System.Data;
using QueryLite.Enums;
using System;

namespace QueryLite.Test.TestCases
{
    public class SqlServerContext : Contracts.IDbConnectionSql
    {

        private string SqlConnectionString = @"Persist Security Info=False;Integrated Security=true;Initial Catalog=escola;server=.\sqlexpress";

        public bool canClose { get; set; } = true;


        public IDbConnection DbConnectionBase { get; set; }

        public DbProviderType DbProvider { get; set; }

        public IDbDataAdapter SetAdapter { get; set; }

        public IDbTransaction DbTransaction { get; set; }

        public SqlServerContext()
        {

            DbConnectionBase = new SqlConnection(SqlConnectionString);                     

            DbProvider = DbProviderType.SqlClient;

            SetAdapter = new SqlDataAdapter();

        }

    }

}
