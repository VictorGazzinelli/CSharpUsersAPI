using CSharpUsersAPI.Utils.AcessoDados;
using CSharpUsersAPI.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.Conexao
{
    public class ConexaoBanco
    {
        public static DbAccessHelper ObterConexao(TipoConexao conexao)
        {
            return ConnectionFactory.Instance.GetConnection(Enum.GetName(typeof(TipoConexao), conexao));
        }
    }
}
