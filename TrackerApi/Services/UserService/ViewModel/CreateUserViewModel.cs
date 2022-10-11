using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TrackerApi.Services.UserService.ViewModel
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MinLength(1, ErrorMessage = "")]
        [MaxLength(255, ErrorMessage = "")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MinLength(3, ErrorMessage = "")]
        [MaxLength(255, ErrorMessage = "")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MinLength(1, ErrorMessage = "")]
        [MaxLength(255, ErrorMessage = "")]
        public string Password { get; set; }
    }
}
