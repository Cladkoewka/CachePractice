using CachePractice.Logic.Services;
using CachePractice.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CachePractice.API.Extensions;

public static class APIExtensions
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.AddScoped<CustomerService>();
        services.AddScoped<CustomerRepository>();
        services.AddDbContext<CustomerContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DbConnection"));
        });
        services.AddControllers();
    }
}