using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Infrastructure
{
    public static class ServiceRegistiration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
