namespace TrackerApi.Models
{
    public class UserTvShowFavorite
    {
        public int UserId { get; set; }
        public int TvShowsId { get; set; }
        public virtual TvShow TvShow { get; set; }
        public virtual User User { get; set; }
    }
}
