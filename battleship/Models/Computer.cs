using battleshipBeta.Entities;
using System.ComponentModel.DataAnnotations;

namespace battleship.Models
{
    public class Computer
    {
        [Key]
        public int Id { get; set; }
        public bool isWinner { get; set; }
        public int ComputerRoundCounter { get; set; }
    }
}
