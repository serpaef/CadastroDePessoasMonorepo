namespace backend.Domain.DTO
{
    public class PagedResultsDto<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
