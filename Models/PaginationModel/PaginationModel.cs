namespace Models.PaginationModel
{
    public class PaginationModel
    {
        private int _pageSize = 10;

        private int _pageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value > 0) ? value : 1;
        }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 0 && value <= 500) ? value : _pageSize;
        }
    }
    public class PaginationRequestModel : PaginationModel
    {
        public string Query { get; set; }
        public string Filters { get; set; }
        public string Sorts { get; set; }
    }
}

