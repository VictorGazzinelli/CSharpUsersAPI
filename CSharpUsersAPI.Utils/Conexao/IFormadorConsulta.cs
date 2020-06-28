using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.Conexao
{
    public interface IFormatadorConsulta
    {
        string Formatar(string sql, object parametros);
    }
}
