using Common_Shared.ResponseResult;
using Models.Anime;
using Models.PaginationModel;

namespace Business.Anime
{
    public interface IAnimeSerivice
    {
        Task<ResponseResult> CreateAnimeAsync(CreateAnimeRequestModel model);
        Task<ResponseResult> GetAllAnimeAsync(PaginationRequestModel model);
        Task<ResponseResult> GetAllAnimeByIdAsync(string Id);
        Task<ResponseResult> UpdateAnimeAsync(string Id, UpdateAnimeRequestModel model);
        Task<ResponseResult> DeleteAnimeAsync(string Id);
        Task<Tuple<bool, byte[],string>> ExportAnimeExcelAsync();
       
    }
}
