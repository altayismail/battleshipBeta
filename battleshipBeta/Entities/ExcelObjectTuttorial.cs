using System.ComponentModel.DataAnnotations;

namespace battleshipBeta.Entities
{
    public class ExcelObjectTuttorial
    {
        [Key]
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public double Duration { get; set; }
        public DateTime PlayedTime { get; set; }
    }
}
