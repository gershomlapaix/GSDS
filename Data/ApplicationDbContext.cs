using Gsds.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gsds.Data{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>{

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){}
        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);
        }
    }
}