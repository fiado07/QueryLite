using QueryLite.Enums;
using System.Data;
using System.Data.Odbc;

namespace QueryLite.Test.TestCases
{
    public class ODBCContext : Contracts.IDbConnectionSql
    {


        public bool canClose { get; set; }       

        public IDbConnection DbConnectionBase { get; set; }

        public DbProviderType DbProvider { get; set; }

        public IDbDataAdapter SetAdapter { get; set; }

        public IDbTransaction DbTransaction { get; set; }

        public ODBCContext()
        {

            //DbConnectionBase = new OdbcConnection("Provider=MSDASQL;DSN=; UID=; PWD=");

  
            DbProvider = DbProviderType.ODBCClient;

            SetAdapter = new OdbcDataAdapter();

        }

    }

}
