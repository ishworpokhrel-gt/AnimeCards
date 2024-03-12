using Common_Shared.ResponseResult;
using Models.Anime;

namespace Business.Anime
{
    public interface IAnimeSerivice
    {
        Task<ResponseResult> GetAllAsync();
        Task<ResponseResult> GetAllByIdAsync(string Id);
        Task<ResponseResult> UpdateAsync(string Id, UpdateAnimeRequestModel model);
        Task<ResponseResult> DeleteAsync(string Id);
    }
}
