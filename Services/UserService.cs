using MinAuth.Models;
using MinAuth.Repository;

namespace MinAuth.Services{
    public class UserService: IUserService{
        public UserModel GetUser(UserLogin userLogin){
            UserModel user = UserRepository.Users.FirstOrDefault(o => o.Username.Equals(userLogin.Username, StringComparison.OrdinalIgnoreCase)
            && o.Password.Equals(userLogin.Password));

            return user;
        }
    }
}