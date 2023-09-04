using GsdsAuth.Models;

namespace GsdsAuth.Repository{
    public class UserRepository{
        public static List<User> Users = new List<User>(){
            new User(){Username = "jason_admin", Email="jason.admin@gmail.com", Password="mypass_!!word", FirstName="Jason", LastName="Kobe", Role="Administrator"},
            new User(){Username = "kareemMario", Email="kareem@gmail.com", Password="mypass_!!word", FirstName="Kareem", LastName="mario", Role="User"}
        }; 
    }
}