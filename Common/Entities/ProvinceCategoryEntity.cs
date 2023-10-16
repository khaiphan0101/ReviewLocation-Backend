using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace review.Common.Entities
{
    [Table("ProvinceCategory")]
    public class ProvinceCategoryEntity
    {
        [Key]
        [Column(TypeName = "varchar(60)")]
        public string ID { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(500)")]

        public string Thumb { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string ProvinceID { get; set; }
        public ProvinceEntity Province { get; set; }

    }
}
