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
          

    }
}
