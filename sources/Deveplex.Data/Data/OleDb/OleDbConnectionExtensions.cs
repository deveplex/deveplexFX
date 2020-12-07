using System.Collections.Generic;

namespace System.Data.OleDb
{
    public static class OleDbConnectionExtensions
    {
        public static int ExecuteNonQuery(this OleDbConnection connection, string sql, params OleDbParameter[] parameters)
        {
            return ExecuteNonQuery(connection, sql, CommandType.Text, parameters);
        }

        public static int ExecuteNonQuery(this OleDbConnection connection, string sql, CommandType commandType, params OleDbParameter[] parameters)
        {
            return ExecuteNonQuery(connection, sql, commandType, null, parameters);
        }

        public static int ExecuteNonQuery(this OleDbConnection connection, string sql, OleDbTransaction transaction, params OleDbParameter[] parameters)
        {
            return ExecuteNonQuery(connection, sql, CommandType.Text, transaction, parameters);
        }

        public static int ExecuteNonQuery(this OleDbConnection connection, string sql, CommandType commandType, OleDbTransaction transaction, params OleDbParameter[] parameters)
        {
            using (OleDbCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = commandType;
                if (transaction != null)
                    command.Transaction = transaction;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public static Nullable<T> ExecuteScalar<T>(this OleDbConnection connection, string sql, params OleDbParameter[] parameters) where T : struct
        {
            return ExecuteScalar<T>(connection, sql, CommandType.Text, parameters);
        }

        public static Nullable<T> ExecuteScalar<T>(this OleDbConnection connection, string sql, CommandType commandType, params OleDbParameter[] parameters) where T : struct
        {
            return ExecuteScalar<T>(connection, sql, commandType, null, parameters);
        }

        public static Nullable<T> ExecuteScalar<T>(this OleDbConnection connection, string sql, OleDbTransaction transaction, params OleDbParameter[] parameters) where T : struct
        {
            return ExecuteScalar<T>(connection, sql, CommandType.Text, transaction, parameters);
        }

        public static Nullable<T> ExecuteScalar<T>(this OleDbConnection connection, string sql, CommandType commandType, OleDbTransaction transaction, params OleDbParameter[] parameters) where T : struct
        {
            using (OleDbCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = commandType;
                if (transaction != null)
                    command.Transaction = transaction;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                connection.Open();

                //object value = 
                //NullableConverter converter = new NullableConverter(typeof(Nullable<T>));
                //Nullable<T> dateTimevalue = converter.ConvertFromString(value.ToString());
                return (Nullable<T>)command.ExecuteScalar();
            }
        }

        public static OleDbDataReader ExecuteReader(this OleDbConnection connection, string sql, params OleDbParameter[] parameters)
        {
            return ExecuteReader(connection, sql, CommandType.Text, parameters);
        }
        // 摘要:  
        //     执行ExecuteReader方法，对数据进行了读取操作  
        //       
        //  
        // 参数:  
        //   sql:  
        //     要传入的sql语句  
        //   comType:  
        //     选择执行的类型sql语句 或 储存过程  
        //   pms:  
        //   对可变参数的替换操作  
        //  
        //  
        // 返回结果:  
        //     System.Data.SqlClient.SqlDataReader 对象。  
        public static OleDbDataReader ExecuteReader(this OleDbConnection connection, string sql, CommandType commandType, params OleDbParameter[] parameters)
        {
            using (OleDbCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandType = commandType;
                command.CommandText = sql;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                try
                {
                    connection.Open();
                    return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch
                {
                    connection.Close();
                    connection.Dispose();
                    throw;
                }
            }
        }

        public static T ExecuteQuery<T>(this OleDbConnection connection, string sql, params OleDbParameter[] parameters) where T : DataSet, new()
        {
            return ExecuteQuery<T>(connection, sql, CommandType.Text, parameters);
        }

        public static T ExecuteQuery<T>(this OleDbConnection connection, string sql, CommandType commandType, params OleDbParameter[] parameters) where T : DataSet, new()
        {
            return ExecuteQuery<T>(connection, sql, commandType, null, parameters);
        }

        public static T ExecuteQuery<T>(this OleDbConnection connection, string sql, OleDbTransaction transaction, params OleDbParameter[] parameters) where T : DataSet, new()
        {
            return ExecuteQuery<T>(connection, sql, CommandType.Text, transaction, parameters);
        }

        public static T ExecuteQuery<T>(this OleDbConnection connection, string sql, CommandType commandType, OleDbTransaction transaction, params OleDbParameter[] parameters) where T : DataSet, new()
        {
            using (OleDbCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                command.CommandType = commandType;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                connection.Open();

                T datasource = new T();
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                adapter.Fill(datasource);

                return datasource;
            }
        }

        public static OleDbParameter[] AnonymousTypeToOleDbParameter<T>(this OleDbConnection connection, T parameters)
        {
            return AnonymousTypeToOleDbParameter<T>(connection, parameters, false);
        }

        public static OleDbParameter[] AnonymousTypeToOleDbParameter<T>(this OleDbConnection connection, T parameters, bool isReturnValue)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            List<OleDbParameter> oledbParameters = new List<OleDbParameter>();
            if (isReturnValue /*&& commandType == CommandType.StoredProcedure*/)
            {
                OleDbParameter parameter = new OleDbParameter();
                parameter.ParameterName = @"@ReturnValue";
                parameter.Direction = ParameterDirection.ReturnValue;

                oledbParameters.Add(parameter);
            }

            //object value = member.GetType().GetProperties();//.Where(x => x.Name == "ID").First().GetValue(member, null);
            var properties = parameters.GetType().GetProperties(Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance);//.ToList().ForEach(x => Console.WriteLine(x.Name));
            foreach (var prop in properties)
            {
                OleDbParameter parameter = new OleDbParameter();
                parameter.ParameterName = @"@" + prop.Name;
                parameter.Value = prop.GetValue(parameters) ?? DBNull.Value;

                oledbParameters.Add(parameter);
            }
            return oledbParameters.ToArray();
        }
    }
}
