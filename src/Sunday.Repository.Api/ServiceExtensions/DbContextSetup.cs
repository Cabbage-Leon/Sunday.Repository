using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sunday.Repository.Api.Data;

namespace Sunday.Repository.Api.ServiceExtensions
{
    public static class DbContextSetup
    {
        public static void AddDbContextSetup(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddRepository<DemoContext>(options =>
            {
                options.UseMySql("数据库连接字符串");
            });

            //services.AddRepository(ops =>
            //{
            //    ops.UseMySql("server = 127.0.0.1;database=demo;uid=root;password=root;");
            //});
        }
    }
}