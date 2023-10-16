namespace review.Common.ResModels
{
    public class PagingResModel : ApiResModel
    {
        public decimal TotalPage { get; set; }

        public decimal TotalData { get; set; }

        public int CurrentPage { get; set; }
    }
}
