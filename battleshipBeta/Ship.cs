namespace battleshipBeta
{
    internal class Ship
    {
        public int Length { get; set; }
        public string? Name { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public bool VerorHor { get; set; }
        public int LocationIndex { get; set; }

        public Ship(int length, string? name, int startIndex, int endIndex, bool verorHor, int locationIndex)
        {
            Length = length;
            Name = name;
            StartIndex = startIndex;
            EndIndex = endIndex;
            VerorHor = verorHor;
            LocationIndex = locationIndex;
        }
    }
}
