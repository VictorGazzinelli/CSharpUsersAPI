using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CSharpUsersAPI.Utils.Singletons
{
    public class ConnectionStringSettingsSingleton
    {
        private static ConnectionStringSettingsSingleton Singleton;

        public ConnectionStringSettings connectionStringSettings { get; set; }

        protected ConnectionStringSettingsSingleton()
        {

        }

        public void setConnectionStringSettings(string name, string connectionString, string providerName)
        {
            this.connectionStringSettings = new ConnectionStringSettings(name, connectionString, providerName);
        }

        public static ConnectionStringSettingsSingleton Obter()
        {
            if (Singleton == null)
            {
                Singleton = new ConnectionStringSettingsSingleton();
            }
            return Singleton;
        }
    }
}
