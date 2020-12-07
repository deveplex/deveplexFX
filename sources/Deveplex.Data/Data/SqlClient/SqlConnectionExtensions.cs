using System.Collections.Generic;

namespace System.Data.SqlClient
{
    public static class SqlConnectionExtensions
    {
        public static int ExecuteNonQuery(this SqlConnection connection, string sql, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(connection, sql, CommandType.Text, parameters);
        }

        public static int ExecuteNonQuery(this SqlConnection connection, string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(connection, sql, commandType, null, parameters);
        }

        public static int ExecuteNonQuery(this SqlConnection connection, string sql, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(connection, sql, CommandType.Text, transaction, parameters);
        }

        public static int ExecuteNonQuery(this SqlConnection connection, string sql, CommandType commandType, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand())
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

        public static Nullable<T> ExecuteScalar<T>(this SqlConnection connection, string sql, params SqlParameter[] parameters) where T : struct
        {
            return ExecuteScalar<T>(connection, sql, CommandType.Text, parameters);
        }

        public static Nullable<T> ExecuteScalar<T>(this SqlConnection connection, string sql, CommandType commandType, params SqlParameter[] parameters) where T : struct
        {
            return ExecuteScalar<T>(connection, sql, commandType, null, parameters);
        }

        public static Nullable<T> ExecuteScalar<T>(this SqlConnection connection, string sql, SqlTransaction transaction, params SqlParameter[] parameters) where T : struct
        {
            return ExecuteScalar<T>(connection, sql, CommandType.Text, transaction, parameters);
        }

        public static Nullable<T> ExecuteScalar<T>(this SqlConnection connection, string sql, CommandType commandType, SqlTransaction transaction, params SqlParameter[] parameters) where T : struct
        {
            using (SqlCommand command = connection.CreateCommand())
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

        public static SqlDataReader ExecuteReader(this SqlConnection connection, string sql, params SqlParameter[] parameters)
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
        public static SqlDataReader ExecuteReader(this SqlConnection connection, string sql, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlCommand command = connection.CreateCommand())
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

        public static T ExecuteQuery<T>(this SqlConnection connection, string sql, params SqlParameter[] parameters) where T : DataSet, new()
        {
            return ExecuteQuery<T>(connection, sql, CommandType.Text, parameters);
        }

        public static T ExecuteQuery<T>(this SqlConnection connection, string sql, CommandType commandType, params SqlParameter[] parameters) where T : DataSet, new()
        {
            return ExecuteQuery<T>(connection, sql, commandType, null, parameters);
        }

        public static T ExecuteQuery<T>(this SqlConnection connection, string sql, SqlTransaction transaction, params SqlParameter[] parameters) where T : DataSet, new()
        {
            return ExecuteQuery<T>(connection, sql, CommandType.Text, transaction, parameters);
        }

        public static T ExecuteQuery<T>(this SqlConnection connection, string sql, CommandType commandType, SqlTransaction transaction, params SqlParameter[] parameters) where T : DataSet, new()
        {
            using (SqlCommand command = connection.CreateCommand())
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

                T datasource = new T();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(datasource);

                return datasource;
            }
        }

        public static SqlParameter[] AnonymousTypeToSqlParameter<T>(this SqlConnection connection, T parameters) where T : class
        {
            return AnonymousTypeToSqlParameter<T>(connection, parameters, false);
        }

        public static SqlParameter[] AnonymousTypeToSqlParameter<T>(this SqlConnection connection, T parameters, bool isReturnValue) where T : class
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (isReturnValue /*&& commandType == CommandType.StoredProcedure*/)
            {
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = @"@ReturnValue";
                parameter.Direction = ParameterDirection.ReturnValue;

                sqlParameters.Add(parameter);
            }

            //object value = member.GetType().GetProperties();//.Where(x => x.Name == "ID").First().GetValue(member, null);
            var properties = parameters.GetType().GetProperties(Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance);//.ToList().ForEach(x => Console.WriteLine(x.Name));
            foreach (var prop in properties)
            {
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = @"@" + prop.Name;
                parameter.Value = prop.GetValue(parameters) ?? DBNull.Value;

                sqlParameters.Add(parameter);
            }
            return sqlParameters.ToArray();
        }

    }
}
