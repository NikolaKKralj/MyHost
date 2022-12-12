namespace MyHostAPI.Common.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public PaginatedListMetadata Metadata { get; set; } = new();
        public PaginatedList(List<T> items, PaginatedListMetadata paginatedListMetadata)
        {
            this.Metadata = paginatedListMetadata;
            this.AddRange(items);
        }

        public PaginatedList()
        {

        }
    }

    public class PaginatedListMetadata
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public long TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}

