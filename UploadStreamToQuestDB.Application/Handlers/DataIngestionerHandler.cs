using CsvHelper;
using CsvHelper.Configuration;
using UploadStreamToQuestDB.Infrastructure.Services;
using System.Globalization;
using System.Text;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;
using Microsoft.Extensions.Configuration;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DataIngestionerHandler : AbstractHandler {
        private readonly IConfiguration configuration;
        private readonly IQueryIngestionerService _queryIngestionerService;
        public DataIngestionerHandler(
            IConfiguration configuration,
            IQueryIngestionerService queryIngestionerService) {
            this.configuration = configuration;
            this._queryIngestionerService = queryIngestionerService;
        }
        public override async Task<object> Handle(FileModelsInput files) {
            bool isStepActive = bool.Parse(configuration["AntivirusActive"]);

            Parallel.ForEach(files.Where(p => (
                isStepActive && p.State.Contains(FileModelState.ANTIVIRUS_OK))
                || (isStepActive == false && p.State.Contains(FileModelState.EXTENSION_OK))), (file) => {
                    Execute(files, file, _queryIngestionerService);
                });

            return base.Handle(files);
        }

        private void Execute(FileModelsInput files, FileModel file, IQueryIngestionerService processor) {
            try {
                var entry = new CsvFile<WeatherGermany>();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                    Delimiter = ";",
                    Comment = '%',
                    Encoding = Encoding.UTF8,
                    HasHeaderRecord = true };

                using (var reader = new StreamReader(file.FilePath))
                using (var csv = new CsvReader(reader, config)) {
                    entry.records = csv.GetRecords<WeatherGermany>().ToList();
                }

                processor.Execute(entry, files.SessionId);

                file.State.Add(FileModelState.INGESTION_READY);
            } catch (Exception) {
                file.State.Add(FileModelState.INGESTION_FAILED);
            }
        }
    }
}
