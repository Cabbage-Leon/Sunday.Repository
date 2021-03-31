using Microsoft.EntityFrameworkCore;

namespace Sunday.Repository.Api.Data
{
    public class DemoContext : BaseDbContext
    {
        public DemoContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region 注册领域模型与数据库的映射关系

            modelBuilder.ApplyConfiguration(new UserMap());

            #endregion 注册领域模型与数据库的映射关系
        }
    }
}