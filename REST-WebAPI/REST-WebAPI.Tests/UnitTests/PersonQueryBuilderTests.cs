using FluentAssertions;
using REST_WebAPI.Repositories.QueryBuilders;

namespace REST_WebAPI.Tests.UnitTests {
    public class PersonQueryBuilderTests {
        private readonly PersonQueryBuilder _queryBuilder;

        public PersonQueryBuilderTests() {
            _queryBuilder = new PersonQueryBuilder();
        }

        [Fact]
        public void BuildQueries_ShouldReturnCorrectQueries() {
            // Arrange
            string name = "John";
            string sortDirection = "asc";
            int pageSize = 10;
            int page = 2;

            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);

            // Assert
            query.Should().NotBeNull();
            countQuery.Should().NotBeNull();

            query.Should().Contain("SELECT");
            query.Should().Contain("FROM dbo.person");
            query.Should().Contain("WHERE");
            query.Should().Contain("John");
            query.Should().Contain("ORDER BY");
            query.Should().Contain("OFFSET");
            query.Should().Contain("FETCH NEXT");

            offset.Should().Be(10);
            size.Should().Be(10);
            sort.Should().Be("asc");

            countQuery.Should().Contain("SELECT COUNT(*)");
            countQuery.Should().Contain("FROM dbo.person");
            countQuery.Should().Contain("WHERE");
            countQuery.Should().Contain("John");
        }

        [Fact]
        public void BuildQueries_ShouldHandleInvalidPageAndPageSize() {
            var name = "";
            var sortDirection = "desc";
            var pageSize = 0;
            var page = -1;

            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);

            query.Should().NotBeNull();
            countQuery.Should().NotBeNull();

            query.Should().Contain("SELECT");
            query.Should().Contain("FROM dbo.person");
            query.Should().Contain("WHERE");
            query.Should().Contain("ORDER BY");
            query.Should().Contain("OFFSET");
            query.Should().Contain("FETCH NEXT");

            offset.Should().Be(0);
            size.Should().Be(1);
            sort.Should().Be("desc");

            countQuery.Should().Contain("SELECT COUNT(*)");
            countQuery.Should().Contain("FROM dbo.person");
            countQuery.Should().Contain("WHERE");
        }

        [Fact]
        public void BuildQueries_ShouldHandleNullOrWhitespaceName() {
            var name = "   ";
            var sortDirection = "asc";
            var pageSize = 5;
            var page = 1;
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);

            query.Should().NotBeNull();
            countQuery.Should().NotBeNull();

            query.Should().Contain("SELECT");
            query.Should().Contain("FROM dbo.person");
            query.Should().Contain("WHERE");
            query.Should().NotContain("LIKE");
            query.Should().Contain("ORDER BY");
            query.Should().Contain("OFFSET");
            query.Should().Contain("FETCH NEXT");

            offset.Should().Be(0);
            size.Should().Be(5);
            sort.Should().Be("asc");

            countQuery.Should().Contain("SELECT COUNT(*)");
            countQuery.Should().Contain("FROM dbo.person");
            countQuery.Should().Contain("WHERE");
            countQuery.Should().NotContain("LIKE");
        }

        [Fact]
        public void BuildQueries_ShouldDefaultToDescForInvalidSortDirection() {
            var name = "Jane";
            var sortDirection = "invalid";
            var pageSize = 10;
            var page = 1;

            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(name, sortDirection, pageSize, page);

            query.Should().NotBeNull();
            countQuery.Should().NotBeNull();

            query.Should().Contain("SELECT");
            query.Should().Contain("FROM dbo.person");
            query.Should().Contain("WHERE");
            query.Should().Contain("Jane");
            query.Should().Contain("ORDER BY");
            query.Should().Contain("OFFSET");
            query.Should().Contain("FETCH NEXT");

            offset.Should().Be(0);
            size.Should().Be(10);
            sort.Should().Be("asc");

            countQuery.Should().Contain("SELECT COUNT(*)");
            countQuery.Should().Contain("FROM dbo.person");
            countQuery.Should().Contain("WHERE");
            countQuery.Should().Contain("Jane");
        }
    }
}
