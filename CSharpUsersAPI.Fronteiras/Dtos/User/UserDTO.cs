using System;
using System.Collections.Generic;
using System.Text;
using UserEntidade = CSharpUsersAPI.Entidades.User.User;

namespace CSharpUsersAPI.Fronteiras.Dtos.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public UserDTO(UserEntidade User)
        {
            if (User == null)
                return;

            this.UserId = User.UserId;
            this.Name = User.Name;
            this.Phone = User.Phone;
            this.Email = User.Email;
            this.Avatar = User.Avatar;
        }
    }
}
