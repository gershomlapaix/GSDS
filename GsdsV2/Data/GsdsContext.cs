// create a database context
using Microsoft.EntityFrameworkCore;
using GsdsAuth.Models;
using Gsds.Models.Dossier;
using GsdsV2.Models.HelperModels;
using GsdsV2.Models.Dossier;

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



        // FROM HELPER MODELS
        public DbSet<PersonType>   PersonTypes { get; set;}
        public DbSet<Gender> Gender { get; set;}
        public DbSet<IdentifierType> IdentifierTypes { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<ComplaintCategory> complaintCategories { get; set; }
        public DbSet<ComplaintType> ComplaintTypes { get; set; }

           // LOCATIONS
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Cell> Cells { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Complaint>()
                .HasOne(e => e.Accused)
                .WithOne(e => e.Complaint)
                .HasForeignKey<Accused>(e => e.complaintCode)
                .IsRequired();
        }

    }
}