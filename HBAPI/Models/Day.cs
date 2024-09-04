using System.ComponentModel.DataAnnotations;

namespace HBAPI.Models
{
    public class Day
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DayName { get; set; } = String.Empty;

        // Navigation property to ClassDays
        public ICollection<ClassDay>? ClassDays { get; set; }
    }
}