using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UploadStreamToQuestDB.API.SwaggerFilters {
    public class FileUploadOperation : IOperationFilter {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="context">The context.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {

            var isFileUploadOperation =
                context.MethodInfo.CustomAttributes.Any(a => a.AttributeType == typeof(FileContentType));

            if (!isFileUploadOperation) return;

            operation.Parameters.Clear();

            var uploadFileMediaType = new OpenApiMediaType() {
                Schema = new OpenApiSchema() {
                    Type = "object",
                    Properties =
                    {
                    ["uploadedFile"] = new OpenApiSchema()
                    {
                        Description = "Upload File",
                        Type = "file",
                        Format = "formData"
                    }
                },
                    Required = new HashSet<string>() { "uploadedFile" }
                }
            };

            operation.RequestBody = new OpenApiRequestBody {
                Content = { ["multipart/form-data"] = uploadFileMediaType }
            };

            if (operation.Parameters is null) {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter {
                Name = "X-SessionId",
                In = ParameterLocation.Header,
                Description = "X-SessionId here",
                Required = true,
            });
        }

        /// <summary>
        /// Indicates swashbuckle should consider the parameter as a file upload
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class FileContentType : Attribute {

        }
    }
}
