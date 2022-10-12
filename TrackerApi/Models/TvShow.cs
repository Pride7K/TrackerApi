﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TrackerApi.Models
{
    public class TvShow
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public bool Available { get; set; }
        public bool StillGoing { get; set; }
        public GenreEnum Genre { get; set; }
        public virtual List<Episode> Episodes { get; set; }

        public virtual List<ActorTvShow> ActorTvShow { get; set; }

        public virtual List<UserTvShowFavorite> UserTvShowFavorite { get; set; }


        public enum GenreEnum
        {
            Drama,
            SciFi,
            Suspence,
            Horror,
            Adventure
        }

    }
}
