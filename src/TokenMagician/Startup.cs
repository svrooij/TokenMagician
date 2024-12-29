using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Svrooij.PowerShell.DI;

namespace TokenMagician;

/// <summary>
/// The startup class for the module.
/// </summary>
public class Startup : PsStartup
{
    /// <summary>
    /// Configure the services for the module.
    /// </summary>
    /// <param name="services"></param>
    public override void ConfigureServices(IServiceCollection services)
    {

    }

    /// <summary>
    /// Configure the logging for the module.
    /// </summary>
    /// <returns></returns>
    public override Action<Svrooij.PowerShell.DI.Logging.PowerShellLoggerConfiguration> ConfigurePowerShellLogging()
    {
        return builder =>
        {
            builder.DefaultLevel = LogLevel.Debug;

            builder.IncludeCategory = true;
            builder.StripNamespace = true;
        };
    }
}
