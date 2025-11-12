using REST_WebAPI.Hypermedia.Abstract;
using System.Xml.Serialization;

namespace REST_WebAPI.Hypermedia.Utils {
    public class PagedSearchDTO<T> where T : ISupportsHypermedia {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
        public string SortDirections { get; set; } = "asc";
        public string SortFields { get; set; }

        [XmlIgnore]
        public Dictionary<string, object> Filters { get; set; } = [];
        public List<T> List { get; set; } = [];
        public PagedSearchDTO() { }

        public PagedSearchDTO(int currentPage, int pageSize, string sortDirections, string sortFields, Dictionary<string, object> filters) {
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortDirections = sortDirections;
            SortFields = sortFields;
            Filters = filters ?? [];
        }

        public PagedSearchDTO(int currentPage, string sortDirections, string sortFields)
            : this(currentPage, 10, sortDirections, sortFields, null) { }

        public int GetCurrentPage() {
            return CurrentPage <= 0 ? 1 : CurrentPage;
        }

        public int GetPageSize() {
            return PageSize <= 0 ? 10 : PageSize;
        }
    }
}
