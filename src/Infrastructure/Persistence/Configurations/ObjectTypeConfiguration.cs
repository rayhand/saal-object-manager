using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Entities;

namespace OMS.Infrastructure.Persistence.Configurations;

public class ObjectTypeConfiguration : IEntityTypeConfiguration<ObjectType>
{
    public void Configure(EntityTypeBuilder<ObjectType> builder)
    {
        // Create an alternate key by placing a unique constraint (and therefore a unique index)
        // on a property or properties other than those that form the primary key.
        // This is typically done to help ensure the uniqueness of data.
        //builder
        //    .HasAlternateKey(a => a.Name);
        builder.HasIndex(u => u.Name).IsUnique();

        builder.Property(t => t.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
