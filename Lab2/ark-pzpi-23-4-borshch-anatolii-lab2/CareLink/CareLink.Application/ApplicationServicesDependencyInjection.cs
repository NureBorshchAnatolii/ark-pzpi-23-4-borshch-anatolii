using CareLink.Application.Contracts.Services;
using CareLink.Application.Dtos.User;
using CareLink.Application.Implementations;
using CareLink.Application.Validators.User;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CareLink.Application
{
    public static class ApplicationServicesDependencyInjection
    {
        public static IServiceCollection AddApplicationServicesDependencyInjection(this IServiceCollection services)
        {
            //Validators
            services.AddScoped<IValidator<UpdateUserProfileRequest>, UpdateUserProfileValidator>();
            
            //Implementations
            services.AddScoped<IUserProfileService, UserProfileService>();
            
            return services;
        }
    }
}