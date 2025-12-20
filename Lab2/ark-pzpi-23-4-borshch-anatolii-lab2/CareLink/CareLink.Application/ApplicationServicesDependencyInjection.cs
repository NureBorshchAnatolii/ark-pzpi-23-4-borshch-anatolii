using CareLink.Application.Contracts.Repositories;
using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos.User;
using CareLink.Application.Implementations;
using CareLink.Application.Notifications;
using CareLink.Application.Validators.User;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CareLink.Application
{
    public static class ApplicationServicesDependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Validators
            services.AddScoped<IValidator<UpdateUserProfileRequest>, UpdateUserProfileValidator>();
            
            //Implementations
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<ICognitiveExerciseService, CognitiveExerciseService>();
            services.AddScoped<IIoTDeviceService, IoTDeviceService>();
            services.AddScoped<IIoTReadingService, IoTReadingService>();
            services.AddScoped<IRelativeService, RelativeService>();
            services.AddScoped<INotificationContentFactory, NotificationContentFactory>();

            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IRelativeMetricsService, RelativeMetricsService>();
            
            services.AddScoped<IMessageService>(sp =>
            {
                var service = new MessageService(
                    sp.GetRequiredService<IMessageRepository>(),
                    sp.GetRequiredService<IUserRepository>());

                return NotificationDecoratorFactory.Create<IMessageService>(
                    service,
                    sp.GetRequiredService<INotificationRepository>(),
                    sp.GetRequiredService<INotificationContentFactory>());
            });
            
            return services;
        }
    }
}