using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class UserProfileReqModel   
    {
        [MaxLength(60)]
        [MinLength(4)]
        public string? Name { get; set; }

        public string? Phone { get; set; }

        [Range(0,1)]
        public int? Gender { get; set; }

        public IFormFile? Avatar { get; set; }
                 
    }
}

