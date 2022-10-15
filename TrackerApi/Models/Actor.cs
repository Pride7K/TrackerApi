using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TrackerApi.Models
{
    public sealed class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  List<ActorTvShow> ActorTvShow { get; set; }

    }
}
