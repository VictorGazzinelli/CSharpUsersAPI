using Dapper;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSharpUsersAPI.Utils.AcessoDados
{
    public class DbAccessHelper : IDisposable
    {
        // Fields
        protected readonly string _nomeConexao;

        // Methods
        public DbAccessHelper(string nomeConexao)
        {
            this._nomeConexao = nomeConexao;
        }

        public void Dispose()
        {
        }

        public int Execute(string sql, object parametro) =>
            this.Execute(sql, parametro, null, null, null);

        public int Execute(string sql, object parametro, CommandType tipoComando) =>
            this.Execute(sql, parametro, null, null, new CommandType?(tipoComando));

        public int Execute(string sql, object parametro, IDbTransaction transaction, int? commandTimeout, CommandType? commandType)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                using (CallContext context = CallContext.Instance)
                {
                    if (context.CallerMetadata != null)
                    {
                        context.HistoryCallback(connection, sql, parametro);
                    }
                }
                return SqlMapper.Execute(connection, sql, parametro, transaction, commandTimeout, commandType);
            }
        }

        public int ExecuteIgnorandoIdentity(string sql, object parametro, string nomeTabela)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int? nullable = null;
                CommandType? nullable2 = null;
                SqlMapper.Execute(connection, $"SET IDENTITY_INSERT {nomeTabela} ON", null, null, nullable, nullable2);
                nullable = null;
                nullable2 = null;
                SqlMapper.Execute(connection, $"SET IDENTITY_INSERT {nomeTabela} OFF", null, null, null, null);
                return SqlMapper.Execute(connection, sql, parametro, null, nullable, nullable2);
            }
        }

        public int ExecuteRetornandoIdentity(string sql, object parametro)
        {
            try
            {
                using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
                {
                    using (CallContext context = CallContext.Instance)
                    {
                        if (context.CallerMetadata != null)
                        {
                            context.HistoryCallback(connection, sql, parametro);
                        }
                    }
                    sql = sql + " \nSELECT @@IDENTITY AS int";
                    return SqlMapper.ExecuteScalar<int>(connection, sql, parametro, null, null, null);
                }
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        public string MontarJoins(List<string> sqlJoins)
        {
            if (sqlJoins.Count > 0)
            {
                StringBuilder builder1 = new StringBuilder();
                builder1.Append(string.Join(" ", sqlJoins));
                return builder1.ToString();
            }
            return "";
        }

        public string MontarWhereSQLCondicional(List<string> sqlParametros)
        {
            if (sqlParametros.Count > 0)
            {
                StringBuilder builder1 = new StringBuilder();
                builder1.Append(" WHERE ");
                builder1.Append(string.Join(" AND ", sqlParametros));
                return builder1.ToString();
            }
            return "";
        }

        public IEnumerable<T> Query<T>(string sql)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<T>(connection, sql, null, null, true, null, null);
            }
        }

        public IEnumerable<IDictionary<string, object>> Query(string sql, object param)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<IDictionary<string, object>>(connection, sql, param, null, true, null, null);
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<T>(connection, sql, param, null, true, null, null);
            }
        }

        public IEnumerable<IDictionary<string, object>> Query(string sql, object param, IDbTransaction transaction)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<IDictionary<string, object>>(connection, sql, param, transaction, true, null, null);
            }
        }

        public IEnumerable<IDictionary<string, object>> Query(string sql, object param, CommandType? commandType)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<IDictionary<string, object>>(connection, sql, param, null, true, null, commandType);
            }
        }

        public IEnumerable<IDictionary<string, object>> Query(string sql, object param, IDbTransaction transaction, CommandType? commandType)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<IDictionary<string, object>>(connection, sql, param, transaction, true, null, commandType);
            }
        }

        public IEnumerable<T> Query<T>(string sql, object parametro, bool bufferizado = true, int? timeoutComando = new int?(), CommandType? tipoComando = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<T>(connection, sql, parametro, null, bufferizado, timeoutComando, tipoComando);
            }
        }

        public IEnumerable<IDictionary<string, object>> Query(string sql, object param, IDbTransaction transaction, bool buffered, int? commandTimeout, CommandType? commandType)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<IDictionary<string, object>>(connection, sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        public IEnumerable<T> Query<T>(string sql, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                if (Classe06<T>.Classep1 == null)
                {
                    Classe06<T>.Classep1 = CallSite<Func<CallSite, object, IEnumerable<T>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable<T>), typeof(DbAccessHelper)));
                }
                if (Classe06<T>.Classep0 == null)
                {
                    Type[] typeArguments = new Type[] { typeof(T) };
                    CSharpArgumentInfo[] argumentInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.IsStaticType | CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null), CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null) };
                    Classe06<T>.Classep0 = CallSite<Func<CallSite, Type, IDbConnection, string, object, IDbTransaction, bool, int?, CommandType?, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Query", typeArguments, typeof(DbAccessHelper), argumentInfo));
                }
                return Classe06<T>.Classep1.Target(Classe06<T>.Classep1, Classe06<T>.Classep0.Target(Classe06<T>.Classep0, typeof(SqlMapper), connection, sql, param, transaction, buffered, commandTimeout, commandType));
            }
        }

        public IEnumerable<TRetorno> Query<TRetorno, TSegundo>(string sql, Func<TRetorno, TSegundo, TRetorno> funcaoMapeamento, object parametro, string colunaDemarcacao = "Id", bool bufferizado = true, int? timeoutComando = new int?(), CommandType? tipoComando = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TRetorno, TSegundo, TRetorno>(connection, sql, funcaoMapeamento, parametro, null, bufferizado, colunaDemarcacao, timeoutComando, tipoComando);
            }
        }

        public IEnumerable<TRetorno> Query<TRetorno, TSegundo, Terceiro>(string sql, Func<TRetorno, TSegundo, Terceiro, TRetorno> funcaoMapeamento, object parametro, string colunaDemarcacao = "Id", bool bufferizado = true, int? timeoutComando = new int?(), CommandType? tipoComando = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TRetorno, TSegundo, Terceiro, TRetorno>(connection, sql, funcaoMapeamento, parametro, null, bufferizado, colunaDemarcacao, timeoutComando, tipoComando);
            }
        }

        public IEnumerable<TRetorno> Query<TRetorno, TSegundo, Terceiro, TQuarto>(string sql, Func<TRetorno, TSegundo, Terceiro, TQuarto, TRetorno> funcaoMapeamento, object parametro, string colunaDemarcacao = "Id", bool bufferizado = true, int? timeoutComando = new int?(), CommandType? tipoComando = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TRetorno, TSegundo, Terceiro, TQuarto, TRetorno>(connection, sql, funcaoMapeamento, parametro, null, bufferizado, colunaDemarcacao, timeoutComando, tipoComando);
            }
        }

        public IEnumerable<TRetorno> Query<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto>(string sql, Func<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TRetorno> funcaoMapeamento, object parametro, string colunaDemarcacao = "Id", bool bufferizado = true, int? timeoutComando = new int?(), CommandType? tipoComando = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TRetorno>(connection, sql, funcaoMapeamento, parametro, null, bufferizado, colunaDemarcacao, timeoutComando, tipoComando);
            }
        }

        public IEnumerable<TRetorno> Query<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TSexto>(string sql, Func<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TSexto, TRetorno> funcaoMapeamento, object parametro, string colunaDemarcacao = "Id", bool bufferizado = true, int? timeoutComando = new int?(), CommandType? tipoComando = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TSexto, TRetorno>(connection, sql, funcaoMapeamento, parametro, null, bufferizado, colunaDemarcacao, timeoutComando, tipoComando);
            }
        }

        public IEnumerable<TRetorno> Query<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TSexto, TSetimo>(string sql, Func<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TSexto, TSetimo, TRetorno> funcaoMapeamento, object parametro, string colunaDemarcacao = "Id", bool bufferizado = true, int? timeoutComando = new int?(), CommandType? tipoComando = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TRetorno, TSegundo, Terceiro, TQuarto, TQuinto, TSexto, TSetimo, TRetorno>(connection, sql, funcaoMapeamento, parametro, null, bufferizado, colunaDemarcacao, timeoutComando, tipoComando);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TFirst, TSecond, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TFirst, TSecond, TThird, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = new int?(), CommandType? commandType = new CommandType?())
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(connection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
            }
        }

        public IEnumerable<TReturn> Query2<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, string splitOn)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.Query<TFirst, TSecond, TReturn>(connection, sql, map, null, null, true, splitOn, null, null);
            }
        }

        public ICollection<TReturn> QueryListWith2Object<TReturn, TReturnKey, TChildObjectOne, TChildObjectTwo>(string pQuery, object pParameters, Func<TReturn, TReturnKey> parentKeySelector, Func<TReturn, TChildObjectOne, TChildObjectOne> childObjectOneSelector, Func<TReturn, TChildObjectTwo, TChildObjectTwo> childObjectTwoSelector, string pSplitOn)
        {
            using (SqlMapper.GridReader reader = this.QueryMultiple(pQuery, pParameters))
            {
                Dictionary<TReturnKey, TReturn> cacheParent = new Dictionary<TReturnKey, TReturn>();
                reader.Read<TReturn, TChildObjectOne, TChildObjectTwo, TReturn>(delegate (TReturn parent, TChildObjectOne childObjectOne, TChildObjectTwo childObjectTwo) {
                    if (!cacheParent.ContainsKey(parentKeySelector(parent)))
                    {
                        cacheParent.Add(parentKeySelector(parent), parent);
                    }
                    childObjectOneSelector(parent, childObjectOne);
                    childObjectTwoSelector(parent, childObjectTwo);
                    return cacheParent[parentKeySelector(parent)];
                }, pSplitOn, true);
                return (ICollection<TReturn>)cacheParent.Values;
            }
        }

        public ICollection<TReturn> QueryListWith3Object<TReturn, TReturnKey, TChildObjectOne, TChildObjectTwo, TChildObjectThree>(string pQuery, object pParameters, Func<TReturn, TReturnKey> parentKeySelector, Func<TReturn, TChildObjectOne, TChildObjectOne> childObjectOneSelector, Func<TReturn, TChildObjectTwo, TChildObjectTwo> childObjectTwoSelector, Func<TReturn, TChildObjectThree, TChildObjectThree> childObjectThreeSelector, string pSplitOn)
        {
            using (SqlMapper.GridReader reader = this.QueryMultiple(pQuery, pParameters))
            {
                Dictionary<TReturnKey, TReturn> cacheParent = new Dictionary<TReturnKey, TReturn>();
                reader.Read<TReturn, TChildObjectOne, TChildObjectTwo, TChildObjectThree, TReturn>(delegate (TReturn parent, TChildObjectOne childObjectOne, TChildObjectTwo childObjectTwo, TChildObjectThree childObjectThree) {
                    if (!cacheParent.ContainsKey(parentKeySelector(parent)))
                    {
                        cacheParent.Add(parentKeySelector(parent), parent);
                    }
                    childObjectOneSelector(parent, childObjectOne);
                    childObjectTwoSelector(parent, childObjectTwo);
                    childObjectThreeSelector(parent, childObjectThree);
                    return cacheParent[parentKeySelector(parent)];
                }, pSplitOn, true);
                return (ICollection<TReturn>)cacheParent.Values;
            }
        }

        public ICollection<TReturn> QueryListWithinList<TReturn, TReturnKey, TChild>(string pQuery, object pParameters, Func<TReturn, ICollection<TChild>> childSelector, Func<TReturn, TReturnKey> parentKeySelector, string pSplitOn)
        {
            using (SqlMapper.GridReader reader = this.QueryMultiple(pQuery, pParameters))
            {
                Dictionary<TReturnKey, TReturn> cache = new Dictionary<TReturnKey, TReturn>();
                reader.Read<TReturn, TChild, TReturn>(delegate (TReturn parent, TChild child) {
                    if (!cache.ContainsKey(parentKeySelector(parent)))
                    {
                        cache.Add(parentKeySelector(parent), parent);
                    }
                    TReturn arg = cache[parentKeySelector(parent)];
                    childSelector(arg).Add(child);
                    return arg;
                }, pSplitOn, true);
                return (ICollection<TReturn>)cache.Values;
            }
        }

        public ICollection<TReturn> QueryListWithinListAndObject<TReturn, TReturnKey, TFirstChild, TChildObject>(string pQuery, object pParameters, Func<TReturn, ICollection<TFirstChild>> childSelector, Func<TReturn, TReturnKey> parentKeySelector, Func<TReturn, TChildObject, TChildObject> childObjectSelector, string pSplitOn)
        {
            using (SqlMapper.GridReader reader = this.QueryMultiple(pQuery, pParameters))
            {
                Dictionary<TReturnKey, TReturn> cacheParent = new Dictionary<TReturnKey, TReturn>();
                reader.Read<TReturn, TFirstChild, TChildObject, TReturn>(delegate (TReturn parent, TFirstChild child, TChildObject childObject) {
                    if (!cacheParent.ContainsKey(parentKeySelector(parent)))
                    {
                        cacheParent.Add(parentKeySelector(parent), parent);
                    }
                    TReturn arg = cacheParent[parentKeySelector(parent)];
                    childObjectSelector(parent, childObject);
                    childSelector(arg).Add(child);
                    return arg;
                }, pSplitOn, true);
                return (ICollection<TReturn>)cacheParent.Values;
            }
        }

        public TReturn QueryListWithinObject<TReturn, TChild>(string pQuery, object pParameters, Func<TReturn, ICollection<TChild>> childSelector)
        {
            using (SqlMapper.GridReader reader = this.QueryMultiple(pQuery, pParameters))
            {
                TReturn arg = reader.Read<TReturn>(true).Single<TReturn>();
                ICollection<TChild> vChildList = childSelector(arg);
                reader.Read<TChild>(true).ToList<TChild>().ForEach(delegate (TChild _item) {
                    vChildList.Add(_item);
                });
                return arg;
            }
        }

        public ICollection<TReturn> QueryListWithObject<TReturn, TReturnKey, TChildObject>(string pQuery, object pParameters, Func<TReturn, TReturnKey> parentKeySelector, Func<TReturn, TChildObject, TChildObject> childObjectSelector, string pSplitOn)
        {
            using (SqlMapper.GridReader reader = this.QueryMultiple(pQuery, pParameters))
            {
                Dictionary<TReturnKey, TReturn> cacheParent = new Dictionary<TReturnKey, TReturn>();
                reader.Read<TReturn, TChildObject, TReturn>(delegate (TReturn parent, TChildObject childObject) {
                    if (!cacheParent.ContainsKey(parentKeySelector(parent)))
                    {
                        cacheParent.Add(parentKeySelector(parent), parent);
                    }
                    childObjectSelector(parent, childObject);
                    return cacheParent[parentKeySelector(parent)];
                }, pSplitOn, true);
                return (ICollection<TReturn>)cacheParent.Values;
            }
        }

        public ICollection<TReturn> QueryListWithObjectWithinList<TReturn, TReturnKey, TFirstChild, TChildObject>(string pQuery, object pParameters, Func<TReturn, ICollection<TFirstChild>> childSelector, Func<TReturn, TReturnKey> parentKeySelector, Func<TFirstChild, TChildObject, TChildObject> childObjectSelector, string pSplitOn)
        {
            using (SqlMapper.GridReader reader = this.QueryMultiple(pQuery, pParameters))
            {
                Dictionary<TReturnKey, TReturn> cacheParent = new Dictionary<TReturnKey, TReturn>();
                reader.Read<TReturn, TFirstChild, TChildObject, TReturn>(delegate (TReturn parent, TFirstChild child, TChildObject childObject) {
                    if (!cacheParent.ContainsKey(parentKeySelector(parent)))
                    {
                        cacheParent.Add(parentKeySelector(parent), parent);
                    }
                    TReturn arg = cacheParent[parentKeySelector(parent)];
                    childObjectSelector(child, childObject);
                    childSelector(arg).Add(child);
                    return arg;
                }, pSplitOn, true);
                return (ICollection<TReturn>)cacheParent.Values;
            }
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.QueryMultiple(connection, sql, param, null, null, null);
            }
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param, CommandType commandType)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.QueryMultiple(connection, sql, param, null, null, new CommandType?(commandType));
            }
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param, IDbTransaction transaction)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.QueryMultiple(connection, sql, param, transaction, null, null);
            }
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param, IDbTransaction transaction, CommandType commandType)
        {
            using (IDbConnection connection = ConnectionFactory.Instance.GetIDbConnection(this._nomeConexao))
            {
                return SqlMapper.QueryMultiple(connection, sql, param, transaction, null, new CommandType?(commandType));
            }
        }

        // Nested Types
        [CompilerGenerated]
        private static class Classe06<T>
        {
            // Fields
            public static CallSite<Func<CallSite, Type, IDbConnection, string, object, IDbTransaction, bool, int?, CommandType?, object>> Classep0;
            public static CallSite<Func<CallSite, object, IEnumerable<T>>> Classep1;
        }
    }
}
