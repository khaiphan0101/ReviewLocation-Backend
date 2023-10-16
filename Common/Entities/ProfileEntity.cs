using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace review.Common.Entities
{
    [Table("Profile")]
    public class ProfileEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [Column(TypeName = "varchar(12)")]
        public string? Phone { get; set; }

        public int? Gender { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Avatar { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Identify { get; set; }

        public string AccountID { get; set; }

        public AccountEntity Account { get; set; }

        public ICollection<ProfileFollowEntity> Followers { get; set; }

        public ICollection<ProfileFollowEntity> Followings { get; set; }

    }
}
