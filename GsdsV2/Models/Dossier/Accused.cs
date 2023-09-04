// Accused model
using Gsds.Constants.Dossier;

namespace Gsds.Models.Dossier{
    public class Accused{
     public Category Category;
     public Identifier Identifier;
     public string? national_id;
     public string? other_identifier;
     public string? Name;
     public Gender Gender;
     public MaritalStatus MaritalStatus;
     public string? Phone;
     public string? Email;
     public DateTime DateOfBirth;

    //  location
    public string? Province;
    public string? District;
    public string? Sector;
    public string? Cell;
    }
}