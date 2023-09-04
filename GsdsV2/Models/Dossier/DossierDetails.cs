// This class holds the real content of the problem

using System.Diagnostics.CodeAnalysis;

namespace Gsds.Models.Dossier{
    public class DossierDetails{
        public int id;
        public string DossierCode;
        public string Complainer_NationalId;

        // the location where the problem happened
        public string? Province;
        public string? Country;
        public string? District;
        public string? Sector;
        public string? Cell;

        // problem content
        [NotNull]
        public string Subject;
        public IFormFile[] Attachments;
        public string[] PreviousInstitutions;
        public string Description;
    }
}