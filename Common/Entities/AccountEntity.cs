using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace review.Common.Entities
{
    [Table("Account")]
    public class AccountEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string UserName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Role { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        public ProfileEntity Profile { get; set; }

        public ICollection<RefeshTokenEntity> RefeshTokens { get; set; }
    }
}
