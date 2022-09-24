using Serilog;

namespace Microsoft.AspNetCore.Builder;

public static class IHostBuilderExtensions
{
    public static IHostBuilder ConfigureLogger(this IHostBuilder builder, IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            builder.UseSerilog((hostBuilder, loggerConfiguration) =>
            {
                loggerConfiguration
                    .WriteTo.Console();
            });
        }
        else
        {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "log/SearchOn-Log-.txt");

            builder.UseSerilog((hostBuilder, loggerConfiguration) =>
            {
                loggerConfiguration
                    .WriteTo.Console()
                    .WriteTo.File(logPath, rollingInterval: RollingInterval.Hour);
            });
        }

        return builder;
    }
}