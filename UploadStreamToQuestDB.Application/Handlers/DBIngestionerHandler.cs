using CsvHelper;
using CsvHelper.Configuration;
using UploadStreamToQuestDB.Infrastructure.Services;
using System.Globalization;
using System.Text;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;
using Microsoft.Extensions.Configuration;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DBIngestionerHandler : AbstractHandler {
        private readonly IConfiguration configuration;
        public DBIngestionerHandler(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public override async Task<object> Handle(FileModelsInput files) {
            bool isStepActive = bool.Parse(configuration["AntivirusActive"]);
            var processor = new QueryIngestionerService(
                configuration["QuestDbAddress"],
                configuration["QuestDbPort"],
                configuration["QuestDbSettings"]);

            Parallel.ForEach(files.Where(p => (
                isStepActive && p.State.Contains(FileModelState.ANTIVIRUS_OK))
                || (isStepActive == false && p.State.Contains(FileModelState.EXTENSION_OK))), (file) => {
                    Execute(files, file, processor);
                });

            return base.Handle(files);
        }

        private void Execute(FileModelsInput files, FileModel file, QueryIngestionerService processor) {
            try {
                var entry = new CsvFile<WeatherGermany>();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", Encoding = Encoding.UTF8 };

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
