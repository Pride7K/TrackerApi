namespace TrackerApi.Models
{
    public class ActorTvShow
    {
        public int ActorsId { get; set; }
        public int TvShowsId { get; set; }
        public virtual TvShow TvShow { get; set; }
        public virtual Actor Actor { get; set; }
    }
}
