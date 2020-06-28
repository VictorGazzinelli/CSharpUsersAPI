using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CSharpUsersAPI.Fronteiras.Dtos.User;
using CSharpUsersAPI.Fronteiras.Repositorios.User;
using CSharpUsersAPI.Models.User;
using CSharpUsersAPI.Repositorios.User;
//using CSharpUsersAPI.Utils.Fronteira;
//using CSharpUsersAPI.Utils.InversaoControle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserEntity = CSharpUsersAPI.Entidades.User.User;

namespace CSharpUsersAPI.Controllers
{
    [Produces("application/json")]
    [RequireHttps]
    [ApiController]
    public class UserController : ControllerBase
    {
        //[Authorize(Policy = "JWT")]
        [HttpGet]
        [Route("api/[controller]/GetUser")]
        public GetUserOutput GetUser([Required]int UserId)
        {
            IUserRepositorio userRepositorio = new UserRepositorio();

            UserEntity userEntity = userRepositorio.Obter(UserId);

            return new GetUserOutput()
            {
                User = new UserDTO(userEntity)
            };
        }

        //[Authorize(Policy = "JWT")]
        [HttpGet]
        [Route("api/[controller]/ListUser")]
        public ListUserOutput ListUser()
        {
            IUserRepositorio userRepositorio = new UserRepositorio();

            IEnumerable<UserEntity> enumerableUserEntity = userRepositorio.Listar();

            return new ListUserOutput()
            {
                ArrUser = enumerableUserEntity.Select(ue => new UserDTO(ue)).ToArray()
            };
        }
    }
}