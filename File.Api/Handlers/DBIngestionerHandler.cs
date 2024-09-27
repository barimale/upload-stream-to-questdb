using CsvHelper;
using CsvHelper.Configuration;
using Database;
using File.Api.Handlers.Abstraction;
using Infrastructure.Entries;
using Infrastructure.Services;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers {
    class DBIngestionerHandler : AbstractHandler {
        public DBIngestionerHandler() {
        }
        public override object Handle(FileModels files) {
            try {
                var processor = new InsertAndQuery();

                files
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .ForAll(async file => {
                    var entry = new CsvFile<Foo>();
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";", Encoding = Encoding.UTF8 };

                    using (var reader = new StreamReader(file.FilePath))
                    using (var csv = new CsvReader(reader, config)) {
                        entry.records = csv.GetRecords<Foo>().ToList();
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
