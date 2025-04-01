using Serilog;

namespace ProductApp.API.Extensions
{
    public static class WebAppBuilderExtensions
    {
        /// <summary>
        /// Configuring Host
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureHost(this WebApplicationBuilder builder)
        {
            // Configure Serilog for logging
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
