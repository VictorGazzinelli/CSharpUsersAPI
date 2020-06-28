using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.InversaoControle
{
    public class Mapeamento
    {
        // Methods
        public Mapeamento(Type de, Type para) : this(string.Empty, de, para)
        {
        }

        public Mapeamento(string nome, Type de, Type para)
        {
            this.Nome = nome;
            this.De = de;
            this.Para = para;
        }

        public override string ToString() =>
            $"{this.De.Name}-{this.Para.Name}";

        // Properties
        public string Nome { get; private set; }

        public Type De { get; private set; }

        public Type Para { get; private set; }
    }
}
