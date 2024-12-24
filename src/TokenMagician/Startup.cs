using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Svrooij.PowerShell.DI;

namespace TokenMagician
{
    public class Startup : PsStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // Add your services here
            //services.AddSingleton<ISampleService, SampleService>();
        }
        
        // Optionally over the logging configuration
        public override Action<Svrooij.PowerShell.DI.Logging.PowerShellLoggerConfiguration> ConfigurePowerShellLogging()
        {
            return builder =>
            {
                builder.DefaultLevel = LogLevel.Information;
                //builder.LogLevel["Svrooij.PowerShell.DependencyInjection.Sample.TestSampleCmdletCommand"] = LogLevel.Debug;
                // builder.LogLevel["Svrooij.PowerShell.DependencyInjection.Sample.TestService"] = LogLevel.Information;
                builder.IncludeCategory = true;
                builder.StripNamespace = true;
            };
        }
    }
}