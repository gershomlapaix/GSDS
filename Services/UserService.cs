using GsdsAuth.Models;
using GsdsAuth.Repository;

namespace GsdsAuth.Services{
    public class UserService: IUserService{
        public User GetUser(UserLogin userLogin){
            User user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase)
            && o.Password.Equals(userLogin.Password));

            return user;
        }
    }
}