using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUsersAPI.Utils.InversaoControle
{
    public class ResolvedorDeDependencias
    {
        // Fields
        private static volatile ResolvedorDeDependencias _instance;
        private static volatile object _sync = new object();
        private readonly IUnityContainer _container = new UnityContainer();

        // Methods
        private ResolvedorDeDependencias()
        {
            UnityContainerExtensions.AddNewExtension<Interception>(this._container);
            //this.AdicionarConfiguracaoDeIntercepcao("policy", typeof(DocsTransacaoAttribute), new DocsTransacaoHandler());
        }

        public void AdicionarConfiguracaoDeIntercepcao(string nomePolitica, Type tipoAtributo, ICallHandler instanciaHandler)
        {
            UnityContainerExtensions.Configure<Interception>(this._container).AddPolicy(nomePolitica).AddMatchingRule(new CustomAttributeMatchingRule(tipoAtributo, false)).AddCallHandler(instanciaHandler);
        }

        private static ResolverOverride[] BuildParameters(ISobrescreverMapeamento[] parametros)
        {
            if ((parametros == null) || !parametros.Any<ISobrescreverMapeamento>())
            {
                return null;
            }
            List<ResolverOverride> list1 = new List<ResolverOverride>();
            list1.AddRange(BuildParametersTipo(parametros.OfType<SobrescreverMapeamentoTipo>()));
            list1.AddRange(BuildParametersNome(parametros.OfType<SobrescreverMapeamentoNome>()));
            return list1.ToArray();
        }

        private static IEnumerable<ResolverOverride> BuildParametersNome(IEnumerable<SobrescreverMapeamentoNome> parametros) =>
            ((IEnumerable<ResolverOverride>)(from parametro in parametros select new ParameterOverride(parametro.NomeParametro, parametro.Para)));

        private static IEnumerable<ResolverOverride> BuildParametersTipo(IEnumerable<SobrescreverMapeamentoTipo> parametros) =>
            ((IEnumerable<ResolverOverride>)(from parametro in parametros select new DependencyOverride(parametro.De, parametro.Para)));

        public void CarregarMapeamentos(params Mapeamento[] mapeamentos)
        {
            object obj2 = _sync;
            lock (obj2)
            {
                foreach (Mapeamento mapeamento in mapeamentos)
                {
                    this.RegistrarMapeamento(mapeamento, false);
                }
            }
        }

        public void CarregarMapeamentosSobrescrevendo(params Mapeamento[] mapeamentos)
        {
            object obj2 = _sync;
            lock (obj2)
            {
                foreach (Mapeamento mapeamento in mapeamentos)
                {
                    this.RegistrarMapeamento(mapeamento, true);
                }
            }
        }

        public static ResolvedorDeDependencias Instance()
        {
            if (_instance == null)
            {
                object obj2 = _sync;
                lock (obj2)
                {
                    if (_instance == null)
                    {
                        _instance = new ResolvedorDeDependencias();
                    }
                }
            }
            return _instance;
        }

        public void LimparMapeamentos()
        {
            foreach (ContainerRegistration registration in from r in this._container.Registrations
                                                           where r.LifetimeManager != null
                                                           select r)
            {
                if ((registration.RegisteredType.Namespace == null) || !registration.RegisteredType.Namespace.Contains("Microsoft.Practices.Unity"))
                {
                    registration.LifetimeManager.RemoveValue();
                }
            }
        }

        public T ObterInstanciaDe<T>() =>
            this.ObterInstanciaDe<T>(string.Empty);

        public T ObterInstanciaDe<T>(params ISobrescreverMapeamento[] parametros) =>
            this.ObterInstanciaDe<T>(string.Empty, parametros);

        public T ObterInstanciaDe<T>(string nome) =>
            this.ObterInstanciaDe<T>(nome, null);

        public T ObterInstanciaDe<T>(string nome, params ISobrescreverMapeamento[] parametros)
        {
            T local2;
            try
            {
                T local;
                ResolverOverride[] overrideArray = BuildParameters(parametros);
                if (string.IsNullOrEmpty(nome))
                {
                    local = (overrideArray == null) ? UnityContainerExtensions.Resolve<T>(this._container, new ResolverOverride[0]) : UnityContainerExtensions.Resolve<T>(this._container, overrideArray);
                }
                else
                {
                    local = (overrideArray == null) ? UnityContainerExtensions.Resolve<T>(this._container, nome, new ResolverOverride[0]) : UnityContainerExtensions.Resolve<T>(this._container, nome, overrideArray);
                }
                local2 = local;
            }
            catch (Exception exception)
            {
                throw new Exception($"Ocorreu erro ao tentar obter o tipo {typeof(T).Name}, verifique o injetor.", exception);
            }
            return local2;
        }

        private void RegistrarMapeamento(Mapeamento mapeamento, bool sobreescrever)
        {
            if (string.IsNullOrEmpty(mapeamento.Nome))
            {
                if (!sobreescrever && UnityContainerExtensions.IsRegistered(this._container, mapeamento.De))
                {
                    throw new Exception($"Já existe registro de mapeamento para este tipo ( {mapeamento.De.FullName} ), verifique o tipo ou faça o registro explicitando um nome.");
                }
                InjectionMember[] memberArray1 = new InjectionMember[] { new Interceptor<InterfaceInterceptor>(), new InterceptionBehavior<PolicyInjectionBehavior>() };
                UnityContainerExtensions.RegisterType(this._container, mapeamento.De, mapeamento.Para, memberArray1);
            }
            else
            {
                if (!sobreescrever && UnityContainerExtensions.IsRegistered(this._container, mapeamento.De))
                {
                    throw new Exception($"Já existe registro de mapeamento nomeado para este tipo ( {mapeamento.De.FullName} ), verifique o tipo.");
                }
                InjectionMember[] memberArray2 = new InjectionMember[] { new Interceptor<InterfaceInterceptor>(), new InterceptionBehavior<PolicyInjectionBehavior>() };
                UnityContainerExtensions.RegisterType(this._container, mapeamento.De, mapeamento.Para, mapeamento.Nome, memberArray2);
            }
        }
    }
}
