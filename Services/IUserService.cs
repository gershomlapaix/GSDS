using GsdsAuth.Models;

namespace GsdsAuth.Services{
    public interface IUserService{
        public UserModel GetUser(UserLogin userLogin);
    }
}