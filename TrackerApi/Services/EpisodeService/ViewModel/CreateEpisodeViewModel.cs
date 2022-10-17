using System;
using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.EpisodeService.ViewModel
{
    public struct CreateEpisodeViewModel
    {
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public int TvShowId { get; set; }
    }
}
