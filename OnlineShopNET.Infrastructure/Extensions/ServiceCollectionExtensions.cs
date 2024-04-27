﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShopNET.Infrastructure.Data;

namespace OnlineShopNET.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnectionOS");
            services.AddDbContext<OnlineShopDbContext>(options =>
            options.UseSqlServer(connectionString));


        }
    }
}
