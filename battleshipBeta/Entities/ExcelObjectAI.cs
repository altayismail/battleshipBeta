using System.ComponentModel.DataAnnotations;

namespace battleshipBeta.Entities
{
    public class ExcelObjectAI
    {
        [Key]
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Mode { get; set; }
        public double Duration { get; set; }
    }
}
