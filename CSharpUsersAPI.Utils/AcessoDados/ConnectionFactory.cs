using CSharpUsersAPI.Utils.Singletons;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace CSharpUsersAPI.Utils.AcessoDados
{
    public class ConnectionFactory : IDisposable
    {
        // Fields
        private IDbConnection _connectionSybase = null;
        [ThreadStatic]
        private static volatile ConnectionFactory _instance;
        private static readonly object SYNC_ROOT = new object();

        // Methods
        private ConnectionFactory()
        {
        }

        public void Begin()
        {
            this.InTransaction = true;
        }

        private static IDbConnection CreateConnection(ConnectionStringSettings configuration)
        {
            // In .NET Framework, the providers are automatically available via machine.config
            // In .NET Core, there is no GAC or global configuration anymore. This means that you'll have to register your provider in your project first
            // A proxima linha nao estava aqui, e' necessario cadastrar o provider System.Data.SqlClient antes de utiliza-lo.
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            DbConnection connection1 = DbProviderFactories.GetFactory(configuration.ProviderName).CreateConnection();
            if (connection1 == null)
            {
                throw new Exception($"Não foi possível criar uma conexão para a connectionstring ( {configuration.Name} )");
            }
            connection1.ConnectionString = configuration.ConnectionString;
            return connection1;
        }

        public void Dispose()
        {
            if (this._connectionSybase != null)
            {
                if (this._connectionSybase.State != ConnectionState.Closed)
                {
                    this._connectionSybase.Close();
                }
                this._connectionSybase.Dispose();
                this._connectionSybase = null;
            }
        }

        public void End()
        {
            this.InTransaction = false;
        }

        public DbAccessHelper GetConnection(string name) =>
            new DbAccessHelper(name);

        private IDbConnection GetConnectionSybase(ConnectionStringSettings configuration)
        {
            object obj2 = SYNC_ROOT;
            lock (obj2)
            {
                IDbConnection connection;
                if (this._connectionSybase == null)
                {
                    connection = CreateConnection(configuration);
                    if (this.InTransaction)
                    {
                        this._connectionSybase = connection;
                        this._connectionSybase.Open();
                    }
                    return connection;
                }
                connection = this._connectionSybase;
                if ((connection == null) || (connection.State != ConnectionState.Closed))
                {
                    return connection;
                }
                connection.Dispose();
                return CreateConnection(configuration);
            }
        }

        internal IDbConnection GetIDbConnection(string name)
        {
            //ConnectionStringSettings configuration = ConfigurationManager.ConnectionStrings[name];//.get_Item(name); -> Maneira Antiga de se Obter a Connection String em .net Framework
            ConnectionStringSettings configuration = ConnectionStringSettingsSingleton.Obter().connectionStringSettings;
            if (configuration == null)
            {
                throw new Exception($"Não foi possível criar uma conexão para a connectionstring ( {name} )");
            }
            if (GetKey(configuration) != "SYBASE")
            {
                return CreateConnection(configuration);
            }
            return this.GetConnectionSybase(configuration);
        }

        private static string GetKey(ConnectionStringSettings configuration)
        {
            if (!configuration.ProviderName.ToUpper().Contains("SYBASE"))
            {
                return configuration.Name;
            }
            return "SYBASE";
        }

        // Properties
        private bool InTransaction { get; set; }

        public static ConnectionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    object obj2 = SYNC_ROOT;
                    lock (obj2)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConnectionFactory();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
