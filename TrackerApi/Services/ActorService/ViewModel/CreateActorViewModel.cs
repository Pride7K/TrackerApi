using System.ComponentModel.DataAnnotations;

namespace TrackerApi.Services.ActorService.ViewModel
{
    public struct CreateActorViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
