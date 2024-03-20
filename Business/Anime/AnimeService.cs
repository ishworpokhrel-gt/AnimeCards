using Common_Shared.ResponseResult;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Anime;

namespace Business.Anime
{
    public class AnimeService : IAnimeSerivice
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AnimeService(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }

        public async Task<ResponseResult> CreateAnimeAsync(CreateAnimeRequestModel model)
        {

            var addData = new Entity.Anime
            {
                Name = model.Name,
                Language = model.Language,
                RatingLevel = model.RatingLevel,
            };
            addData.ImageUrl = GetImagePath(model.Image);

            await _dbContext.Animes.AddAsync(addData);
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Success("Anime created successfully.");


        }

        public async Task<ResponseResult> GetAllAnimeAsync()
        {
            var storedData = await _dbContext.Animes.ToListAsync();
            if (storedData == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }
            var data = storedData.Select(a => new GetAllAnimeResppnseModel
            {
                Id = a.Id,
                Name = a.Name,
                RatingLevel = a.RatingLevel,
                Language = a.Language,
                ImageUrl = a.ImageUrl
            });

            return ResponseResult.Success(data);
        }


        public async Task<ResponseResult> GetAllAnimeByIdAsync(string Id)
        {
            var data = await _dbContext.Animes
                                             .Where(a => a.Id == Id)
                                             .FirstOrDefaultAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            var individualData = new GetAnimeByIdResponseModel
            {
                Name = data.Name,
                RatingLevel = data.RatingLevel,
                Language = data.Language,
                ImageUrl = data.ImageUrl
            };

            return ResponseResult.Success(individualData);
        }

        public async Task<ResponseResult> UpdateAnimeAsync(string Id, UpdateAnimeRequestModel model)
        {
            var data = await _dbContext.Animes
                                            .Where(a => a.Id == Id)
                                            .FirstOrDefaultAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            data.ImageUrl = GetImagePath(model.Image);
            data.Name = model.Name ?? data.Name;
            data.Language = model.Language ?? data.Language;
            data.RatingLevel = model.RatingLevel;

            _dbContext.Animes.Update(data);
            await _dbContext.SaveChangesAsync();
            return ResponseResult.Success("Data Updated Successfully.");
        }
        public async Task<ResponseResult> DeleteAnimeAsync(string Id)
        {
            var data = await _dbContext.Animes
                                           .Where(a => a.Id == Id)
                                           .FirstOrDefaultAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }
            _dbContext.Animes.Remove(data);
            await _dbContext.SaveChangesAsync();

            return ResponseResult.Success("Data Deleted Successfully.");
        }

        private string GetImagePath(IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string Url = "";
            if (file != null)
            {
                string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string Imagepath = Path.Combine(wwwRootPath, @"Images\Animeimage");

                using (var fileStream = new FileStream(Path.Combine(Imagepath, FileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Url = @"\images\Animeimage\" + FileName;
            }
            else return string.Empty;
        }


    }
}
