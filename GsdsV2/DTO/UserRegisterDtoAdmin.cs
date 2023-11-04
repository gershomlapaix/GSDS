using GsdsAuth.Models;

namespace GsdsV2.DTO
{
    public class UserRegisterDtoAdmin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string RoleId { get; set; }
        public int GroupId { get; set; }
        public string DepartmentId { get; set; }

        // Constructors
        public UserRegisterDtoAdmin() { }
        public UserRegisterDtoAdmin(User theUser) =>
            (Username, email, FullName, Phone) = (theUser.Username, theUser.email, theUser.FullName, theUser.Phone);
    }
}
