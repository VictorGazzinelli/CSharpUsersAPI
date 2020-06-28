using CSharpUsersAPI.Utils.AcessoDados;
using CSharpUsersAPI.Utils.Excecoes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace CSharpUsersAPI.Utils.Transacao
{
    public class ControladorDeTransacao
    {
        public static T FuncaoComRetorno<T>(Func<T> p_FuncaoARodar)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ConnectionFactory.Instance.Begin();
                try
                {
                    var v_Retorno = p_FuncaoARodar();
                    scope.Complete();

                    return v_Retorno;
                }
                catch (Exception ex)
                {
                    if (!(ex is HttpResponseException))
                        throw new ControladorDeTransacaoException(ex);
                    else
                        throw ex;
                }
                finally
                {
                    ConnectionFactory.Instance.End();
                    ConnectionFactory.Instance.Dispose();
                }
            }
        }


        public static void FuncaoSemRetorno(Action p_FuncaoARodar)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ConnectionFactory.Instance.Begin();
                try
                {
                    p_FuncaoARodar();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    if (!(ex is HttpResponseException))
                        throw new ControladorDeTransacaoException(ex);
                    else
                        throw ex;
                }
                finally
                {
                    ConnectionFactory.Instance.End();
                    ConnectionFactory.Instance.Dispose();
                }
            }
        }

    }
}
