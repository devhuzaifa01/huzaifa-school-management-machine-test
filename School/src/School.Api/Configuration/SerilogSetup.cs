using Serilog;

namespace School.Api.Configuration
{
    public static class SerilogSetup
    {
        public static void AddSerilogConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
