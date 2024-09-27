using Database;
using File.Api.Handlers.Abstraction;
using Infrastructure.Entries;
using System;
using System.IO;
using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers {
    class DiskCleanUpHandler : AbstractHandler {
        private readonly IRepository repository;

        public DiskCleanUpHandler(IRepository repository) {
            this.repository = repository;
        }
        public override object Handle(FileModels files) {
            try {
                // put bool switch on off here
                foreach (var file in files) {
                    try {
                        System.IO.File.Delete(Path.Join(files.FilePath, file.file.FileName));
                    } catch (Exception) {
                        continue;
                    }
                }

                files.State = FileModelState.DISK_CLEANUP;

            } catch (Exception) {

                throw;
            } finally {
                // DB state
            }

            return base.Handle(files);
        }
    }
}
