using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CSharpUsersAPI.Utils.Excecoes
{
    public class HttpResponseException : Exception
    {
        public virtual string ExceptionName { get; set; } = "HttpResponseException";
        public virtual int Status { get; set; } = StatusCodes.Status500InternalServerError;
        public virtual bool IsMessageUserFriendly { get; set; } = false;
        public virtual bool IsCustomException { get; set; } = true;

        public JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = false,
            IgnoreNullValues = false,
            IgnoreReadOnlyProperties = false,
            MaxDepth = 64,
            PropertyNameCaseInsensitive = false,
            WriteIndented = false,
        };

        public HttpResponseException() : base()
        {

        }

        public HttpResponseException(string message, Exception innerException = null) : base(message, innerException)
        {

        }

        public HttpResponseException(Exception exception) : base(exception.Message, exception.InnerException)
        {

        }

        public string ToJsonString()
        {
            return JsonSerializer.Serialize(this, JsonSerializerOptions);
        }
    }
}
