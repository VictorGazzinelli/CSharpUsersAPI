using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.Conexao
{
    public class FormatadorConsulta : IFormatadorConsulta
    {
        public string Formatar(string sql, object parametros)
        {
            return sql;
        }
    }
}
