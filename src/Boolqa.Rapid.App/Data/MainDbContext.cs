using System;
using System.Reflection;
using System.Text;
using Boolqa.Rapid.PluginCore.Data;
using Microsoft.EntityFrameworkCore;

namespace Boolqa.Rapid.App.Data;

public partial class MainDbContext : DbContext
{
    private readonly IEnumerable<Assembly> _pluginAssemblies;

    public MainDbContext()
    {
        _pluginAssemblies = new Assembly[0];
    }

    // todo: список сборок вынести в отдельный класс и этот класс передавать сюда (объект динамической привязки сущностей из плагинов)
    public MainDbContext(IEnumerable<Assembly> assemblies)
    {
        _pluginAssemblies = assemblies;
    }

    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EntityHistory> EntityHistories { get; set; }

    public virtual DbSet<GroupObject> GroupObjects { get; set; }

    public virtual DbSet<LinkObject> LinkObjects { get; set; }

    public virtual DbSet<CoreObject> Objects { get; set; }

    public virtual DbSet<CorePlugin> Plugins { get; set; }

    public virtual DbSet<SettingObject> SettingObjects { get; set; }

    public virtual DbSet<SharedObject> SharedObjects { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=8RKf45n9;Host=localhost;Port=5432;Database=rapid;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoreObject>().UseTptMappingStrategy(); // Устанавливаем подход TPT

        modelBuilder
            .HasPostgresEnum("object_link_type", new[] { "linked" })
            .HasPostgresEnum("object_type", new[] { "object", "category", "tag", "custom" })
            .HasPostgresEnum("setting_type", new[] { "system", "user", "plugin" })
            .HasPostgresEnum("shared_mode", new[] { "denied", "read", "write", "share" })
            .HasPostgresEnum("variable_type", new[] { "stringboolintegerfloattimedateTimeenum" });

        modelBuilder.Entity<EntityHistory>(entity =>
        {
            entity.HasKey(e => e.EntityHistoryId).HasName("entity_history_pkey");

            entity.ToTable("entity_history");

            entity.Property(e => e.EntityHistoryId)
                .ValueGeneratedNever()
                .HasColumnName("entity_history_id");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.FieldName)
                .HasMaxLength(50)
                .HasColumnName("field_name");
            entity.Property(e => e.NewValue).HasColumnName("new_value");
            entity.Property(e => e.OldValue).HasColumnName("old_value");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<GroupObject>(entity =>
        {
            //entity.HasKey(e => e.ObjectId).HasName("group_object_pkey");

            entity.ToTable("group_object");

            entity.Property(e => e.ObjectId)
                .ValueGeneratedNever()
                .HasColumnName("group_object_id");

            //entity.HasOne(d => d.GroupObjectNavigation).WithOne(p => p.GroupObject)
            //    .HasForeignKey<GroupObject>(d => d.GroupObjectId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("group_object_group_object_id_fkey");
        });

        modelBuilder.Entity<LinkObject>(entity =>
        {
            //entity.HasKey(e => e.ObjectId).HasName("link_object_pkey");

            entity.ToTable("link_object");

            entity.HasIndex(e => e.ObjectFromId, "link_object_object_from_id_key").IsUnique();

            entity.HasIndex(e => e.ObjectToId, "link_object_object_to_id_key").IsUnique();

            entity.Property(e => e.ObjectId)
                .ValueGeneratedNever()
                .HasColumnName("link_object_id");
            entity.Property(e => e.ObjectFromId).HasColumnName("object_from_id");
            entity.Property(e => e.ObjectToId).HasColumnName("object_to_id");
            entity.Property(e => e.LinkType).HasColumnName("link_type");

            //entity.HasOne(d => d.LinkObjectNavigation).WithOne(p => p.LinkObjectLinkObjectNavigation)
            //    .HasForeignKey<LinkObject>(d => d.LinkObjectId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("link_object_link_object_id_fkey");

            entity.HasOne(d => d.ObjectFrom).WithMany(p => p.LinkObjectObjectFrom)
                .HasForeignKey(d => d.ObjectFromId)
                .HasConstraintName("link_object_object_from_id_fkey");

            entity.HasOne(d => d.ObjectTo).WithMany(p => p.LinkObjectObjectTo)
                .HasForeignKey(d => d.ObjectToId)
                .HasConstraintName("link_object_object_to_id_fkey");
        });

        modelBuilder.Entity<CoreObject>(entity =>
        {
            // Не вызывай .HasName("object_pkey") СУКА не хочет генерить уникальные имена производным таблицам
            entity.HasKey(e => e.ObjectId);

            entity.ToTable("object");

            entity.Property(e => e.ObjectId)
                .ValueGeneratedNever()
                .HasColumnName("object_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Objects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("object_user_id_fkey");
        });

        modelBuilder.Entity<CorePlugin>(entity =>
        {
            entity.HasKey(e => e.PluginId).HasName("plugin_pkey");

            entity.ToTable("plugin");

            entity.Property(e => e.PluginId)
                .ValueGeneratedNever()
                .HasColumnName("plugin_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PluginKey)
                .HasMaxLength(255)
                .HasColumnName("plugin_key");
            entity.Property(e => e.Version)
                .HasMaxLength(30)
                .HasColumnName("version");
        });

        modelBuilder.Entity<SettingObject>(entity =>
        {
            //entity.HasKey(e => e.ObjectId).HasName("setting_object_pkey");

            entity.ToTable("setting_object");

            entity.HasIndex(e => e.Key, "setting_object_key_key").IsUnique();

            entity.Property(e => e.ObjectId)
                .ValueGeneratedNever()
                .HasColumnName("setting_object_id");
            entity.Property(e => e.Key)
                .HasMaxLength(255)
                .HasColumnName("key");
            entity.Property(e => e.Value)
                .HasMaxLength(5000)
                .HasColumnName("value");
            entity.Property(e => e.SettingType).HasColumnName("setting_type");
            entity.Property(e => e.VariableType).HasColumnName("variable_type");

            //entity.HasOne(d => d.SettingObjectNavigation).WithOne(p => p.SettingObject)
            //    .HasForeignKey<SettingObject>(d => d.SettingObjectId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("setting_object_setting_object_id_fkey");
        });

        modelBuilder.Entity<SharedObject>(entity =>
        {
            //entity.HasKey(e => e.ObjectId).HasName("shared_object_pkey");

            entity.ToTable("shared_object");

            entity.Property(e => e.ObjectId)
                .ValueGeneratedNever()
                .HasColumnName("object_id");
            entity.Property(e => e.AccessUserId).HasColumnName("access_user_id");
            entity.Property(e => e.TargetObjectId).HasColumnName("target_object_id");
            entity.Property(e => e.Mode).HasColumnName("mode"); // ??? .HasColumnType("shared_mode")

            entity.HasOne(d => d.AccessUser).WithMany(p => p.SharedObjects)
                .HasForeignKey(d => d.AccessUserId)
                .HasConstraintName("shared_object_access_user_id_fkey");

            //entity.HasOne(d => d.Object).WithOne(p => p.SharedObjectObject)
            //    .HasForeignKey<SharedObject>(d => d.ObjectId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("shared_object_object_id_fkey");

            entity.HasOne(d => d.TargetObject).WithMany(p => p.SharedObjectTargetObjects)
                .HasForeignKey(d => d.TargetObjectId)
                .HasConstraintName("shared_object_target_object_id_fkey");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.TenantId).HasName("tenant_pkey");

            entity.ToTable("tenant");

            entity.Property(e => e.TenantId)
                .ValueGeneratedNever()
                .HasColumnName("tenant_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");

            entity.HasOne(d => d.OwnerUser).WithMany(p => p.Tenants)
                .HasForeignKey(d => d.OwnerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tenant_owner_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Users)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("user_tenant_id_fkey");
        });

        CreateStartData(modelBuilder);
        OnModelCreatingPartial(modelBuilder);

        modelBuilder.RegisterAllEntities<CoreObject>(_pluginAssemblies.ToArray());
    }

    private void CreateStartData(ModelBuilder modelBuilder)
    {
        var adminUserId = new Guid("63D26DF9-0E3F-4E00-8187-1A5F7B000001");
        var mainTenantId = new Guid("63D26DF9-0E3F-4E00-8187-1A5F7B000002");
        var sharedTestId = new Guid("63D26DF9-0E3F-4E00-8187-1A5F7B000003");

        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = adminUserId,
            Name = "Admin"
        });

        modelBuilder.Entity<Tenant>().HasData(new Tenant
        {
            TenantId = mainTenantId,
            Name = "Main",
            OwnerUserId = adminUserId
        });

        modelBuilder.Entity<SharedObject>().HasData(new SharedObject(sharedTestId, adminUserId, "Test share")
        {

        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

// todo: вынести в отдельный файл (объект динамической привязки сущностей из плагинов)
public static class ModelBuilderExtensions
{
    public static void RegisterAllEntities<BaseModel>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var types = assemblies
            .SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseModel).IsAssignableFrom(c));

        foreach (var type in types)
        {
            _ = modelBuilder.Entity(type);
        }

        foreach (var asm in assemblies)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(asm);
        }
    }
}
