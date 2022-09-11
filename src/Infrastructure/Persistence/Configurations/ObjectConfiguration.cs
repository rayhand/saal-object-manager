using OMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Object = OMS.Domain.Entities.Object;
using IdentityModel;
using System.Reflection.Emit;

namespace OMS.Infrastructure.Persistence.Configurations;

public class ObjectConfiguration : IEntityTypeConfiguration<Object>
{
    public void Configure(EntityTypeBuilder<Object> builder)
    {
        // Create an alternate key by placing a unique constraint (and therefore a unique index)
        // on a property or properties other than those that form the primary key.
        // This is typically done to help ensure the uniqueness of data.
        //builder
        //.HasAlternateKey(a => a.Name);

        builder.HasIndex(u => u.Name).IsUnique();

        builder.Property(e => e.Description).HasMaxLength(500);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(d => d.ObjectType)
            .WithMany(p => p.Objects)
            .HasForeignKey(d => d.ObjectTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Objects_ObjectTypes");
    }
}
