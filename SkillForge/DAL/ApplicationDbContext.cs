using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using SkillForge.Models;

namespace SkillForge.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            // Disable proxy creation
            this.Configuration.ProxyCreationEnabled = false;
            // Disable lazy loading
            this.Configuration.LazyLoadingEnabled = false;
            // Enable automatic migrations
            Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                base.OnModelCreating(modelBuilder);

                // Remove pluralizing table name convention
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

                // Configure unique indexes
                modelBuilder.Entity<User>()
                    .HasIndex(u => u.Email)
                    .IsUnique();

                modelBuilder.Entity<User>()
                    .HasIndex(u => u.Username)
                    .IsUnique();
            }
            catch (Exception ex)
            {
                // Log the error or handle it appropriately
                throw new Exception("Error configuring database model", ex);
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflicts
                throw new Exception("A concurrency error occurred while saving changes", ex);
            }
            catch (DbUpdateException ex)
            {
                // Handle update errors
                throw new Exception("An error occurred while updating the database", ex);
            }
            catch (Exception ex)
            {
                // Handle other errors
                throw new Exception("An error occurred while saving changes", ex);
            }
        }
    }
}