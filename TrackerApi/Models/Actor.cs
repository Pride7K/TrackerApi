using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TrackerApi.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<TvShow> TvShows { get; set; }
    }
}
