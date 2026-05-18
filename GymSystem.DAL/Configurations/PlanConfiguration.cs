using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(p => p.Description).HasColumnType("varchar").HasMaxLength(200);
            builder.Property(p => p.Price).HasColumnType("decimal(10,2)");
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_Plan_Duration", "DurationDays between 1 and 365");
            });
        }
    }
}
