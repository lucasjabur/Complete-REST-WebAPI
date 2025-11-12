namespace REST_WebAPI.Repositories.QueryBuilders {
    public class BookQueryBuilder {
        public (string query, string countQuery, string sort, int size, int offset)
            BuildQueries(string title, string sortDirection, int pageSize, int page) {
            page = Math.Max(1, page);
            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? pageSize = 1 : pageSize;
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "asc" : "desc";

            var whereClause = $" FROM dbo.books b WHERE 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(title)) {
                whereClause += $" AND (b.title LIKE '%{title}%') ";
            }

            var query = $@"
                SELECT * {whereClause}
                ORDER BY b.title {sort}
                OFFSET {offset} ROWS
                FETCH NEXT {size} ROWS ONLY";

            var countQuery = $"SELECT COUNT(*) {whereClause}";

            return (query, countQuery, sort, size, offset);
        }
    }
}
