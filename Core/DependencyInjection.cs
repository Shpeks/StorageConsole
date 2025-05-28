using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Repositories;
using Core.Services;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public static IServiceProvider ConfigureServices()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        
        var configuration = builder.Build();

        var services = new ServiceCollection();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IBoxRepository, BoxRepository>();
        services.AddScoped<IPalletRepository, PalletRepository>();
        
        services.AddScoped<IBoxService, BoxService>();
        services.AddScoped<IPalletService, PalletService>();

        return services.BuildServiceProvider();
    }
}