namespace REST_WebAPI.Models {
    public class PagedSearch<T>{
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
        public string SortDirections { get; set; } = "asc";
        public List<T> List { get; set; }

    }
}
