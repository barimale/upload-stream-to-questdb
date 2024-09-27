using File.Api.Handlers.Abstraction;
using Infrastructure.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using static File.Api.Controllers.UploadController;

namespace File.Api.Handlers {
    class ExtensionHandler : AbstractHandler {
        private readonly Controller controller;
        private readonly IConfiguration configuration;
        public ExtensionHandler(
            Controller controller,
            IConfiguration configuration) {
            this.controller = controller;
            this.configuration = configuration;
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
            }

            return base.Handle(files);
        }
    }
}
