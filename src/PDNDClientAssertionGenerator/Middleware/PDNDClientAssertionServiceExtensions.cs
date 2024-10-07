using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PDNDClientAssertionGenerator.Configuration;
using PDNDClientAssertionGenerator.Interfaces;
using PDNDClientAssertionGenerator.Services;

namespace PDNDClientAssertionGenerator.Middleware
{
    public static class PDNDClientAssertionServiceExtensions
    {
        /// <summary>
        /// Configures the services required for the PDND Client Assertion process.
        /// This method sets up the configuration for `ClientAssertionConfig` and registers necessary services.
        /// </summary>
        /// <param name="services">The IServiceCollection to which the services are added.</param>
        /// <returns>The updated IServiceCollection instance.</returns>
        public static IServiceCollection AddPDNDClientAssertionServices(this IServiceCollection services)
        {
            // Use ConfigurationManager to load the configuration file (appsettings.json)
            var configuration = new ConfigurationManager()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load configuration
                .Build();

            // Ensure that the configuration contains required sections and values
            var configSection = configuration.GetSection("ClientAssertionConfig");
            if (!configSection.Exists())
            {
                throw new InvalidOperationException("Missing 'ClientAssertionConfig' section in appsettings.json.");
            }

            // Register ClientAssertionConfig as a singleton using the IOptions pattern
            services.Configure<ClientAssertionConfig>(config =>
            {
                // Copy values from the configuration file into the ClientAssertionConfig model
                config.ClientId = configuration["ClientAssertionConfig:ClientId"];
                config.ServerUrl = configuration["ClientAssertionConfig:ServerUrl"];
                config.KeyId = configuration["ClientAssertionConfig:KeyId"];
                config.Algorithm = configuration["ClientAssertionConfig:Algorithm"];
                config.Type = configuration["ClientAssertionConfig:Type"];
                config.Issuer = configuration["ClientAssertionConfig:Issuer"];
                config.Subject = configuration["ClientAssertionConfig:Subject"];
                config.Audience = configuration["ClientAssertionConfig:Audience"];
                config.PurposeId = configuration["ClientAssertionConfig:PurposeId"];
                config.KeyPath = configuration["ClientAssertionConfig:KeyPath"];
                config.Duration = int.Parse(configuration["ClientAssertionConfig:Duration"]);
            });

            // Register OAuth2Service and ClientAssertionGeneratorService as scoped services
            services.AddScoped<IOAuth2Service, OAuth2Service>();
            services.AddScoped<IClientAssertionGenerator, ClientAssertionGeneratorService>();

            // Return the updated service collection
            return services;
        }
    }
}
