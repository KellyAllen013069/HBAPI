namespace HBAPI.Models;

public class ClassSpecific
{
    public int Id { get; set; }  // Primary key
    public int ClassId { get; set; }
    public DanceClass DanceClass { get; set; }
    public DateTime Date { get; set; }
}
