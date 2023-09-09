using GsdsAuth.Models;

namespace GsdsV2.DTO
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }

        // Constructors
        public UserDto() { }
        public UserDto(User theUser) =>
            (Username, email, FullName, Phone) = (theUser.Username, theUser.email, theUser.FullName, theUser.Phone);
    }
}
