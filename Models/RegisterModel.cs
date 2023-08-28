// User registration model class

using System.ComponentModel.DataAnnotations;

namespace Gsds.Models{

  public class RegisterModel{
      // properties
    [Required(ErrorMessage ="Name is required")]
    public string Name {get; set;}

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email {get; set;}

    [Required(ErrorMessage ="Password is required.")]
    public string Password {get; set;}
  }
}