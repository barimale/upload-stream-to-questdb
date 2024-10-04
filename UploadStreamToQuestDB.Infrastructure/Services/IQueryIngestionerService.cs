using UploadStreamToQuestDB.Domain;

namespace UploadStreamToQuestDB.Infrastructure.Services {
    public interface IQueryIngestionerService {
        void Execute(CsvFile<WeatherGermany> file, string sessionId);
    }
}