using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Deveplex.Repositorys
{
    public interface IRepository<TConnection, TTransaction> where TConnection : DbConnection, new() where TTransaction : DbTransaction
    {
        string ConnectionString { get; }

        int ExecuteQuery(string sql, object parameters);
        int ExecuteQuery(string sql, CommandType commandType, object parameters);
        int ExecuteQuery(string sql, TTransaction transaction, object parameters);
        int ExecuteQuery(string sql, CommandType commandType, TTransaction transaction, object parameters);
        Nullable<T> ExecuteScalar<T>(string sql, object parameters) where T : struct;
        Nullable<T> ExecuteScalar<T>(string sql, CommandType commandType, object parameters) where T : struct;
        Nullable<T> ExecuteScalar<T>(string sql, TTransaction transaction, object parameters) where T : struct;
        Nullable<T> ExecuteScalar<T>(string sql, CommandType commandType, TTransaction transaction, object parameters) where T : struct;
        IEnumerable<T> ExecuteQuery<T>(string sql, object parameters) where T : class, new();
        IEnumerable<T> ExecuteQuery<T>(string sql, CommandType commandType, object parameters) where T : class, new();

    }

}