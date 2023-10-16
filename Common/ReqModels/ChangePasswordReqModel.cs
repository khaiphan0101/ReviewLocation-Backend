using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class ChangePasswordReqModel
    {
        [Required]
        [MaxLength(60)]
        [MinLength(8)]

        public string OldPassword { get; set; }
        [Required]
        [MaxLength(60)]
        [MinLength(8)]
        public string NewPassword { get; set; }
        [Required]
        [MaxLength(60)]
        [MinLength(8)]
        public string ConfirmPassword { get; set; }

    }
}
