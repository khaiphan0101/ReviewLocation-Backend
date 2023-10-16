namespace review.Common.ResModels
{
    public class ApiResModel
    {
        public int? StatusCode { get; set; }

        public string? MessageText { get; set; }

        public object? Data { get; set; }

        public string? ErrorText { get; set; }
    }
}
