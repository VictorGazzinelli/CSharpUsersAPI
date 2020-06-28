using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Entidades.Base
{
    public interface IEntidade
    {
        IChaveEntidade ObterChave();
    }
}
