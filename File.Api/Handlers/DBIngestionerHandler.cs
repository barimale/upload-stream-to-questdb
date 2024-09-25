using CsvHelper;
using CsvHelper.Configuration;
using Database;
using File.Api.Handlers.Abstraction;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers {
    class DBIngestionerHandler : AbstractHandler {
        private readonly IRepository repository;
        public DBIngestionerHandler(IRepository repository) {
            this.repository = repository;
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
            } finally {
                // DB state | healthy | virus
            }

            return base.Handle(files);
        }
    }
}
