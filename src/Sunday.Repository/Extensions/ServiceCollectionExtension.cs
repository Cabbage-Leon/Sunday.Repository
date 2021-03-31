using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sunday.Repository
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 使用默认的BaseContext
        /// </summary>
        public static IServiceCollection AddRepository(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<BaseDbContext>(options);
            services.AddScoped<DbContext, BaseDbContext>();
            services.AddRepository();
            return services;
        }

        /// <summary>
        /// 使用自定义的Context,不过需要继承BaseContext
        /// </summary>
        public static IServiceCollection AddRepository<TDbContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options) where TDbContext : BaseDbContext
        {
            services.AddDbContext<TDbContext>(options);
            services.AddScoped<DbContext, TDbContext>();
            services.AddRepository();
            return services;
        }

        /// <summary>
        /// 仓储的注册
        /// </summary>
        private static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetCurrentPathAssembly().Where(x => !x.GetName().Name.Equals("Sunday.Repository"));
            services.AddRepository(assemblies, typeof(IRepository<>));
            services.AddRepository(assemblies, typeof(IRepository<,>));
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IRepository<,>), typeof(Repository<,>));
            return services;
        }

        /// <summary>
        /// 将实现了IRepository接口的仓储，注册进容器
        /// </summary>
        private static void AddRepository(this IServiceCollection services, IEnumerable<Assembly> assemblies, Type baseType)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                                    .Where(x => x.IsClass
                                            && !x.IsAbstract
                                            && x.BaseType != null
                                            && x.HasImplementedRawGeneric(baseType));
                foreach (var type in types)
                {
                    Type[] interfaces = type.GetInterfaces();
                    Type interfaceType = interfaces.FirstOrDefault(x => x.Name == $"I{type.Name}");
                    if (interfaceType == null) interfaceType = type;
                    ServiceDescriptor serviceDescriptor = new ServiceDescriptor(interfaceType, type, ServiceLifetime.Transient);
                    if (!services.Contains(serviceDescriptor)) services.Add(serviceDescriptor);
                }
            }
        }
    }
}
