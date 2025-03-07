using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SamtryggBrfPortal.Core.Entities;
using SamtryggBrfPortal.Infrastructure.Identity;

namespace SamtryggBrfPortal.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BrfAssociation> BrfAssociations { get; set; }
        public DbSet<BrfBoardMember> BrfBoardMembers { get; set; }
        public DbSet<BrfDocument> BrfDocuments { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<RentalApplication> RentalApplications { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BackgroundCheck> BackgroundChecks { get; set; }
        public DbSet<BackgroundCheckDocument> BackgroundCheckDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure entity relationships and constraints
            
            // BrfAssociation
            builder.Entity<BrfAssociation>()
                .HasMany(b => b.Properties)
                .WithOne(p => p.BrfAssociation)
                .HasForeignKey(p => p.BrfAssociationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<BrfAssociation>()
                .HasMany(b => b.BoardMembers)
                .WithOne(m => m.BrfAssociation)
                .HasForeignKey(m => m.BrfAssociationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BrfAssociation>()
                .HasMany(b => b.Documents)
                .WithOne(d => d.BrfAssociation)
                .HasForeignKey(d => d.BrfAssociationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property
            builder.Entity<Property>()
                .HasMany(p => p.RentalApplications)
                .WithOne(r => r.Property)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Property>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // RentalApplication
            builder.Entity<RentalApplication>()
                .HasMany(r => r.Documents)
                .WithOne(d => d.RentalApplication)
                .HasForeignKey(d => d.RentalApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RentalApplication>()
                .HasMany(r => r.Messages)
                .WithOne(m => m.RentalApplication)
                .HasForeignKey(m => m.RentalApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RentalApplication>()
                .HasOne(r => r.BackgroundCheck)
                .WithOne(b => b.RentalApplication)
                .HasForeignKey<BackgroundCheck>(b => b.RentalApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Document
            builder.Entity<Document>()
                .HasMany(d => d.Versions)
                .WithOne(v => v.Document)
                .HasForeignKey(v => v.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            // BackgroundCheck
            builder.Entity<BackgroundCheck>()
                .HasMany(b => b.Documents)
                .WithOne(d => d.BackgroundCheck)
                .HasForeignKey(d => d.BackgroundCheckId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure indexes for performance
            builder.Entity<RentalApplication>()
                .HasIndex(r => r.Status);

            builder.Entity<RentalApplication>()
                .HasIndex(r => r.StartDate);

            builder.Entity<RentalApplication>()
                .HasIndex(r => r.EndDate);

            builder.Entity<Notification>()
                .HasIndex(n => n.RecipientUserId);

            builder.Entity<Message>()
                .HasIndex(m => m.SenderUserId);

            builder.Entity<Message>()
                .HasIndex(m => m.RecipientUserId);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically set created/modified dates
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}