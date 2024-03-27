using Models.PaginationModel;

namespace Common_Shared.SieveExtensio
{
    public interface ISieveService
    {
        Task<(IQueryable<T> result, int totalCount, int totalPage)> ApplyPagination<T, TModel>(IQueryable<T> requestQuery, TModel model) where TModel : PaginationRequestModel;
        Task<(IQueryable<T> result, int totalCount, int totalPage)> ApplyPagination<T, TModel>(IQueryable<T> query, TModel model, string filter, string sort) where TModel : PaginationRequestModel;
    }
}
