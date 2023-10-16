using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class ProvinceCategoryReqModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public IFormFile Thumb { get; set; }
        [Required]
        [MaxLength(60)]
        public string ProvinceID { get; set; }
    }
}
