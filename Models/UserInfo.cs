using System.ComponentModel.DataAnnotations;

namespace JokesAPI.Models
{
    public class UserInfo
    {
        [Required(ErrorMessage = "Please provide a valid Id for the User")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Enter a valid user name")]
        [StringLength(64, MinimumLength = 1)]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Enter a first name. Must be at least 1 character")]
        [StringLength(64, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter a last name. Must be at least 1 character")]
        [StringLength(64, MinimumLength = 1)]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "A valid email address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A password is RequiredAttribute")]
        public string Password { get; set; }

    }
}
