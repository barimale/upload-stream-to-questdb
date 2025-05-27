using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Security.Authentication;

namespace UploadStreamToQuestDB.API {
    public class Program {
        public static void Main(string[] args) {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseIISIntegration()
                .UseKestrel(options => {
                    options.Limits.MaxRequestBodySize = long.MaxValue;
                })
                .Build();
    }
}
