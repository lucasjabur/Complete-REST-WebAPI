using Microsoft.AspNetCore.Mvc.Filters;

namespace REST_WebAPI.Hypermedia.Abstract {
    public interface IResponseEnricher {
        bool CanEnrich(ResultExecutingContext context);
        Task Enrich(ResultExecutingContext context);
    }
}
