using Data;
using Microsoft.EntityFrameworkCore;
using Models.Anime;
using Models.ResponseModel;

namespace Business.Anime
{
    public class AnimeService : IAnimeSerivice
    {
        private readonly AppDbContext _dbContext;
        public AnimeService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseResult> GetAllAsync()
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
                Language = a.Language
            });

            return ResponseResult.Success(data);
        }


        public async Task<ResponseResult> GetAllByIdAsync(string Id)
        {
            var data = await _dbContext.Animes
                                             .Where(a => a.Id == Id)
                                             .FirstOrDefaultAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            var individualData = new GetAllAnimeResppnseModel
            {
                Name = data.Name,
                RatingLevel = data.RatingLevel,
                Language = data.Language
            };

            return ResponseResult.Success(individualData);
        }

        public async Task<ResponseResult> UpdateAsync(string Id, UpdateAnimeRequestModel model)
        {
            var data = await _dbContext.Animes
                                            .Where(a => a.Id == Id)
                                            .FirstOrDefaultAsync();
            if (data == null)
            {
                return ResponseResult.Failed("Empty Record , Not found.");
            }

            data.Name = model.Name ?? data.Name;
            data.Language = model.Language ?? data.Language;
            data.RatingLevel = model.RatingLevel;

            _dbContext.Animes.Update(data);
            await _dbContext.SaveChangesAsync();
            return ResponseResult.Success("Data Updated Successfully.");
        }
        public async Task<ResponseResult> DeleteAsync(string Id)
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
    }
}
