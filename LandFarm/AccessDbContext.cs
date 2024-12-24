using System.Data;
using System.Data.OleDb;

namespace LandFarm
{
    public class AccessDbContext
    {
        private readonly string _connectionString;

        public AccessDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable ExecuteQuery(string query, OleDbParameter[] parameters = null)
        {
            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OleDbCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    using (var adapter = new OleDbDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        public void ExecuteNonQuery(string query, OleDbParameter[] parameters = null)
        {
            using (var connection = new OleDbConnection(_connectionString))
            {
                connection.Open();
                using (var command = new OleDbCommand(query, connection))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
