using NUnit.Framework;
using QueryLite.Parameters;
using QueryLite.Test.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryLite.Test.TestCases
{

    [TestFixture]
    public class ODBCTestCase
    {

        SqlQuery.QueryBuilder queryBuilder;

        public ODBCTestCase()
        {

            queryBuilder = SqlQuery.QueryBuilder.GetQueryBuilder(new ODBCContext());

        }

        [Test]
        public void ODBC_ExecuteSqlGetDataTable()
        {


            StringBuilder builder = new StringBuilder();
            SqlAndParameters sqlParameters = new SqlAndParameters();
            System.Data.DataTable table = null;


            builder.Append("Select * from c02 where CFB='' and CF=1 ");

            sqlParameters.Sql = builder.ToString();

            table = queryBuilder.ExecuteSqlGetDataTable(sqlParameters);

            Assert.IsTrue(table.Rows.Count > 0);


        }

        [Test]
        public void ODBC_ExecuteSqlGetObject()
        {

            StringBuilder builder = new StringBuilder();
            SqlAndParameters sqlParameters = new SqlAndParameters();
            List<Parameter> parameters = new List<Parameter>();

            cfp102 db2object = null;

            builder.Append("Select * from c102 where CCH=? and CFB=1");

            parameters.Add(new Parameter { ParameterKey = "?", ParameterValue = "121" });

            sqlParameters.Sql = builder.ToString();
            sqlParameters.Parameter = parameters;

            db2object = queryBuilder.ExecuteSqlGetObject<cfp102>(sqlParameters);

            Assert.IsNotNull(db2object);


        }


        [Test]
        public void ODBC_GetObject()
        {

            cfp102 db2object = null;

            db2object = queryBuilder.Get<cfp102>((x) => x.CF2 == "121");

            Assert.IsNotNull(db2object);


        }

        [Test]
        public void ODBC_GetObjectList()
        {

            IEnumerable<cfp102> db2object = null;

            db2object = queryBuilder.GetList<cfp102>((x) => x.CF2 .Contains("M"));

            Assert.IsNotNull(db2object);


        }

        [Test]
        public void ODBC_GetObjectAny()
        {

            bool db2object = false;

            db2object = queryBuilder.any<cfp102>((x) => x.CFB1.Contains("M"));

            Assert.IsNotNull(db2object);


        }


    }
}
