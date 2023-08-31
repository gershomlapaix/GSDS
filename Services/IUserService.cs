using GsdsAuth.Models;

namespace GsdsAuth.Services{
    public interface IUserService{
        public User GetUser(UserLogin userLogin);
    }
}