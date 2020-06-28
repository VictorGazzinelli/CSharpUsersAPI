using CSharpUsersAPI.Fronteiras.Repositorios.User;
using CSharpUsersAPI.Repositorios.User;
using CSharpUsersAPI.Utils.InversaoControle;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Mapeador
{
    public class Mapeador
    {
        private static Mapeador Singleton;

        protected Mapeador()
        {

        }

        public static Mapeador Obter()
        {
            if (Singleton == null)
            {
                Singleton = new Mapeador();
            }
            return Singleton;
        }

        public void RegistrarMapeamentos()
        {
            ResolvedorDeDependencias.Instance().CarregarMapeamentos(ListarMapeamentos());
        }

        private Mapeamento[] ListarMapeamentos()
        {
            var listaMapeamentos = new List<Mapeamento>();

            #region Mapeamento de Repositorios
            listaMapeamentos.Add(new Mapeamento(typeof(IUserRepositorio), typeof(UserRepositorio)));
            #endregion

            return listaMapeamentos.ToArray();
        }
    }
}