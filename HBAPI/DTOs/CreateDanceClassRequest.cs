using HBAPI.DTOs;

namespace HBAPI.DTOs
{
    public class CreateDanceClassRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public string Instructor { get; set; } = string.Empty; 
        public string StartTime { get; set; } = "00:00:00"; 
        public string EndTime { get; set; } = "00:00:00"; 
        public string StartDate { get; set; } = "0001-01-01"; 
        public string EndDate { get; set; } = "0001-01-01"; 
        public decimal Price { get; set; }
        public string Coupon { get; set; } = string.Empty; 
        public bool MonthlyEligible { get; set; }
        public List<ClassDayDto> ClassDays { get; set; } = new List<ClassDayDto>(); 
        public List<ClassLevelDto> ClassLevels { get; set; } = new List<ClassLevelDto>(); 
    }
}