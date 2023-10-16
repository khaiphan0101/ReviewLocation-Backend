using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class SignUpReqModel
    {
        [Required]
        [MaxLength(60)]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(60)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
