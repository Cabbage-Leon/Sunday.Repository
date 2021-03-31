using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sunday.Repository.Api.Data
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // table
            //builder.ToTable("");

            //// keys
            //builder.HasKey(t => t.Id);

            //// Properties
            //builder.Property(p => p.Id).HasColumnName("Id").IsRequired();
        }
    }
}