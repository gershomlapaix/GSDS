namespace GsdsAuth.Models{
    public class User{
        
        // properties
        public int UserId {get; set;}
        public string? Username {get;set;}
        public byte[]? Password {get;set;}
        public string? email {get;set;}
        public string ID_ROLE  {get;set;}
        public string FullName { get;set;}
        public string Phone { get;set;}

        // Default properties
        public int status { get;set;}
        public int GroupId { get;set;}
        public int DEPARTMENT_ID { get;set;}
        public string EMPID { get;set;}

    }
}