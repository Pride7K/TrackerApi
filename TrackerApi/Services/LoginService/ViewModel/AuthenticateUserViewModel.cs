using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.Login.ViewModel
{
    public struct AuthenticateUserViewModel
    {
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
