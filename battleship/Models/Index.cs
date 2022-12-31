using System.ComponentModel.DataAnnotations;

namespace battleship.Models
{
    public class CoordinateIndex
    {
        [Key]
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
