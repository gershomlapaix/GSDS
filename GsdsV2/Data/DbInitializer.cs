using GsdsAuth.Models;

namespace Gsds.Data{
    public class DbInitializer{
        
        // initializer method
        public static async void Initialize(GsdsDb db){
            //db.Database.EnsureCreated();

            if(db.Users.Any()){
                return;
            }

            //var users = new User[]{
            //   new User(){Username = "jason_admin", email="jason.admin@gmail.com", Password="mypass_!!word", FullName="Jason Kobe", ID_ROLE="001"},
            //new User(){Username = "kareemMario", email="kareem@gmail.com", Password="mypass_!!word", FullName="Kareem Mario", ID_ROLE="003"}
            //};

            //foreach(User user in users){
            //    db.Users.Add(user);
            //}

            //await db.SaveChangesAsync();
        }
    }
}