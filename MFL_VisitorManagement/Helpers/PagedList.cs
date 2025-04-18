using Microsoft.EntityFrameworkCore;

namespace MFL_VisitorManagement.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(IEnumerable<T> items, int count, int pageSize, int pageNumber)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count; 
            AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IEnumerable<T> source, int pageNumber, int PageSize)
        {
            var count =  source.Count();
            var items =  source.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();
            var data =  new PagedList<T>(items, count, PageSize, pageNumber);
            return data;
        } 
    }

}
