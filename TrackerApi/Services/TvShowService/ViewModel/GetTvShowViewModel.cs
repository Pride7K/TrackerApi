using System.Collections.Generic;
using TrackerApi.Models;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public class GetTvShowViewModel
    {
        public int TotalTvShows { get; set; }
        public List<TvShow> TvShows { get; set; }
    }
}
