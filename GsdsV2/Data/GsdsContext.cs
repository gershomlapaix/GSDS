// create a database context
using Microsoft.EntityFrameworkCore;
using GsdsAuth.Models;
using Gsds.Models.Dossier;
using GsdsV2.Models.HelperModels;

namespace Gsds.Data
{

    // DbContext represents a connection or session that's used to query and save 
    // instances of entities in a database
    public class GsdsDb: DbContext{
        public GsdsDb(DbContextOptions options): base(options){}

        public DbSet<User> Users {get; set;}

        public DbSet<PersonType>   PersonTypes { get; set;}

        public DbSet<Gender> Gender { get; set;}
        public DbSet<Complainer> Complainers {get; set;}
    }
}