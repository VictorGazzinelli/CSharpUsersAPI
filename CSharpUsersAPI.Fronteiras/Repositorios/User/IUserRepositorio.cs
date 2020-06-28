using System;
using System.Collections.Generic;
using System.Text;
using UserEntidade = CSharpUsersAPI.Entidades.User.User;

namespace CSharpUsersAPI.Fronteiras.Repositorios.User
{

    public interface IUserRepositorio
    {
        UserEntidade Obter(int userId);
        IEnumerable<UserEntidade> Listar();
    }
}
