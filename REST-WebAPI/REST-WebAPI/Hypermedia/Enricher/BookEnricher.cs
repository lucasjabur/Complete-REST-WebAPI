using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Hypermedia.Constants;

namespace REST_WebAPI.Hypermedia.Enricher {
    public class BookEnricher : ContentResponseEnricher<BookDTO> {
        protected override Task EnrichModel(BookDTO content, IUrlHelper urlHelper) {
            var request = urlHelper.ActionContext.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}/api/book/v1";
            content.Links.AddRange(GenerateLinks(content.Id, baseUrl));
            return Task.CompletedTask;
        }

        private IEnumerable<HypermediaLink> GenerateLinks(long id, string baseUrl) {
            //return new List<HypermediaLink> {
            return [
            // new HypermediaLink
                new() {
                    Rel = RelationType.COLLECTION,
                    Href = $"{baseUrl}",
                    Type = ResponseTypeFormat.DefaultGet,
                    Action = HttpActionVerb.GET,
                },
                new() {
                    Rel = RelationType.SELF,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.DefaultGet,
                    Action = HttpActionVerb.GET,
                },
                new() {
                    Rel = RelationType.CREATE,
                    Href = $"{baseUrl}",
                    Type = ResponseTypeFormat.DefaultPost,
                    Action = HttpActionVerb.POST,
                },
                new() {
                    Rel = RelationType.UPDATE,
                    Href = $"{baseUrl}",
                    Type = ResponseTypeFormat.DefaultPut,
                    Action = HttpActionVerb.PUT,
                },
                new() {
                    Rel = RelationType.DELETE,
                    Href = $"{baseUrl}/{id}",
                    Type = ResponseTypeFormat.DefaultDelete,
                    Action = HttpActionVerb.DELETE,
                },
            ];
            // };
        }
    }
}
