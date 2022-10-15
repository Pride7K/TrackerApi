using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public class PutTvShowViewModel
    {
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must have at least one character")]
        public string Title { get; set; }
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Description must have at least one character")]
        public string Description { get; set; }
        public bool? Available { get; set; }
    }
}
