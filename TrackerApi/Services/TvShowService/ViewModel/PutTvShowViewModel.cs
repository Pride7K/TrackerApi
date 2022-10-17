using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public sealed class  PutTvShowViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Available { get; set; }
    }
}
