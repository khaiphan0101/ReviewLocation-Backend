using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace review.Common.Entities
{
    [Table("RefeshToken")]
    public class RefeshTokenEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string RefeshToken { get; set; }

        public DateTime ExpiredAt { get; set; }

        public string AccountID { get; set; }

        public AccountEntity Account { get; set; }
    }
}
