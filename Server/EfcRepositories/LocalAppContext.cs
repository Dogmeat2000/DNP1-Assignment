using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfcRepositories;

public class LocalAppContext : DbContext {
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Forum> Forums => Set<Forum>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite(@"Data Source = ..\EfcRepositories\app.db")
            .LogTo(Console.WriteLine, LogLevel.Information);;
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // UserProfile entity configuration
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("UserProfiles");
            entity.HasKey(uP => uP.Profile_id);
            entity.Property(uP => uP.Username).IsRequired().HasMaxLength(100);
            entity.Property(uP => uP.Password).IsRequired().HasMaxLength(100);

            // Navigation properties
            entity.HasOne(uP => uP.User)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(uP => uP.User_id)
                .IsRequired();
        });
        
        
        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.User_id);

            // Navigation properties
            entity.HasOne(u => u.UserProfile)
                .WithOne(uP => uP.User)
                .IsRequired(false);
            
            entity.HasMany(u => u.ManagedForums)
                .WithOne(f => f.AuthoringUser)
                .HasForeignKey(f => f.Author_id)
                .IsRequired(false);
            
            entity.HasMany(u => u.ManagedPosts)
                .WithOne(p => p.AuthoringUser)
                .HasForeignKey(p => p.Author_id)
                .IsRequired(false);
            
            entity.HasMany(u => u.ManagedComments)
                .WithOne(c => c.AuthoringUser)
                .HasForeignKey(c => c.Author_Id)
                .IsRequired(false);
        });

        
        // Forum entity configuration
        modelBuilder.Entity<Forum>(entity =>
        {
            entity.ToTable("Forums");
            entity.HasKey(f => f.Forum_id);
            entity.Property(f => f.Timestamp_created).IsRequired();
            entity.Property(f => f.Timestamp_modified).IsRequired(false);
            entity.Property(f => f.Timestamp_deleted).IsRequired(false);
            entity.Property(f => f.LastPost_id).IsRequired(false);
            entity.Property(f => f.LastComment_id).IsRequired(false);
            entity.Property(f => f.LastCommentPost_id).IsRequired(false);
            

            // Navigation properties
            entity.HasOne(f => f.AuthoringUser)
                .WithMany(u => u.ManagedForums)
                .HasForeignKey(f => f.Author_id)
                .IsRequired();

            entity.HasMany(f => f.ChildPosts)
                .WithOne(p => p.ParentForum)
                .HasForeignKey(p => p.ParentForum_id)
                .IsRequired(false);
            
            entity.HasMany( f => f.ChildForums)
                .WithOne(f => f.ParentForum)
                .HasForeignKey(f => f.ParentForum_id)
                .IsRequired(false);
        });
        
        
        // Post entity configuration
        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");
            entity.HasKey(p => p.Post_id);
            entity.Property(p => p.Timestamp_created).IsRequired();
            entity.Property(p => p.Timestamp_modified).IsRequired(false);
            entity.Property(p => p.Timestamp_deleted).IsRequired(false);

            // Navigation properties
            entity.HasOne(p => p.AuthoringUser)
                .WithMany(u => u.ManagedPosts)
                .HasForeignKey(p => p.Author_id)
                .IsRequired();
            
            entity.HasOne(p => p.ParentForum)
                .WithMany(f => f.ChildPosts)
                .HasForeignKey(f => f.ParentForum_id)
                .IsRequired(false);
            
            entity.HasMany(p => p.ChildComments)
                .WithOne(c => c.ParentPost)
                .HasForeignKey(c => c.ParentPost_id)
                .IsRequired(false);
        });
        
        
        // Post entity configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.HasKey(c => c.Comment_id);

            // Navigation properties
            entity.HasOne(p => p.AuthoringUser)
                .WithMany(u => u.ManagedComments)
                .HasForeignKey(c => c.Author_Id)
                .IsRequired(false);
            
            entity.HasOne(c => c.ParentPost)
                .WithMany(p => p.ChildComments)
                .HasForeignKey(c => c.ParentPost_id)
                .IsRequired();
        });
        
    }
}