using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.InversaoControle
{
    public class SobrescreverMapeamentoNome : ISobrescreverMapeamento
    {
        // Properties
        public string NomeParametro { get; set; }
        public object Para { get; set; }
    }
}
