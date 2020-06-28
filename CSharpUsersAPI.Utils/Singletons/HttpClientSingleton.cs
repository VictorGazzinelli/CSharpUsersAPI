using CSharpUsersAPI.Utils.Fronteira;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSharpUsersAPI.Utils.Singletons
{
    public class HttpClientSingleton : DelegatingHandler
    {
        private static HttpClientSingleton Singleton;
        public HttpClient HttpClient;

        protected HttpClientSingleton()
        {

        }

        public static HttpClientSingleton Obter()
        {
            if (Singleton == null)
            {
                Singleton = new HttpClientSingleton();
            }
            return Singleton;
        }

        public void StartHttpClient()
        {
            HttpClientHandler hch = new HttpClientHandler();
            hch.Proxy = null;
            hch.UseProxy = false;

            this.HttpClient = new HttpClient(hch);
            //this.HttpClient.Timeout = new TimeSpan(0, 3, 0);

        }
    }
}
