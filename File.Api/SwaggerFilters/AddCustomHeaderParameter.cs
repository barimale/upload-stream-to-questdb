using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace File.Api.SwaggerFilters {
    public class AddCustomHeaderParameter
        : IOperationFilter {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context) {
            if (operation.Parameters is null) {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (!operation.Parameters.Where(p => p.Name == "X-SessionId").Any()) {
                operation.Parameters.Add(new OpenApiParameter {
                    Name = "X-SessionId",
                    In = ParameterLocation.Header,
                    Description = "X-SessionId here",
                    Required = true,
                });
            }
        }


        /// <summary>
        /// Indicates swashbuckle should consider the parameter as a header parameter
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class FileContentType : Attribute {

        }
    }
}
