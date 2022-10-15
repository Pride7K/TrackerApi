namespace TrackerApi.Models
{
    public sealed class UserTvShowFavorite
    {
        public int UserId { get; set; }
        public int TvShowsId { get; set; }
        public  TvShow TvShow { get; set; }
        public  User User { get; set; }
    }
}
