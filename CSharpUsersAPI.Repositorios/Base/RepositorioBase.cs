using CSharpUsersAPI.Entidades.Base;
using CSharpUsersAPI.Utils.Conexao;
using CSharpUsersAPI.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUsersAPI.Repositorios.Base
{
    public class RepositorioBase<TEntidade> : AdoHelper where TEntidade : IEntidade
    {
        public RepositorioBase(TipoConexao conexao) : base(conexao)
        {
        }

        protected TEntidade Obter(String consulta, Object parametros)
        {
            using (var conexao = ObterConexao())
            {
                return conexao.Query<TEntidade>(Formatar(consulta, parametros), parametros).FirstOrDefault();
            }
        }

        protected IEnumerable<TEntidade> Listar(String consulta, Object parametros, bool buferizado = true, int? timeoutComando = null)
        {
            using (var conexao = ObterConexao())
            {
                return conexao.Query<TEntidade>(Formatar(consulta, parametros), parametros, buferizado, timeoutComando);
            }
        }

        protected IEnumerable<T> Listar<T>(String consulta, Object parametros, bool buferizado = true, int? timeoutComando = null)
        {
            using (var conexao = ObterConexao())
            {
                return conexao.Query<T>(Formatar(consulta, parametros), parametros, buferizado, timeoutComando);
            }
        }

        protected T Obter<T>(String consulta, Object parametros)
        {
            using (var conexao = ObterConexao())
            {
                return conexao.Query<T>(Formatar(consulta, parametros), parametros).FirstOrDefault();
            }
        }

        protected IEnumerable<TEntidade> Listar<TFilho>(String consulta, Object parametros, String[] colunasDemarcacao, Func<TEntidade, TFilho, TEntidade> funcaoMapeamento, bool buferizado = true, int? timeoutComando = null)
        {
            return QueryComListaFilho<TEntidade, TFilho>(Formatar(consulta, parametros), parametros, colunasDemarcacao, funcaoMapeamento, buferizado, timeoutComando);
        }

        protected IEnumerable<TPai> QueryComListaFilho<TPai, TFilho>(String consulta, Object parametros, String[] colunasDemarcacao, Func<TPai, TFilho, TPai> funcaoMapeamento, bool buferizado = true, int? timeoutComando = null)
            where TPai : IEntidade
        {
            Dictionary<IChaveEntidade, TPai> pais = new Dictionary<IChaveEntidade, TPai>();
            using (var conexao = ObterConexao())
            {
                conexao.Query<TPai, TFilho>(Formatar(consulta, parametros), (pai, filho) =>
                {
                    var paiNaLista = VerificarEntidade<TPai>(pai, pais);

                    funcaoMapeamento(paiNaLista, filho);

                    return paiNaLista;
                }, parametros, ObterSplitOn(colunasDemarcacao, 1), buferizado);
            }

            return pais.Values;
        }

        private T VerificarEntidade<T>(T entidade, Dictionary<IChaveEntidade, T> mapeamento) where T : IEntidade
        {
            if (!object.Equals(entidade, default(T)) && entidade.ObterChave() != null && entidade.ObterChave().TemValor())
            {
                IChaveEntidade chave = entidade.ObterChave();
                if (!mapeamento.Any(item => item.Key.Equals(chave)))
                {
                    mapeamento[chave] = entidade;
                }

                return mapeamento[chave];
            }

            return entidade;
        }

        private String ObterSplitOn(IEnumerable<String> colunasDemarcacao, int quantidadeTipos)
        {
            String splitOn = null;
            if (colunasDemarcacao != null && colunasDemarcacao.Any())
            {
                if (colunasDemarcacao.Count() == quantidadeTipos
                    && colunasDemarcacao.All(demarcacao => !String.IsNullOrWhiteSpace(demarcacao) && !demarcacao.Contains(",")))
                {
                    splitOn = String.Join(",", colunasDemarcacao);
                }
                else
                {
                    throw new ArgumentException("Colunas de demarcação não foram definidas para todas as entidades, ou alguma delas contém vírgula (',').");
                }
            }

            return splitOn;
        }

        protected string MontaCondicoes(IList<string> condicoes)
        {
            if (condicoes.Count == 0)
                return string.Empty;
            string sql = $" WHERE {condicoes[0]}";
            for (int i = 1; i < condicoes.Count; i++)
            {
                sql = $"{sql} AND {condicoes[i]}";
            }
            return sql;
        }

        protected string MontaCondicoesOu(IList<string> condicoes)
        {
            if (condicoes.Count == 0)
                return string.Empty;
            string sql = $" WHERE {condicoes[0]}";
            for (int i = 1; i < condicoes.Count; i++)
            {
                sql = $"{sql} OR {condicoes[i]}";
            }
            return sql;
        }

        protected string MontaAtualizacoes(IList<string> atualizacoes)
        {
            if (atualizacoes.Count == 0)
                return string.Empty;
            string sql = atualizacoes[0];
            for (int i = 1; i < atualizacoes.Count; i++)
            {
                sql = $"{sql}, {atualizacoes[i]}";
            }
            return sql;
        }

    }
}
