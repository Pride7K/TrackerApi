using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public class CreateTvShowViewModel
    {
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must have at least one character")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Description must have at least one character")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public bool StillGoing { get; set; }
    }
}
