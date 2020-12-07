using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace Deveplex.Repositorys.OleDb
{
    public abstract class OleDbRepository : IRepository<OleDbConnection, OleDbTransaction>
    {
        public abstract string ConnectionString { get; }

        public int ExecuteQuery(string sql, object parameters)
        {
            return ExecuteQuery(sql, CommandType.Text, parameters);
        }

        public int ExecuteQuery(string sql, CommandType commandType, object parameters)
        {
            return ExecuteQuery(sql, commandType, null, parameters);
        }

        public int ExecuteQuery(string sql, OleDbTransaction transaction, object parameters)
        {
            return ExecuteQuery(sql, CommandType.Text, transaction, parameters);
        }

        public int ExecuteQuery(string sql, CommandType commandType, OleDbTransaction transaction, object parameters)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbParameter[] oledbParameters = connection.AnonymousTypeToOleDbParameter(parameters);

                return connection.ExecuteNonQuery(sql, commandType, transaction, oledbParameters);
            }
        }

        public Nullable<T> ExecuteScalar<T>(string sql, object parameters) where T : struct
        {
            return ExecuteScalar<T>(sql, CommandType.Text, parameters);
        }

        public Nullable<T> ExecuteScalar<T>(string sql, CommandType commandType, object parameters) where T : struct
        {
            return ExecuteScalar<T>(sql, commandType, null, parameters);
        }

        public Nullable<T> ExecuteScalar<T>(string sql, OleDbTransaction transaction, object parameters) where T : struct
        {
            return ExecuteScalar<T>(sql, CommandType.Text, transaction, parameters);
        }

        public Nullable<T> ExecuteScalar<T>(string sql, CommandType commandType, OleDbTransaction transaction, object parameters) where T : struct
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbParameter[] oledbParameters = connection.AnonymousTypeToOleDbParameter(parameters);

                return connection.ExecuteScalar<T>(sql, commandType, transaction, oledbParameters);
            }
        }

        public IEnumerable<T> ExecuteQuery<T>(string sql, object parameters) where T : class, new()
        {
            return ExecuteQuery<T>(sql, CommandType.Text, parameters);
        }
        public IEnumerable<T> ExecuteQuery<T>(string sql, CommandType commandType, object parameters) where T : class, new()
        {
            IEnumerable<T> result = new List<T>();

            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbParameter[] oledbParameters = connection.AnonymousTypeToOleDbParameter(parameters);
                DataSet dataset = connection.ExecuteQuery<DataSet>(sql, commandType, oledbParameters);
                if (dataset == null || dataset.Tables.Count <= 0)
                    return result;

                return dataset.Tables[0].Rows.Cast<T>();
            }
        }

        //public IEnumerable<T> Execute<T, TReturn>(string sql, object parameters, out Nullable<TReturn> returnValue) where TReturn : struct
        //{
        //    IEnumerable<T> result = new List<T>();

        //    List<SqlParameter> sqlParameters = new List<SqlParameter>();
        //    //if (commandType == CommandType.StoredProcedure)
        //    {
        //        SqlParameter parameter = new SqlParameter();
        //        parameter.ParameterName = @"@ReturnValue";
        //        parameter.Direction = ParameterDirection.ReturnValue;

        //        sqlParameters.Add(parameter);
        //    }

        //    //object value = member.GetType().GetProperties();//.Where(x => x.Name == "ID").First().GetValue(member, null);
        //    var properties = parameters.GetType().GetProperties();//.ToList().ForEach(x => Console.WriteLine(x.Name));
        //    foreach (var prop in properties)
        //    {
        //        SqlParameter parameter = new SqlParameter();
        //        parameter.ParameterName = @"@" + prop.Name;
        //        parameter.Value = prop.GetValue(parameters) ?? DBNull.Value;

        //        sqlParameters.Add(parameter);
        //    }
        //    SqlParameter[] fff = sqlParameters.ToArray();
        //    DataSet dataset = Connection.ExecuteQuery(sql, CommandType.StoredProcedure, fff);
        //    var p = fff.FirstOrDefault(x => x.Direction == ParameterDirection.ReturnValue);
        //    returnValue = p == null ? null : (Nullable<TReturn>)(p.Value);

        //    if (dataset == null || dataset.Tables.Count <= 0)
        //        return result;

        //    return dataset.Tables[0].Rows.Cast<T>();
        //}

        //public T Execute<T>(string sql, IDbTransaction transaction, object parameters)
        //{
        //    return default(T);
        //}
    }
}
