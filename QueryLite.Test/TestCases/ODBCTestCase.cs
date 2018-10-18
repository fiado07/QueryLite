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

            string inCIF = "";

            builder.Append("SELECT ");
            builder.Append("'' as cif, ");
            builder.Append("'' as nome,  ");
            builder.Append("'' as datanascimento,  ");
            builder.Append("'' as sexo,  ");
            builder.Append("'' as morada1,  ");
            builder.Append("'' as balcao ");
            builder.Append("FROM ''  where  ");
            builder.Append("''='" + inCIF + "' and ''=1 ");

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

            Person person = null;
                        
            string inCIF = "''";

            builder.Append("'' ");
            builder.Append("'' as cif, ");
            builder.Append("'' as nome,  ");
            builder.Append("'' as datanascimento,  ");
            builder.Append("'' as sexo,  ");
            builder.Append("'' as morada1,  ");
            builder.Append("'' as balcao ");
            builder.Append("FROM ''  where  ");
            builder.Append("''='" + inCIF + "' and ''=? ");


            parameters.Add(new Parameter { ParameterKey = "?", ParameterValue = 1 });

            sqlParameters.Sql = builder.ToString();
            sqlParameters.Parameter = parameters;

            person = queryBuilder.ExecuteSqlGetObject<Person>(sqlParameters);


            Console.WriteLine(person.nome);

            Assert.IsTrue(person != null);



        }


    }
}
