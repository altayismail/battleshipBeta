using battleshipBeta.Entities;

namespace battleship.DataAccess
{
    public static class Shipper
    {
        public static Ship computerAircraft = new Ship(5, "Computer Aircraft Carrier", 1);
        public static Ship computerBattleship = new Ship(4, "Computer Battleship", 2);
        public static Ship computerCruiser = new Ship(3, "Computer Cruiser", 3);
        public static Ship computerSubmarine = new Ship(3, "Computer Submarine", 4);
        public static Ship computerDestroyer = new Ship(2, "Computer Destroyer", 5);

        public static Ship userAircraft = new Ship(5, "User Aircraft Carrier", 1);
        public static Ship userBattleship = new Ship(4, "User Battleship", 2);
        public static Ship userCruiser = new Ship(3, "User Cruiser", 3);
        public static Ship userSubmarine = new Ship(3, "User Submarine", 4);
        public static Ship userDestroyer = new Ship(2, "User Destroyer", 5);
    }
}
