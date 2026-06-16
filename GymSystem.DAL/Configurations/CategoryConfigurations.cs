using GymSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar")
                .HasMaxLength(20);

            builder.HasData(
                             new Category { Id = 1, Name = "Cardio" },
                             new Category { Id = 2, Name = "Strength" },
                             new Category { Id = 3, Name = "Yoga" },
                             new Category { Id = 4, Name = "Boxing" },
                             new Category { Id = 5, Name = "CrossFit" }
                         );

        }
    }
}
