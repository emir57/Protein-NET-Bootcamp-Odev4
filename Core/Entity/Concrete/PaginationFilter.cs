namespace Core.Entity.Concrete
{
    public class PaginationFilter
    {
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize, string cacheKey)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 1 ? 10 : pageSize;
            CacheKey = cacheKey;
        }
        public string CacheKey { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
