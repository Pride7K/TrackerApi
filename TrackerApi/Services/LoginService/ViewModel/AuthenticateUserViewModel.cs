using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.Login.ViewModel
{
    public class AuthenticateUserViewModel
    {
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
