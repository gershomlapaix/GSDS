namespace Gsds.Constants.Dossier{

    // Gender
    public enum Gender{
        MALE,
        FEMALE,
    }

// whether the accuser is the
    public enum DossierOwner{
        YES,
        NO
    }

// This is like a category in which the accuser is identified
    public enum Category{
        OWNER,
        GOVERNMENTAL_INSTITUTION,
        INDEPENDENT_INSTITUTION,
        OTHER
    }

// Identification
     public enum Identifier{
        NATIONAL_ID,
        PASSPORT,
        OTHER,
        DO_NOT_HAVE
     }

    //  Irangamimerere
    public enum MaritalStatus{
        MARRIED,
        DEVORCED,
        NOT_MARRIED,
        WIDOW,
        WIDOWER
    }
}