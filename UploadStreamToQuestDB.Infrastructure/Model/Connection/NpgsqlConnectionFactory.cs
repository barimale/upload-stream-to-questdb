using Npgsql;
using System.Data.Common;

namespace UploadStreamToQuestDB.Infrastructure.Model.Connection {
    /// <summary>
    /// Default implementation of IDbConnectionFactory for PostgreSQL/QuestDB connections.
    /// </summary>
    public class NpgsqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _database;

        /// <summary>
        /// Initializes a new instance of the NpgsqlConnectionFactory class.
        /// </summary>
        public NpgsqlConnectionFactory(
            string host,
            int port,
            string username,
            string password,
            string database)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            _database = database;
        }

        /// <summary>
        /// Creates and opens a PostgreSQL/QuestDB connection.
        /// </summary>
        public async Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
        {
            var connectionString = $"Host={_host};Port={_port};Username={_username};Password={_password};Database={_database};";
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            return connection;
        }
    }
}
