using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.Fronteira
{
    public interface IExecutor<TRequisicao, TResultado>
    {
        // Methods
        TResultado Executar(TRequisicao requisicao);
    }

    public interface IExecutorSemRequisicao<TResultado>
    {
        // Methods
        TResultado Executar();
    }

    public interface IExecutorSemResultado<TRequisicao>
    {
        // Methods
        void Executar(TRequisicao requisicao);
    }

    public interface IRequisicao<TInformacoesLog>
    {
        // Properties
        TInformacoesLog InformacoesLog { get; set; }
    }
}
