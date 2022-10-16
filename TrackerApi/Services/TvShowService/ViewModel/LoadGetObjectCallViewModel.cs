using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrackerApi.Services.TvShowService.ViewModel
{

    public struct TvShowsGetObjectCallViewModel
    {
        [JsonPropertyName("tv_shows")]
        public List<LoadGetObjectCallViewModel> tv_shows { get; set; }
        public int page { get; set; }
        public int pages { get; set; }
    }

    public struct LoadGetObjectCallViewModel
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("status")]
        public string status { get; set; }

    }
}
