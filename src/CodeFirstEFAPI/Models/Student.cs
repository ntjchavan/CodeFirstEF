using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstEFAPI.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public int Age { get; set; }

        public int StandardId { get; set; }

        public string? Remark { get; set; }
    }
}
