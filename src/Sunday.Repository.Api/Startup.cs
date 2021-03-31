using System;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sunday.Repository.Api.ServiceExtensions;

namespace Sunday.Repository.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextSetup();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitUserTable(app);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private void InitUserTable(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            using var conn = unitOfWork.GetConnection();
            var userTable = conn.QueryFirstOrDefault<string>("SHOW TABLES LIKE 'User';");
            if (!"User".Equals(userTable, StringComparison.CurrentCultureIgnoreCase))
            {
                conn.Execute(@"
                            CREATE TABLE `user` (
                              `Id` int(11) NOT NULL AUTO_INCREMENT,
                              `CreateTime` datetime(6) NOT NULL,
                              `UpdateTime` datetime(6) DEFAULT NULL,
                              `IsDelete` tinyint(1) NOT NULL,
                              `DeleteTime` datetime(6) DEFAULT NULL,
                              `UserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
                              `Password` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
                              `Mobile` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
                              PRIMARY KEY (`Id`)
                            ) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
                    ");
            }
        }
    }
}