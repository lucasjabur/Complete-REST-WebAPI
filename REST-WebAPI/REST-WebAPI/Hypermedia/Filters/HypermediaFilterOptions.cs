using REST_WebAPI.Hypermedia.Abstract;

namespace REST_WebAPI.Hypermedia.Filters {
    public class HypermediaFilterOptions {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = [];
    }
}
