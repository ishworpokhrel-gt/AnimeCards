using Microsoft.AspNetCore.Http;

namespace Models.Anime
{
    public class UpdateAnimeRequestModel
    {
        public string? Name { get; set; }
        public string? Language { get; set; }
        public int RatingLevel { get; set; }
        public IFormFile Image { get; set; }
    }

}
