using NUnit.Framework;
using QueryLite.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryLite.Test.TestCases
{
    [TestFixture]
    public class SQlServerTestCase
    {

        SqlQuery.QueryBuilder queryBuilder;

        Contracts.IDbConnectionSql connectionSql;

        public SQlServerTestCase()
        {

            connectionSql = new SqlServerContext();

            queryBuilder = SqlQuery.QueryBuilder.GetQueryBuilder(connectionSql);

        }


        [Test]
        public void ExecuteSQL()
        {

            string sql = "insert into Aluno(nota,curso) values(@nota,@curso) ";
            List<Parameter> parameters = new List<Parameter>();
            SqlAndParameters sqlParameters = new SqlAndParameters();


            parameters.Add(new Parameter { ParameterKey = "@nota", ParameterValue = 50 });
            parameters.Add(new Parameter { ParameterKey = "@curso", ParameterValue = "PttAP2" });


            sqlParameters.Sql = sql;
            sqlParameters.Parameter = parameters;

            queryBuilder.ExecuteSql(sqlParameters);


            Assert.IsTrue(true);


        }

        [Test]
        public void ExecuteSQL_With_Transaction()
        {

            string sql = "insert into Aluno(nota,curso) values(@nota,@curso) ";
            List<Parameter> parameters = new List<Parameter>();
            SqlAndParameters sqlParameters = new SqlAndParameters();


            connectionSql.DbConnectionBase.Open();
            connectionSql.DbTransaction = connectionSql.DbConnectionBase.BeginTransaction();


            parameters.Add(new Parameter { ParameterKey = "@nota", ParameterValue = 50 });
            parameters.Add(new Parameter { ParameterKey = "@curso", ParameterValue = "PttAP2" });


            sqlParameters.Sql = sql;
            sqlParameters.Parameter = parameters;


            queryBuilder.ExecuteSql(sqlParameters);


            connectionSql.DbTransaction.Rollback();

            Assert.IsTrue(true);


        }


        [Test]
        public void ExecuteSqlGetObject()
        {

            string sql = "select * from Aluno where alunoID=@alunoID";
            List<Parameter> parameters = new List<Parameter>();
            SqlAndParameters sqlParameters = new SqlAndParameters();
            Aluno aluno = new Aluno();

            parameters.Add(new Parameter { ParameterKey = "@alunoID", ParameterValue = 1 });

            sqlParameters.Sql = sql;
            sqlParameters.Parameter = parameters;

            aluno = queryBuilder.ExecuteSqlGetObject<Aluno>(sqlParameters);


            Assert.IsTrue(true);


        }

        [Test]
        public void ExecuteSqlGetObjectList()
        {

            string sql = "select * from Aluno";
            List<Parameter> parameters = new List<Parameter>();
            SqlAndParameters sqlParameters = new SqlAndParameters();
            List<Aluno> alunoLista = new List<Aluno>();



            sqlParameters.Sql = sql;


            alunoLista = queryBuilder.ExecuteSqlGetObjectList<Aluno>(sqlParameters).ToList();


            Console.WriteLine(alunoLista.Count());

            Assert.IsTrue(alunoLista.Count() > 0);



        }

        [Test]
        public void ExecuteSqlGetAny()
        {

            string sql = "select * from Aluno";
            List<Parameter> parameters = new List<Parameter>();
            SqlAndParameters sqlParameters = new SqlAndParameters();
            bool hasAny = false;



            sqlParameters.Sql = sql;


            hasAny = queryBuilder.any(sqlParameters);

            Console.WriteLine(hasAny);

            Assert.IsTrue(hasAny);


        }

        [Test]
        public void ExecuteSqlGetDataTable()
        {

            string sql = "select * from Aluno";
            List<Parameter> parameters = new List<Parameter>();
            SqlAndParameters sqlParameters = new SqlAndParameters();
            System.Data.DataTable table = null;



            sqlParameters.Sql = sql;


            table = queryBuilder.ExecuteSqlGetDataTable(sqlParameters);

            Console.WriteLine(table.Rows.Count);

            Assert.IsTrue(table.Rows.Count > 0);



        }

        [Test]
        public void ExecuteSql_AddObject()
        {
            List<string> exclude = new List<string>() { "alunoID" };
            Aluno aluno = new Aluno() { curso = "curs2", Nome = "fiaTest", data = DateTime.Now, nota = "12" };


            queryBuilder.Add(aluno, exclude);

            Assert.IsTrue(true);


        }

        [Test]
        public void ExecuteSql_UpdateObject()
        {
            List<string> exclude = new List<string>() { "alunoID" };
            Aluno aluno = new Aluno() { curso = "AAA", Nome = "fiaTest", data = DateTime.Now, nota = "40" };

            queryBuilder.Update(aluno, "alunoID=23", exclude);


            Assert.IsTrue(true);

        }


        [Test]
        public void ExecuteSql_GetObject()
        {

            Aluno aluno = null;

            aluno = queryBuilder.Get<Aluno>((x) => x.curso.Contains( "AAA") && x.Nome.EndsWith("BB") && x.Nome.StartsWith ("CC") && x.curso .Contains ("CC"));

            Assert.IsNotNull(aluno);


        }


    }
}
