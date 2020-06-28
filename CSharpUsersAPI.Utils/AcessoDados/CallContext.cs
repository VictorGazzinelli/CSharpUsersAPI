using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CSharpUsersAPI.Utils.AcessoDados
{
    public class CallContext : IDisposable
    {
        // Fields
        [ThreadStatic]
        private static CallContext _instance;
        [ThreadStatic]
        private static int _instanceCounter;

        // Methods
        public void Dispose()
        {
            Release();
        }

        private static void Release()
        {
            if (--_instanceCounter == 0)
            {
                _instance = null;
            }
        }

        // Properties
        public Action<IDbConnection, string, object> HistoryCallback { get; set; }

        public Attribute CallerMetadata { get; set; }

        public static CallContext Instance
        {
            get
            {
                if (_instanceCounter++ == 0)
                {
                    _instance = new CallContext();
                }
                return _instance;
            }
        }
    }
}
