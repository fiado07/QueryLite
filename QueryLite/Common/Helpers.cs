using QueryLite.Enums;
using QueryLite.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace QueryLite.Common
{
    public static class Helpers
    {

        public static void SetParameters(this IDbCommand command, SqlAndParameters sqlParameter)
        {
                    
            
            // check befor use
            sqlParameter?.Parameter?.ForEach(keyValueParameter =>
            {

                IDbDataParameter parameters = command.CreateParameter();

                
                parameters.ParameterName = keyValueParameter.ParameterKey;
                parameters.Value = keyValueParameter.ParameterValue;
                                
                command.Parameters.Add(parameters);

            });


        }

    }
}
