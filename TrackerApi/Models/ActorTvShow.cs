namespace TrackerApi.Models
{
    public sealed class ActorTvShow
    {
        public int ActorsId { get; set; }
        public int TvShowsId { get; set; }
        public  TvShow TvShow { get; set; }
        public  Actor Actor { get; set; }
    }
}
