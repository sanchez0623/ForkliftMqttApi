using ForkliftMqtt.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ForkliftDbContext>>();
            var context = services.GetRequiredService<ForkliftDbContext>();

            try
            {
                logger.LogInformation("开始数据库迁移...");

                // 应用所有待处理的迁移
                await context.Database.MigrateAsync();

                logger.LogInformation("数据库迁移完成");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "数据库迁移出错");
                throw;
            }
        }
    }
}
