using Business.Anime;
using Common_Shared.ResponseWrapper;
using Microsoft.AspNetCore.Mvc;
using Models.Anime;
using System.Threading.Tasks;

namespace AnimeCards.Controllers
{
    public class AnimeController : BaseApiController
    {
        private readonly IAnimeSerivice _animeService;
        public AnimeController(IAnimeSerivice animeService)
        {
            _animeService = animeService;
        }
        [HttpGet("GetAllAnimes")]
        public async Task<IActionResult> GetAll()
        {
            var responseData = await _animeService.GetAllAsync();
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpPost("GetAnimeById")]
        public async Task<IActionResult> GetAnimeById(string Id)
        {
            var responseData = await _animeService.GetAllByIdAsync(Id);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpPut("UpdateAnime")]
        public async Task<IActionResult> UpdateAnime(string Id, UpdateAnimeRequestModel model)
        {
            var responseData = await _animeService.UpdateAsync(Id, model);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpDelete("DeleteAnime")]
        public async Task<IActionResult> DeleteAnime(string Id)
        {
            var responseData = await _animeService.DeleteAsync(Id);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }
    }
}
