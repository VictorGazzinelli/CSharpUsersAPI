using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.InversaoControle
{
    public class SobrescreverMapeamentoTipo : ISobrescreverMapeamento
    {
        // Properties
        public Type De { get; set; }

        public object Para { get; set; }
    }
}
