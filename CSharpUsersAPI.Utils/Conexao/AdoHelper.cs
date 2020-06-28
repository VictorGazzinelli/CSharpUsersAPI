using CSharpUsersAPI.Utils.AcessoDados;
using CSharpUsersAPI.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.Conexao
{
    public class AdoHelper
    {
        private readonly TipoConexao tipoConexao;
        private static IFormatadorConsulta _formatadorConsulta = new FormatadorConsulta();

        public static void RegistrarFormatadorConsulta(IFormatadorConsulta formatadorConsulta)
        {
            _formatadorConsulta = formatadorConsulta;
        }

        protected static string Formatar(string sql, object parametros)
        {
            return _formatadorConsulta.Formatar(sql, parametros);
        }

        public AdoHelper(TipoConexao tipoConexao)
        {
            this.tipoConexao = tipoConexao;
        }

        public DbAccessHelper ObterConexao()
        {
            return ObterConexao(tipoConexao);
        }

        public static DbAccessHelper ObterConexao(TipoConexao tipoConexao)
        {
            return ConnectionFactory.Instance.GetConnection(Enum.GetName(typeof(TipoConexao), tipoConexao));
        }

        public int Executar(String sql, Object parametros)
        {
            using (DbAccessHelper conexao = ObterConexao())
            {
                return conexao.Execute(Formatar(sql, parametros), parametros);
            }
        }

        public int ExecutarRetornandoId(String sql, Object parametros)
        {
            using (DbAccessHelper conexao = ObterConexao())
            {
                return conexao.ExecuteRetornandoIdentity(Formatar(sql, parametros), parametros);
            }
        }
    }
}
