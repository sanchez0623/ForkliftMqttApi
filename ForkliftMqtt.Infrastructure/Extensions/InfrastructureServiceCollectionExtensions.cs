using ForkliftMqtt.Domain.Repositories;
using ForkliftMqtt.Domain.Services;
using ForkliftMqtt.Infrastructure.Messaging;
using ForkliftMqtt.Infrastructure.Persistence.DbContext;
using ForkliftMqtt.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ForkliftMqtt.Infrastructure.Extensions
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 注册DbContext
            services.AddDbContext<ForkliftDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null)
                )
            );

            // 添加领域层服务
            // 注册仓储
            services.AddScoped<ISensorRepository, SensorRepository>();
            services.AddScoped<ISensorReadingRepository, SensorReadingRepository>();
            services.AddScoped<IForkliftRepository, ForkliftRepository>();

            // 添加MQTT服务
            services.AddMqttServices(configuration);

            return services;
        }

        public static IServiceCollection AddMqttServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MqttSettings>(configuration.GetSection("Mqtt"));
            services.AddSingleton<ISensorDataService, MqttSensorDataService>();
            //services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();

            return services;
        }
    }
}
