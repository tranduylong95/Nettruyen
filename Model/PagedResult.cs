using System.Collections.Generic;
namespace nettruyen.Model
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        // Tổng số bản ghi trong cơ sở dữ liệu
        public int TotalRecords { get; set; }

        // Số trang hiện tại
        public int PageNumber { get; set; }

        // Kích thước trang (số bản ghi mỗi trang)
        public int PageSize { get; set; }

        // Tổng số trang (tính từ tổng số bản ghi và kích thước trang)
        public int TotalPages { get; set; }

        // Tạo một PagedResult từ các tham số
        public PagedResult(IEnumerable<T> items, int totalRecords, int pageNumber, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        }
    }
}
