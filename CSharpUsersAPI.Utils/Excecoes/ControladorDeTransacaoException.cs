using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUsersAPI.Utils.Excecoes
{
    public class ControladorDeTransacaoException : HttpResponseException
    {
        public override string ExceptionName { get; set; } = "ControladorDeTransacaoException";
        public override int Status { get; set; } = StatusCodes.Status500InternalServerError;
        public override bool IsMessageUserFriendly { get; set; } = false;

        const string message = "O controlador de transação encontrou uma exception, a transação foi abortada.";

        public ControladorDeTransacaoException() : base(message)
        {

        }

        public ControladorDeTransacaoException(Exception innerException) : base(message, innerException)
        {

        }
    }
}
