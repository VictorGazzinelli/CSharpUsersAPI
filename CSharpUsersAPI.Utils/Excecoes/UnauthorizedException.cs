using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.Excecoes
{
    public class UnauthorizedException : HttpResponseException
    {
        public override string ExceptionName { get; set; } = "UnauthorizedException";
        public override int Status { get; set; } = StatusCodes.Status401Unauthorized;
        public override bool IsMessageUserFriendly { get; set; } = false;

        public UnauthorizedException() : base()
        {

        }

        public UnauthorizedException(string message, bool isMessageUserFriendly = false, Exception innerException = null, params (string Key, object Value)[] extraData) : base(message, innerException)
        {
            this.IsMessageUserFriendly = isMessageUserFriendly;
            if (extraData != null)
            {
                for (int i = 0; i < extraData.Length; i++)
                    this.Data.Add(extraData[i].Key, extraData[i].Value);
            }
        }
    }
}
