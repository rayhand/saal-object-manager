using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Entities;

namespace OMS.Infrastructure.Persistence.Configurations;

public class ObjectRelationshipConfiguration : IEntityTypeConfiguration<ObjectRelationship>
{
    public void Configure(EntityTypeBuilder<ObjectRelationship> builder)
    {
        builder.HasKey(e => new { e.ObjectId, e.RelatedObjectId });

        builder.HasOne(d => d.Object)
            .WithMany(p => p.ObjectRelationshipObjects)
            .HasForeignKey(d => d.ObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ObjectRelationships_Objects");

        builder.HasOne(d => d.RelatedObject)
            .WithMany(p => p.ObjectRelationshipRelatedObjects)
            .HasForeignKey(d => d.RelatedObjectId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_ObjectRelationships_Objects1");
    }
}
