using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstEFAPI.Models
{
    public class Standard
    {

        [Key]
        [Column("StandardId", TypeName = "int")]
        public int ID { get; set; }

        [Column("StandardName",TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Column("StandardDescription", TypeName = "varchar(100)")]
        public string Description { get; set; } = string.Empty;
    }
}
