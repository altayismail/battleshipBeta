using battleshipBeta.Entities;
using System.ComponentModel.DataAnnotations;

namespace battleship.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public int UserRoundCounter { get; set; }
        public bool isTuttorialMode { get; set; }
        public string? AILevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public bool isWinner { get; set; }
        public bool isRandomPlacement { get; set; }
        public bool verOrHor { get; set; }
        public string? strVerOrHor { get; set; }
        public string? ShipChoice { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
    }
}
