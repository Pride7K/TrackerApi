using System.Collections.Generic;

namespace TrackerApi.Models
{
    public sealed class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Active = true;

        public  List<UserTvShowFavorite> UserTvShowFavorite { get; set; }

    }
}
