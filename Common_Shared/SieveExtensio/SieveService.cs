using Entity;
using Microsoft.EntityFrameworkCore;
using Models.PaginationModel;
using Sieve.Models;
using Sieve.Services;

namespace Common_Shared.SieveExtensio
{
    public class SieveService : ISieveService
    {
        private readonly ISieveProcessor _sieveProcessor;
        public SieveService(ISieveProcessor sieveProcessor)
        {
            _sieveProcessor = sieveProcessor;
        }
        public async Task<(IQueryable<T> result, int totalCount, int totalPage)> ApplyPagination<T, TModel>(IQueryable<T> requestQuery, TModel model) where TModel : PaginationRequestModel
        {
            if (string.IsNullOrEmpty(model.Sorts))
                model.Sorts = $"-{nameof(ApplicationBaseEntity.CreatedOn)}";

            if (!string.IsNullOrEmpty(model.Query))
                model.Filters = $"{model.Query},{model.Filters}";

            var sieveModel = new SieveModel()
            {
                Filters = model.Filters,
                Sorts = model.Sorts
            };

            var source = _sieveProcessor.Apply(sieveModel, requestQuery, applyPagination: false);

            var totalCount = await source.CountAsync();

            var result = source
                                .Skip((model.PageNumber - 1) * model.PageSize)
                                .Take(model.PageSize);

            var totalPage = (int)Math.Ceiling(totalCount / (double)model.PageSize);

            return (result, totalCount, totalPage);
        }

        public async Task<(IQueryable<T> result, int totalCount, int totalPage)> ApplyPagination<T, TModel>(IQueryable<T> requestQuery, TModel model, string customfilter, string customsort) where TModel : PaginationRequestModel
        {
            if (string.IsNullOrEmpty(model.Sorts))
                model.Sorts = "-CreatedOn";

            if (!string.IsNullOrEmpty(model.Query))
                model.Filters = $"{model.Query},{model.Filters}";

            if (!string.IsNullOrEmpty(customfilter) && string.IsNullOrEmpty(model.Filters) && string.IsNullOrEmpty(model.Query))
                model.Filters = customfilter;

            var sieveModel = new SieveModel()
            {
                Filters = model.Filters,
                Sorts = model.Sorts
            };

            var source = _sieveProcessor.Apply(sieveModel, requestQuery, applyPagination: false);

            var totalCount = await source.CountAsync();

            var result = source
                                .Skip((model.PageNumber - 1) * model.PageSize)
                                .Take(model.PageSize);

            var totalPage = (int)Math.Ceiling(totalCount / (double)model.PageSize);

            return (result, totalCount, totalPage);
        }
    }
}

