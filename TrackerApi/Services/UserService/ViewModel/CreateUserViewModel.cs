using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TrackerApi.Services.UserService.ViewModel
{
    public struct CreateUserViewModel
    {
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name must have at least one character")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please inform a valid email")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Email must have at least one character")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 4, ErrorMessage = "Password must have at least four character")]
        public string Password { get; set; }
    }
}
