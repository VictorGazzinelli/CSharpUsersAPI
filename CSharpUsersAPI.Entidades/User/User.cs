using CSharpUsersAPI.Entidades.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Entidades.User
{
    public class User : IEntidade
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public IChaveEntidade ObterChave()
        {
            return new ChaveEntidadeString("" + UserId);
        }
    }
}
