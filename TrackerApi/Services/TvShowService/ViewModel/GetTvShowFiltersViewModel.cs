using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TrackerApi.Services.TvShowService.ViewModel
{
    public sealed class GetTvShowFiltersViewModel
    {

        [FromQuery(Name = "genre")]
        public int? Genre { get; set; }
        [FromQuery(Name = "available")]
        public bool? Available { get; set; }
        [FromQuery(Name = "still_going")]
        public bool? StillGoing { get; set; }

        [FromQuery(Name = "sort")]
        public string Sort { get; set; }

        [FromQuery(Name = "sort_by")]
        public string SortingBy { get; set; }

        public List<string> TypesSorting = new List<string>()
        {
            "genre",
            "available",
            "still_going",
        };
    }
}
