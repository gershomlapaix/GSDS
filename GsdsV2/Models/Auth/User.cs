namespace GsdsAuth.Models{
    public class User{
        
        // properties
        public int UserId {get; set;}
        public string Username {get;set;}
        public byte[] Password {get;set;}
        public string email {get;set;}
        public string ID_ROLE  {get;set;}
        public string FullName { get;set;}
        public string Phone { get;set;}
}
}