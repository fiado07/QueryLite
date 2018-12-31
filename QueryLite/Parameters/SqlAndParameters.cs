using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryLite.Parameters
{
    public class SqlAndParameters
    {


        public string Sql { get; set; }

        public bool isStoredProcedure { get; set; } = false;

        public List<Parameter> Parameter { get; set; }

        public SqlAndParameters()
        {

        }

    }

}
