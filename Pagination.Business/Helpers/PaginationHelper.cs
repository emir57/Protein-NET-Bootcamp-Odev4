using Core.Entity.Concrete;
using Core.Utilities.Results;

namespace Pagination.Business.Helpers
{
    public static class PaginationHelper
    {
        public static PaginatedResult<IEnumerable<T>> CreatePaginatedResponse<T>(IEnumerable<T> data, PaginationFilter paginationFilter, int totalRecords, bool skip = true)
        {
            if (skip)
                data = data.Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize);
            int roundedTotalPages;
            var response =
                new PaginatedResult<IEnumerable<T>>(data, paginationFilter.PageNumber, paginationFilter.PageSize);
            var totalPages = totalRecords / (double)paginationFilter.PageSize;
            if (paginationFilter.PageNumber <= 0 || paginationFilter.PageSize <= 0)
            {
                roundedTotalPages = 1;
                paginationFilter.PageSize = 1;
                paginationFilter.PageNumber = 1;
            }
            else
                roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            response.NextPage = paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < roundedTotalPages ?
                true : false;
            response.PreviousPage = paginationFilter.PageNumber - 1 >= 1 && paginationFilter.PageNumber <= roundedTotalPages ?
                true : false;
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = roundedTotalPages * paginationFilter.PageSize;
            return response;
        }
    }
}
