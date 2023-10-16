using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace review.Common.Entities
{
    [Table("ProfileFollow")]
    public class ProfileFollowEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string FollowingID { get; set; }
        
        [Column(TypeName = "varchar(60)")]
        public string FollowerID { get; set; }
        

        public ProfileEntity Following { get; set; }

        public ProfileEntity Follower { get; set; }

    }
}
