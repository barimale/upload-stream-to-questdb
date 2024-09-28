using CsvHelper;
using CsvHelper.Configuration;
using UploadStreamToQuestDB.Infrastructure.Services;
using System.Globalization;
using System.Text;
using UploadStreamToQuestDB.Application.Handlers.Abstraction;
using UploadStreamToQuestDB.Domain;
using UploadStreamToQuestDB.Infrastructure.Model;
using static File.Api.Controllers.UploadController;

namespace UploadStreamToQuestDB.Application.Handlers {
    public class DBIngestionerHandler : AbstractHandler {
        public DBIngestionerHandler() {
        }
        public override object Handle(FileModels files) {
            try {
                var processor = new InsertAndQuery();

                files
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .ForAll(async file => {
                    var entry = new CsvFile<WeatherGermany>();
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", Encoding = Encoding.UTF8 };

                    using (var reader = new StreamReader(file.FilePath))
                    using (var csv = new CsvReader(reader, config)) {
                        entry.records = csv.GetRecords<WeatherGermany>().ToList();
                    }

                    await processor.Execute(entry, files.SessionId);
                });

                files.State = FileModelState.INGESTION_READY;

            } catch (Exception) {

                throw;
            }

            return base.Handle(files);
        }
    }
}
