// This class contains the details about the complainer
using Gsds.Constants.Dossier;

namespace Gsds.Models.Dossier{
    public class Complainer{
        public string Id;
        public Identifier Identifier;
        public string Name;
        public Gender Gender;
        public MaritalStatus MaritalStatus;

        public string? Country;
        public string? Province;
        public string? District;
        public string? Sector;
        public string? Cell;

        public string? Phone;
        public string? Email;
    }
}