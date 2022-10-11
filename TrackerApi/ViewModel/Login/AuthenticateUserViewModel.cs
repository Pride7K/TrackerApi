using System.ComponentModel.DataAnnotations;

namespace TrackerApi.ViewModel.Login
{
    public class AuthenticateUserViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MinLength(3, ErrorMessage = "")]
        [MaxLength(100, ErrorMessage = "")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MinLength(3, ErrorMessage = "")]
        [MaxLength(255, ErrorMessage = "")]
        public string Password { get; set; }
    }
}
