namespace MyHostAPI.Common.Helpers
{
    public class Pagination
    {
        private int _pageIndex;

        public int PageIndex
        {
            get { return _pageIndex > 0 ? _pageIndex - 1 : 0; }
            set { _pageIndex = value; }
        }

        public int PageSize { get; set; } = 20;
    }
}

