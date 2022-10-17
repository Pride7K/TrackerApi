using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public sealed class CreateTvShowViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? StillGoing { get; set; }
    }
}
