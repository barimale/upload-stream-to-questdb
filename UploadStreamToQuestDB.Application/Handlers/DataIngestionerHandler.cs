using CsvHelper;
using CsvHelper.Configuration;
using UploadStreamToQuestDB.Infrastructure.Services;
using System.Globalization;
using System.Text;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DataIngestionerHandler : AbstractHandler, IDataIngestionerHandler {
        private readonly IConfiguration configuration;
        private readonly IQueryIngestionerService _queryIngestionerService;
        private readonly ILogger<DataIngestionerHandler> _logger;
        public DataIngestionerHandler(
            IConfiguration configuration,
            IQueryIngestionerService queryIngestionerService,
            ILogger<DataIngestionerHandler> logger) {
            this.configuration = configuration;
            this._queryIngestionerService = queryIngestionerService;
            this._logger = logger;
        }
        public override async Task<object> Handle(FileModelsInput files) {
            bool isStepActive = bool.Parse(configuration["AntivirusActive"]);

            Parallel.ForEach(files.Where(p => (
                isStepActive && p.State.Contains(FileModelState.ANTIVIRUS_OK))
                || (isStepActive == false && p.State.Contains(FileModelState.EXTENSION_OK))), (file) => {
                    Execute(files, file);
                });

            return base.Handle(files);
        }

        private void Execute(FileModelsInput files, FileModel file) {
            try {
                var entry = new CsvFile<WeatherGermany>();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                    Delimiter = ";",
                    Comment = '%',
                    Encoding = Encoding.UTF8,
                    HasHeaderRecord = true,
                    BadDataFound = context =>
                    {
                        _logger.LogError($"Bad data found on row {context.RawRecord}: {context.RawRecord}");
                    },
                    MissingFieldFound = (context) =>
                    {
                        _logger.LogError($"Field missing at index {context.Index}: {context.HeaderNames}");
                    },
                    ReadingExceptionOccurred = ex =>
                    {
                        _logger.LogError($"An error occurred while reading the CSV file: {ex.Exception.Message}");
                        return false;
                    }

                };

                using (var reader = new StreamReader(file.FilePath))
                using (var csv = new CsvReader(reader, config)) {
                    foreach(var record in csv.GetRecords<WeatherGermany>().ToList()) {
                        entry.Records.Add(record);
                    }
                }

                _queryIngestionerService.Execute(entry, files.SessionId);

                file.State.Add(FileModelState.INGESTION_READY);
            } catch (Exception) {
                file.State.Add(FileModelState.INGESTION_FAILED);
            }
        }
    }
}
