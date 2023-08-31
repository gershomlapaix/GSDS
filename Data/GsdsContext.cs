// create a database context
using Microsoft.EntityFrameworkCore;
using GsdsAuth.Models;

namespace Gsds.Data{

    // DbContext represents a connection or session that's used to query and save 
    // instances of entities in a database
    public class GsdsDb: DbContext{
        public GsdsDb(DbContextOptions options): base(options){}

        public DbSet<User> Users {get; set;}
    }
}