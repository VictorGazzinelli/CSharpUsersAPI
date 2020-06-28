using Dapper;
using CSharpUsersAPI.Fronteiras.Repositorios.User;
using CSharpUsersAPI.Repositorios.Base;
using CSharpUsersAPI.Repositorios.Entidade;
using CSharpUsersAPI.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UserEntidade = CSharpUsersAPI.Entidades.User.User;

namespace CSharpUsersAPI.Repositorios.User
{
    public class UserRepositorio : RepositorioBase<RUser>, IUserRepositorio
    {
        private static readonly string SELECT_WHERE_ID = @"
            SELECT  [userId] AS UserId
                   ,[name] AS Name
                   ,[phone] AS Phone
                   ,[email] AS Email
                   ,[avatar] AS Avatar
             FROM [dbo].[User]
             WHERE userId = @USER_ID
        ";

        private static readonly string SELECT = @"
            SELECT  [userId] AS UserId
                   ,[name] AS Name
                   ,[phone] AS Phone
                   ,[email] AS Email
                   ,[avatar] AS Avatar
             FROM [dbo].[User]
        ";

        public UserRepositorio() : base(TipoConexao.SYS_DATABASE)
        {
        }

        public UserEntidade Obter(int idUser)
        {
            DynamicParameters parametros = new Dapper.DynamicParameters();
            parametros.Add("@USER_ID", idUser, DbType.Int32);

            return Obter(SELECT_WHERE_ID, parametros);
        }

        public IEnumerable<UserEntidade> Listar()
        {
            return Listar(SELECT, null);
        }
    }
}
