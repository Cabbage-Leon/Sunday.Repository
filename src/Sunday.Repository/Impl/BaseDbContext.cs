using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sunday.Repository
{
    public class BaseDbContext : DbContext
    {

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetCurrentPathAssembly().Where(x => !x.GetName().Name.Equals("Sunday.Repository"));
            foreach (var assembly in assemblies)
            {
                //找到所有实体类
                IEnumerable<Type> entityTypes = assembly.GetTypes()
                    .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                    .Where(type => type.IsClass)
                    .Where(type => type.Name != nameof(Entity))
                    .Where(type => type.BaseType != null)
                    .Where(type => typeof(ITrack).IsAssignableFrom(type));

                foreach (var entityType in entityTypes)
                {
                    if (modelBuilder.Model.FindEntityType(entityType) != null) continue;
                    modelBuilder.Model.AddEntityType(entityType);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetTrackInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTrackInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTrackInfo()
        {
            ChangeTracker.DetectChanges();

            //新增和更新的实体
            var entries = this.ChangeTracker.Entries()
                .Where(x => x.Entity is ITrack)
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var entityBase = entity as ITrack;
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entityBase.UpdateModifyTime();
                        break;
                    case EntityState.Added:
                        entityBase.UpdateCreateTime();
                        break;
                }
            }
        }
    }
}
