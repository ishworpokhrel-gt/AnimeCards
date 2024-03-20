using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class Anime : ApplicationBaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Language { get; set; }
        public int RatingLevel { get; set; }
        public string ImageUrl { get; set; }
    }
}
