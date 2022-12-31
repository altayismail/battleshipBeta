namespace battleship.DataAccess
{
    public class GameSettings
    {
        public static Random _random = new Random();
        public static bool isUserTurn { get; set; } = true;

        public static (int, int) lastShootedIndex { get; set; }
        public static (int, int) shipFoundStartPoint { get; set; }

        public static bool isShipFound { get; set; } = false;
        public static bool lastShotSuccess { get; set; } = false;
        public static bool firstSuccessShotChecker { get; set; } = true;
        public static bool threeAreaChecker { get; set; } = false;
        public static string? direction { get; set; }

        public static int randomActivater { get; set; } = 0;
        public static string? ShipInfo { get; set; }
    }
}
