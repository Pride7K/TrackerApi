using System;
using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Models
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }

        public int TvShowId { get; set; }
        public virtual TvShow TvShow { get; set; }
    }
}
