using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.JWT
{
    public class ValidJWTRequirement : IAuthorizationRequirement
    {
        public ValidJWTRequirement()
        {
        }
    }
}
