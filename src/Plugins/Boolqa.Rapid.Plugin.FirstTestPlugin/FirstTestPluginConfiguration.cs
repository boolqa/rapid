using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Boolqa.Rapid.Plugin.FirstTestPlugin;

public class FirstTestPluginConfiguration : IEntityTypeConfiguration<HWObject>
{
    public void Configure(EntityTypeBuilder<HWObject> builder)
    {
        builder.ToTable("hello_world");
        
        //builder.Property(e => e.ObjectId)
        //    .ValueGeneratedNever()
        //    .HasColumnName("hw_object_id");

        builder.Property(e => e.LogText)
            .HasMaxLength(50)
            .HasColumnName("log_text");
    }
}
