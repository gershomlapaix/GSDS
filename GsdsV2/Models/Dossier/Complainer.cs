// This class contains the details about the complainer
using System.ComponentModel.DataAnnotations.Schema;
using Gsds.Constants.Dossier;
using GsdsAuth.Models;

namespace Gsds.Models.Dossier{
    public class Complainer{

        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id {get; set;}
        public Identifier Identifier {get; set;}
        public string Name {get; set;}
        public Gender Gender {get; set;}
        public MaritalStatus MaritalStatus {get; set;}

        public string Country {get; set;}
        public string Province {get; set;}
        public string District {get; set;}
        public string Sector {get; set;}
        public string Cell {get; set;}

        public string Phone {get; set;}
        public string Email {get; set;}

        // Foreign key
        public int userId {get; set;}
        public User User {get; set;}     // navigation property
    }
}