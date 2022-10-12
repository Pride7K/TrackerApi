using System.Collections.Generic;
using TrackerApi.Models;

namespace TrackerApi.Services.ActorService.ViewModel
{
    public struct GetActorViewModel
    {
        public int TotalActors{ get; set; }
        public List<Actor> Actors { get; set; }
    }
}
