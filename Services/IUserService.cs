using MinAuth.Models;

namespace MinAuth.Services{
    public interface IUserService{
        public UserModel GetUser(UserLogin userLogin);
    }
}