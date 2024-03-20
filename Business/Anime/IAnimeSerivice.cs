using Common_Shared.ResponseResult;
using Models.Anime;

namespace Business.Anime
{
    public interface IAnimeSerivice
    {
        Task<ResponseResult> CreateAnimeAsync(CreateAnimeRequestModel model);
        Task<ResponseResult> GetAllAnimeAsync();
        Task<ResponseResult> GetAllAnimeByIdAsync(string Id);
        Task<ResponseResult> UpdateAnimeAsync(string Id, UpdateAnimeRequestModel model);
        Task<ResponseResult> DeleteAnimeAsync(string Id);
    }
}
