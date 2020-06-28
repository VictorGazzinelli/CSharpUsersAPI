using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Entidades.Base
{
    public class ChaveEntidadeString : IChaveEntidade, IEquatable<ChaveEntidadeString>
    {
        private readonly string valor;

        public ChaveEntidadeString(String valor)
        {
            this.valor = valor;
        }

        public bool TemValor()
        {
            return !String.IsNullOrWhiteSpace(valor);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ChaveEntidadeString))
            {
                return false;
            }

            return (obj is ChaveEntidadeString)
                && Equals((ChaveEntidadeString)obj);
        }

        public override int GetHashCode()
        {
            return valor.GetHashCode();
        }

        public bool Equals(ChaveEntidadeString outro)
        {
            if (outro == null)
                return false;
            return (!outro.TemValor() && !this.TemValor()) || (outro.TemValor() && outro.valor.Equals(valor));
        }
    }
}
