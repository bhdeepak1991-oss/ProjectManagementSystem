using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace PMS.Helpers
{
    public static class SqlHelper
    {
        private const string _connectionString= @"Server=122.176.55.107,1433;Database=ProjectManagementSystem;User Id=sa;Password=vi@pra91;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True";
        public static int ExecuteNonQuery(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = commandType;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand(commandText, conn)
            {
                CommandType = commandType
            };

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            conn.Open();

            // Note: CommandBehavior.CloseConnection ensures connection is closed when reader is closed
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static List<T> MapToList<T>(SqlDataReader reader) where T : new()
        {
            List<T> list = new List<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (reader.Read())
            {
                T obj = new T();

                foreach (var prop in properties)
                {
                    if (!reader.HasColumn(prop.Name) || reader[prop.Name] is DBNull)
                        continue;

                    try
                    {
                        var value = Convert.ChangeType(reader[prop.Name], prop.PropertyType);
                        prop.SetValue(obj, value);
                    }
                    catch
                    {
                        // Ignore property if conversion fails
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        public static bool HasColumn(this IDataRecord reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
