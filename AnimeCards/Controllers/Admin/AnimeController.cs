﻿using AnimeCards.Filters.AuthorizationFilters;
using Business.Business.cms.Anime;
using Common_Shared.Constants;
using Common_Shared.ResponseWrapper;
using Microsoft.AspNetCore.Mvc;
using Models.Anime;
using Models.PaginationModel;
using System.Threading.Tasks;

namespace AnimeCards.Controllers.Admin
{
    public class AnimeController : BaseApiController
    {
        private readonly IAnimeSerivice _animeService;
        public AnimeController(IAnimeSerivice animeService)
        {
            _animeService = animeService;
        }
        [HttpPost("CreateAnime")]
        [Permission(PermissionConstants.Create)]
        public async Task<IActionResult> CreateAnime(CreateAnimeRequestModel model)
        {
            var responseData = await _animeService.CreateAnimeAsync(model);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }


        [HttpPost("GetAllAnimes")]
        [Permission(PermissionConstants.Read)]
        public async Task<IActionResult> GetAll(PaginationRequestModel model)
        {
            var responseData = await _animeService.GetAllAnimeAsync(model);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessPaginateResponseWrapper<object, object>.WrapSuccess(responseData.Result, responseData.Pagination));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpPost("GetAnimeById")]
        [Permission(PermissionConstants.Read)]
        public async Task<IActionResult> GetAnimeById(string Id)
        {
            var responseData = await _animeService.GetAllAnimeByIdAsync(Id);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpPut("UpdateAnime")]
        [Permission(PermissionConstants.Update)]
        public async Task<IActionResult> UpdateAnime(string Id, UpdateAnimeRequestModel model)
        {
            var responseData = await _animeService.UpdateAnimeAsync(Id, model);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpDelete("DeleteAnime")]
        [Permission(PermissionConstants.Delete)]
        public async Task<IActionResult> DeleteAnime(string Id)
        {
            var responseData = await _animeService.DeleteAnimeAsync(Id);
            if (responseData.IsSuccess)
            {
                return Ok(SuccessResponseWrapper<object>.SuccessApi(responseData.Result));
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi(responseData.Message));

        }

        [HttpGet("ExportExcel")]
        [Permission(PermissionConstants.ExportExcel)]
        public async Task<IActionResult> ExportAnimeExcel()
        {
            var responseData = await _animeService.ExportAnimeExcelAsync();
            if (responseData.Item1)
            {
                return File(responseData.Item2, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", responseData.Item3);
            }
            return BadRequest(ErrorResponseWrapper.ErrorApi("Failed to get excel."));

        }
    }
}
