using System;
using System.ComponentModel.DataAnnotations;

namespace TrackerApi.ViewModel.Episode
{
    public class CreateEpisodeViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MinLength(3,ErrorMessage ="")]
        [MaxLength(255,ErrorMessage ="")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public int TvShowId { get; set; }
    }
}
