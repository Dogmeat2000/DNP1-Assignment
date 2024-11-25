using Entities;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class AppContext : DbContext {
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Forum> Forums => Set<Forum>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite("Data Source=app.db");
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
                .HasForeignKey<UserProfile>(uP => uP.User_id);
        });
        
        
        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.User_id);

            // Navigation properties
            entity.HasOne(u => u.UserProfile)
                .WithOne(uP => uP.User);
            
            entity.HasMany(u => u.ManagedForums)
                .WithOne(f => f.AuthoringUser)
                .HasForeignKey(f => f.Author_id);
            
            entity.HasMany(u => u.ManagedPosts)
                .WithOne(p => p.AuthoringUser)
                .HasForeignKey(p => p.Author_id);
            
            entity.HasMany(u => u.ManagedComments)
                .WithOne(c => c.AuthoringUser)
                .HasForeignKey(c => c.Author_Id);
        });
                
        
        // Forum entity configuration
        modelBuilder.Entity<Forum>(entity =>
        {
            entity.ToTable("Forums");
            entity.HasKey(f => f.Forum_id);

            // Navigation properties
            entity.HasOne(f => f.AuthoringUser)
                .WithMany(u => u.ManagedForums)
                .HasForeignKey(f => f.Author_id);
            
            entity.HasMany(f => f.ChildPosts)
                .WithOne(p => p.ParentForum)
                .HasForeignKey(p => p.ParentForum);
            
            entity.HasOne(f => f.ParentForum)
                .WithMany(f => f.ChildForums)
                .HasForeignKey(f => f.ParentForum_id);
        });
        
        
        // Post entity configuration
        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");
            entity.HasKey(p => p.Post_id);

            // Navigation properties
            entity.HasOne(p => p.AuthoringUser)
                .WithMany(p => p.ManagedPosts)
                .HasForeignKey(p => p.Author_id);
            
            entity.HasMany(p => p.ChildComments)
                .WithOne(c => c.ParentPost)
                .HasForeignKey(c => c.ParentPost_id);
        });
        
    }
}