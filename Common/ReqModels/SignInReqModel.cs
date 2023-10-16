using System.ComponentModel.DataAnnotations;

namespace review.Common.ReqModels
{
    public class SignInReqModel
    {
        [Required]
        [MaxLength(60)]
        [MinLength(6)]       
        public string UserName { get; set; }

        [Required]
        [MaxLength(60)]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
