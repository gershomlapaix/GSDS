// create a database context
using Microsoft.EntityFrameworkCore;
using GsdsAuth.Models;
using Gsds.Models.Dossier;
using GsdsV2.Models.HelperModels;
using GsdsV2.Models.Dossier;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;

namespace Gsds.Data
{

    // DbContext represents a connection or session that's used to query and save 
    // instances of entities in a database
    public class GsdsDb: DbContext{
        public GsdsDb(DbContextOptions options): base(options){}

        public DbSet<User> Users {get; set;}
        public DbSet<Complainer> Complainers { get; set; }
        public DbSet<Accused> Accuseds { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintManagement> ComplaintManagements { get; set; }


        // FROM HELPER MODELS
        public DbSet<Country> Countrys { get; set; }
        public DbSet<PersonType>   PersonTypes { get; set;}
        public DbSet<Gender> Gender { get; set;}
        public DbSet<IdentifierType> IdentifierTypes { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Category> complaintCategories { get; set; }
        public DbSet<ComplaintType> ComplaintTypes { get; set; }

           // LOCATIONS
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Cell> Cells { get; set; }

        public DbSet<ManagerRoles> ManagerRoles { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<ComplaintAttachment> Attachments { get; set; }

        // many to many relational tables
        public DbSet<ComplaintRoles> ComplaintRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Complaint
            modelBuilder.Entity<Complaint>()
            .HasMany(_ => _.Accuseds)
            .WithOne(a => a.Complaint)
            .HasForeignKey(p => p.complaintCode);

            // complaint and files
            modelBuilder.Entity<Complaint>()
            .HasMany(_ => _.ComplaintAttachments)
            .WithOne(a => a.Complaint)
            .HasForeignKey(a => a.ComplaintCode);

            // many to many of complaints and roles
            modelBuilder.Entity<Complaint>()
            .HasMany(e => e.ManagerRoles)
            .WithMany(e => e.Complaints)
            .UsingEntity<ComplaintRoles>();

            // complaint and category
            modelBuilder.Entity<Complaint>()
            .HasOne(_ => _.ComplaintCategory)
            .WithMany(a => a.Complaints)
            .HasForeignKey(c => c.ComplaintCategoryId);

            // province and complaints
            modelBuilder.Entity<Province>()
            .HasMany(_ => _.Complaints)
            .WithOne(a => a.Province)
            .HasForeignKey(p => p.ProvinceId);


            modelBuilder.Entity<ComplaintRoles>().HasKey(cr => new { cr.ComplaintCode, cr.RoleId });

            modelBuilder.Entity<ComplaintRoles>()
                .HasOne<Complaint>(cr => cr.Complaint)
                .WithMany(c => c.Roles)
                .HasForeignKey(cr => cr.ComplaintCode);


            modelBuilder.Entity<ComplaintRoles>()
                .HasOne<ManagerRoles>(cr => cr.ManagerRoles)
                .WithMany(r => r.Roles)
                .HasForeignKey(sc => sc.RoleId);
        }

    }
}