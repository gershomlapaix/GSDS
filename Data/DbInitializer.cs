using GsdsAuth.Models;

namespace Gsds.Data{
    public class DbInitializer{
        
        // initializer method
        public static async void Initialize(GsdsDb db){
            db.Database.EnsureCreated();

            if(db.Users.Any()){
                return;
            }

            var users = new User[]{
                  new User(){Username = "jason_admin", Email="jason.admin@gmail.com", Password="mypass_!!word", FirstName="Jason", LastName="Kobe", Role="Administrator"},
            new User(){Username = "kareemMario", Email="kareem@gmail.com", Password="mypass_!!word", FirstName="Kareem", LastName="mario", Role="User"}
            };

            foreach(User user in users){
                db.Users.Add(user);
            }

            await db.SaveChangesAsync();
        }
    }
}