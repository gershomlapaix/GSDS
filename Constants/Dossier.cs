namespace Gsds.Constants.Dossier{

    // Gender
    public enum Gender{
        Male,
        Female,
    }

// whether the accuser is the
    public enum DossierOwner{
        Yes,
        No
    }

// This is like a category in which the accuser is identified
    public enum DossierRegistrer{
        Owner,
        Governmental_Institution,
        Independent_Institution,
        Other
    }

// Identification
     public enum Identifier{
        National_Id,
        Passport,
        Other,
        Do_Not_have
     }
}