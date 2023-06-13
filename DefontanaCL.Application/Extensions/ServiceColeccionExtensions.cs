using DefontanaCL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaCL.Application.Extensions
{
    public static class ServiceColeccionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<PruebaContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );
        }
    }
}
