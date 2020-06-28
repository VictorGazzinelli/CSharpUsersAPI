using CSharpUsersAPI.Fronteiras.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpUsersAPI.Models.User
{
    public class ListUserOutput
    {
        public UserDTO[] ArrUser { get; set; }
    }
}
