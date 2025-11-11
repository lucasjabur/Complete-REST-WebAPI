namespace REST_WebAPI.Hypermedia.Abstract {
    public interface ISupportsHypermedia {
        List<HypermediaLink> Links { get; set; }
    }
}
