using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.ActorService.ViewModel
{
    public struct CreateActorViewModel
    {
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must have at least one character")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must have at least one character")]
        public string Description { get; set; }
    }
}
