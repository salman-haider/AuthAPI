using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string userName {  get; set; }
        [Required]
        [MinLength(6, ErrorMessage ="Password is minimum 6 characters.")]
        public string password { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }

    }
}
