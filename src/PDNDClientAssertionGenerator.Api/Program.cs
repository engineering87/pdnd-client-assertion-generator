// (c) 2024 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using PDNDClientAssertionGenerator.Middleware;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var configuration = builder.Configuration;

        // Add configuration PDNDClientAssertionGenerator
        //builder.Services.Configure<ClientAssertionConfig>(configuration.GetSection("ClientAssertionConfig"));
        //builder.Services.AddSingleton<ClientAssertionConfig>();
        //builder.Services.AddScoped<IOAuth2Service, OAuth2Service>();
        //builder.Services.AddScoped<IClientAssertionGenerator, ClientAssertionGeneratorService>();
        builder.Services.AddPDNDClientAssertionServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}