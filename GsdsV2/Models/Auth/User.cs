using GsdsV2.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

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
        public UserGroup TheGroup { get;set;}      // in case one section considered

        [Column("DEPARTMENT_ID")]
        public string? DepartmentId { get;set;}
        public Department? Department { get;set;}       // in case he/she works in one department
        public string? EMPID { get;set;}

    }
}