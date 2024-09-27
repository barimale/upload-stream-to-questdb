using Database;
using File.Api.Handlers.Abstraction;
using Infrastructure.Entries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers {
    class ExtensionHandler : AbstractHandler {
        private readonly Controller controller;
        private readonly IConfiguration configuration;
        private readonly IRepository repository;
        public ExtensionHandler(
            Controller controller,
            IConfiguration configuration,
            IRepository repository) {
            this.controller = controller;
            this.configuration = configuration;
            this.repository = repository;
        }
        public override object Handle(FileModels files) {
            try {
                // check extensions of streamed files
                foreach (var file in files) {
                    string extension = Path.GetExtension(file.file.FileName);
                    if (!extension.EndsWith(configuration["AllowedExtension"]))
                        throw new Exception();
                }

                files.State = FileModelState.EXTENSION_OK;
            } catch (Exception) {

                throw;
            } finally {
                // save files to disk and entry to DB
                // filename | path | sessionId  | state = notcompatible
            }

            return base.Handle(files);
        }
    }
}
