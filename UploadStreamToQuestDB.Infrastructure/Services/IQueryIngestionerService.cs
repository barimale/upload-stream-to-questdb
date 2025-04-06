using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Infrastructure.Services {
    public interface IQueryIngestionerService {
        /// <summary>
        /// Executes the ingestion of a CSV file into QuestDB asynchronously.
        /// </summary>
        /// <param name="file">The CSV file containing weather data.</param>
        /// <param name="sessionId">The session ID for the name of the table.</param>
        void Execute(CsvFile<WeatherGermany> file, string sessionId);
    }
}
