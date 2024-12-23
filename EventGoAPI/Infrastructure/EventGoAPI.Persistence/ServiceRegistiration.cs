﻿using EventGoAPI.Application.Abstractions.Repositories;
using EventGoAPI.Application.Abstractions.Services;
using EventGoAPI.Persistence.Concretes.Repositories;
using EventGoAPI.Persistence.Concretes.Services;
using EventGoAPI.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Persistence
{
    public static class ServiceRegistiration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<EventGoDbContextFactory>()
                .Build();

            services.AddDbContext<EventGoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            services.AddScoped<IEventReadRepository, EventReadRepository>();
            services.AddScoped<IEventWriteRepository, EventWriteRepository>();

            services.AddScoped<IParticipantReadRepository, ParticipantReadRepository>();
            services.AddScoped<IParticipantWriteRepository, ParticipantWriteRepository>();

            services.AddScoped<IPointReadRepository, PointReadRepository>();
            services.AddScoped<IPointWriteRepository, PointWriteRepository>();

            services.AddScoped<IMessageReadRepository, MessageReadRepository>();
            services.AddScoped<IMessageWriteRepository, MessageWriteRepository>();

            services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();
            services.AddScoped<INotificationReadRepository, NotificationReadRepository>();

            services.AddScoped<IFeedbackReadRepository, FeedbackReadRepository>();
            services.AddScoped<IFeedbackWriteRepository, FeedbackWriteRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
