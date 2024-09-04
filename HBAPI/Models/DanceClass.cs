using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBAPI.Models
{
    [Table("Classes")]  // Maps this model to the "Classes" table in the database
    public class DanceClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }

        public bool MonthlyEligible { get; set; }

        [StringLength(45)]
        public string? Instructor { get; set; }

        [StringLength(45)]
        public string? Coupon { get; set; }
    }
}