// create a database context
using Microsoft.EntityFrameworkCore;
using GsdsAuth.Models;
using Gsds.Models.Dossier;
using GsdsV2.Models.HelperModels;
using GsdsV2.Models.Dossier;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using GsdsV2.Controllers.Dossier;
using GsdsV2.Models.Users;

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
        public DbSet<ComplaintAdditionalData> ComplaintAdditionalData { get; set; }
        public DbSet<ComplaintAdditionalInfoReply> ComplaintAdditionalInfoReplies { get; set; }
        public DbSet<ComplaintClose> ComplaintClose { get; set; }
        public DbSet<ComplaintStatus> ComplaintStatuses { get; set; }
        public DbSet<ComplaintMemo> ComplaintMemos { get; set; }

        // FOR USERS
        public DbSet<UserSection> UserSections { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Department> Departments { get; set; }

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
        public DbSet<InstitutionComplaint> InstitutionComplaints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // users
            modelBuilder.Entity<User>()
            .HasOne(_ => _.TheGroup)
            .WithMany(a => a.Users)
            .HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<User>()
            .HasOne(_ => _.ManagerRoles)
            .WithMany(a => a.Users)
            .HasForeignKey(p => p.ID_ROLE);

            modelBuilder.Entity<User>()
           .HasOne(_ => _.Department)
           .WithMany(d => d.Users);

            // Complaint
            modelBuilder.Entity<Accused>()
            .HasOne(_ => _.Complaint)
            .WithOne(a => a.Accused)
            .HasForeignKey<Accused>(p => p.complaintCode);

            // Complaint and memo
            modelBuilder.Entity<ComplaintMemo>()
                .HasOne(c => c.Complaint)
                .WithMany(cm => cm.ComplaintMemo)
                .HasForeignKey(cm => cm.ComplaintCode);

            // Complaint and status
            modelBuilder.Entity<Complaint>()
           .HasOne(_ => _.ComplaintStatus)
           .WithMany(c => c.Complaints)
           .HasForeignKey(c => c.StatusCode);

            // complaint and files
            modelBuilder.Entity<Complaint>()
            .HasMany(_ => _.ComplaintAttachments)
            .WithOne(a => a.Complaint)
            .HasForeignKey(a => a.ComplaintCode);

            // complaint and additional data
            modelBuilder.Entity<Complaint>()
             .HasMany(_ => _.ComplaintAdditionalData)
             .WithOne(a => a.Complaint)
             .HasForeignKey(a => a.ComplaintCode);

            // many to many of complaints and roles
            modelBuilder.Entity<Complaint>()
            .HasMany(e => e.ManagerRoles)
            .WithMany(e => e.Complaints)
            .UsingEntity<ComplaintRoles>();

            // Complaint and institutions
            modelBuilder.Entity<Complaint>()
            .HasMany(e => e.Institutions)
            .WithMany(e => e.Complaints)
            .UsingEntity<InstitutionComplaint>();

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

            modelBuilder.Entity<District>()
            .HasMany(_ => _.Complaints)
            .WithOne(a => a.District)
            .HasForeignKey(p => p.DistrictId);

            modelBuilder.Entity<Sector>()
            .HasMany(_ => _.Complaints)
            .WithOne(a => a.Sector)
            .HasForeignKey(p => p.SectorId);

            modelBuilder.Entity<Cell>()
            .HasMany(_ => _.Complaints)
            .WithOne(a => a.Cell)
            .HasForeignKey(p => p.CellId);

            // ------------ For complaint and complainer roles

            modelBuilder.Entity<ComplaintRoles>().HasKey(cr => new { cr.ComplaintCode, cr.RoleId });
            modelBuilder.Entity<InstitutionComplaint>().HasKey(cr => new { cr.InstitutionId, cr.complaintCode });
            modelBuilder.Entity<ComplainerAssignedToRole>().HasKey(cr => new {  cr.ComplainerId, cr.RoleId });

            modelBuilder.Entity<ComplaintRoles>()
                .HasOne<Complaint>(_ => _.Complaint)
                .WithMany(c => c.Roles)
                .HasForeignKey(cr => cr.ComplaintCode);

            modelBuilder.Entity<ComplaintRoles>()
                .HasOne<ManagerRoles>(_ => _.ManagerRoles)
                .WithMany(r => r.Roles)
                .HasForeignKey(sc => sc.RoleId);

            modelBuilder.Entity<InstitutionComplaint>()
                .HasOne<Complaint>(_ => _.Complaint)
                .WithMany(c => c.InstitutionCompls)
                .HasForeignKey(_ => _.complaintCode);

            modelBuilder.Entity<InstitutionComplaint>()
                .HasOne<Institution>(_ => _.Institution)
                .WithMany(r => r.InstitutionsCompls)
                .HasForeignKey(c => c.InstitutionId);

            //modelBuilder.Entity<ComplainerAssignedToRole>()
            //    .HasOne<Complainer>(_ => _.Complainer)
            //    .WithMany(c => c.AssignedToRoles)
            //    .HasForeignKey(_ => _.ComplainerId);

            //modelBuilder.Entity<ComplainerAssignedToRole>()
            //    .HasOne<ManagerRoles>(_ => _.ManagerRoles)
            //    .WithMany(r => r.AssignedToRoles)
            //    .HasForeignKey(sc => sc.RoleId);

            // ------- COMPLAINER
            modelBuilder.Entity<Province>()
                .HasMany(_ => _.Complainers)
                .WithOne(c => c.Province)
                .HasForeignKey(sc => sc.ProvinceId);

            modelBuilder.Entity<District>()
                .HasMany(_ => _.Complainers)
                .WithOne(c => c.District)
                .HasForeignKey(sc => sc.DistrictId);

            modelBuilder.Entity<Sector>()
                .HasMany(_ => _.Complainers)
                .WithOne(c => c.Sector)
                .HasForeignKey(sc => sc.SectorId);

            modelBuilder.Entity<IdentifierType>()
                .HasMany(_ => _.Complainers)
                .WithOne(c => c.IdentifierType)
                .HasForeignKey(sc => sc.IdType);

            modelBuilder.Entity<PersonType>()
                .HasMany(_ => _.Complainers)
                .WithOne(c => c.PersonType)
                .HasForeignKey(sc => sc.PeronTypeId);

            modelBuilder.Entity<Gender>()
                .HasMany(_ => _.Complainers)
                .WithOne(c => c.Gender)
                .HasForeignKey(sc => sc.GenderId);


            // ACCUSED
            modelBuilder.Entity<Province>()
                .HasMany(_ => _.Accuseds)
                .WithOne(c => c.Province)
                .HasForeignKey(sc => sc.ProvinceId);

            modelBuilder.Entity<District>()
                .HasMany(_ => _.Accuseds)
                .WithOne(c => c.District)
                .HasForeignKey(sc => sc.DistrictId);

            modelBuilder.Entity<Sector>()
                .HasMany(_ => _.Accuseds)
                .WithOne(c => c.Sector)
                .HasForeignKey(sc => sc.SectorId);

            modelBuilder.Entity<IdentifierType>()
                .HasMany(_ => _.Accuseds)
                .WithOne(c => c.IdentifierType)
                .HasForeignKey(sc => sc.IdType);

            modelBuilder.Entity<PersonType>()
                .HasMany(_ => _.Accuseds)
                .WithOne(c => c.PersonType)
                .HasForeignKey(sc => sc.PeronTypeId);

            modelBuilder.Entity<Gender>()
                .HasMany(_ => _.Accuseds)
                .WithOne(c => c.Gender)
                .HasForeignKey(sc => sc.GenderId);

            // Complaint management
            modelBuilder.Entity<ComplaintRoles>()
                .HasOne<Complaint>(cr => cr.Complaint)
                .WithMany(c => c.Roles)
                .HasForeignKey(cr => cr.ComplaintCode);
        }

    }
}