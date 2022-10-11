using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public class CreateTvShowViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [MinLength(3, ErrorMessage = "")]
        [MaxLength(55, ErrorMessage = "")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MinLength(3, ErrorMessage = "")]
        [MaxLength(255, ErrorMessage = "")]
        public string Description { get; set; }
    }
}
