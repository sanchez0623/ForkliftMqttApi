using FluentValidation;
using ForkliftMqtt.Application.Behaviors;
using ForkliftMqtt.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ForkliftMqtt.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // 注册 AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // 注册 MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            // 添加管道行为
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            // 注册 FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 注册应用服务
            services.AddScoped<ISensorApplicationService, SensorApplicationService>();

            return services;
        }
    }
}
