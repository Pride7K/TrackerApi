using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public class PutTvShowViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Available { get; set; }
    }
}
