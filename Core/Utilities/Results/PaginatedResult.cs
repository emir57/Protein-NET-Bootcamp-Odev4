using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class PaginatedResult<T> : IDataResult<T>
    {
        public PaginatedResult(T data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber == 0 ? 1 : pageNumber;
            PageSize = pageSize == 0 ? 10 : pageSize;

            Success = true;
            Data = data;
            Message = "Success pagination list";
        }
        public T Data { get; set; }
        public bool Success { get; }
        public string Message { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool NextPage { get; set; }
        public bool PreviousPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

    }
}
