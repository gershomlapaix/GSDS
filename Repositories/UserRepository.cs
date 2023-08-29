using GsdsAuth.Models;

namespace GsdsAuth.Repository{
    public class UserRepository{
        public static List<UserModel> Users = new List<UserModel>(){
            new UserModel(){Username = "jason_admin", Email="jason.admin@gmail.com", Password="mypass_!!word", FirstName="Jason", LastName="Kobe", Role="Administrator"},
             new UserModel(){Username = "kareemMario", Email="kareem@gmail.com", Password="mypass_!!word", FirstName="Kareem", LastName="mario", Role="User"}
        }; 
    }
}