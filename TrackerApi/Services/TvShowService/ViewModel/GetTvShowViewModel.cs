using System.Collections.Generic;
using TrackerApi.Models;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public struct GetTvShowViewModel
    {
        public int TotalTvShows { get; set; }
        public List<TvShow> TvShows { get; set; }
    }
}
