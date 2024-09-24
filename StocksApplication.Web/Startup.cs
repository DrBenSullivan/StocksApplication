using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StocksApplication.Core.Contracts.RepositoryContracts;
using StocksApplication.Core.Contracts.ServiceContracts;
using StocksApplication.Core.Models;
using StocksApplication.Core.Profiles;
using StocksApplication.Core.Services;
using StocksApplication.Infrastructure.Persistence;
using StocksApplication.Infrastructure.Repositories;


namespace StocksApplication
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddHttpClient();
            services.AddScoped<IFinnhubService, FinnhubService>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IStocksService, StocksService>();
            services.AddScoped<IStocksRepository, StocksRepository>();
            services.Configure<TradingOptions>(
                configuration.GetSection("TradingOptions")
            );

            services.AddAutoMapper(config =>
            {
                config.AddProfile<DomainModelToPresentationModelProfile>();
                config.AddProfile<PresentationModelToDomainModelProfile>();
            }, typeof(Program).Assembly);

            services.AddDbContext<StockMarketDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
