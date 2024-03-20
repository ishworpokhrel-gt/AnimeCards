using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Anime
{
    public class CreateAnimeRequestModel
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public int RatingLevel { get; set; }
        public IFormFile Image { get; set; }
    }
}
