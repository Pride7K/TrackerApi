using System;
using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.EpisodeService.ViewModel
{
    public struct CreateEpisodeViewModel
    {
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public DateTime ReleaseDate { get; set; }
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required", AllowEmptyStrings = false)]
        public int TvShowId { get; set; }
    }
}
