using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace UploadStreamToQuestDB.Infrastructure.Model.Connection {
    /// <summary>
    /// Factory interface for creating database connections.
    /// Enables dependency injection and testability.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Creates and opens a database connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>An opened database connection ready for use.</returns>
        Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
    }
}
