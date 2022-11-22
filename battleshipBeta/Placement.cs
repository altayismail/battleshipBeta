using battleshipBeta.Entities;

namespace battleshipBeta
{
    public class Placement
    {
        private readonly Game _game;
        private readonly Logger _logger;
        private readonly Random _random;

        public Placement(Game game, Logger logger, Random random)
        {
            _game = game;
            _logger = logger;
            _random = random;
        }
        //dynamic placement mechanism
        public void placementMechanism(int[,] gameArea, Ship ship)
        {
            while (true)
            {
                var index = _game.randomIndexing(ship.Length);

                ship.StartIndex = index.Item2;
                ship.EndIndex = index.Item3;
                ship.LocationIndex = index.Item1;

                var verorhor = _game.randomIndexing(10);
                if (verorhor.Item1 < 5)
                    ship.VerorHor = true;
                else
                    ship.VerorHor = false;

                

                //check vertical or horizontal placement
                if (ship.VerorHor == true)
                {
                    if (_game.checkOneSquareRuleinHorizontal(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.horizontalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    setShipLocation(ship);
                    break;
                }
                else
                {
                    if (_game.checkOneSquareRuleinVertical(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.verticalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    setShipLocation(ship);
                    break;
                }
            }
        }

        //Place all the computer ship with a func
        public void placeAllComputerShip(int[,] computerGameArea, List<Ship> ships)
        {
            foreach (var ship in ships)
            {
                placementMechanism(computerGameArea, ship);
            }
        }

        public void placementMechanismForUser(int[,] gameArea, List<Ship> ships, string username, string lastname)
        {
            while (true)
            {
                //User ship choose mechanism start
                int shipId = int.Parse(chooseShip(ships));
                var ship = ships.Where(x => x.Id == shipId).Single();

                if (ship.isShipPlaced == true)
                {
                    Console.WriteLine("You have already placed the ship!!!");
                    continue;
                }
                Console.WriteLine("********************************");
                //User ship choose mechanism end

                //Horizontal and Vertical placement choice start
                string verorhor = VerorHorChoice();

                if (verorhor == "1")
                {
                    ship.VerorHor = true;

                    (ship.LocationIndex, ship.StartIndex) = takeStartCoordinates();
                }
                else if (verorhor == "2")
                {
                    ship.VerorHor = false;

                    (ship.StartIndex, ship.LocationIndex) = takeStartCoordinates();
                }

                ship.EndIndex = ship.StartIndex + ship.Length;
                if (ship.EndIndex >= 11 || ship.EndIndex <= -1)
                {
                    Console.WriteLine($"You choose wrong start index for ship, the length of the ship is {ship.Length}. Please try again.");
                    continue;
                }
                //Horizontal and Vertical placement choice end
                //check vertical or horizontal placement
                if (ship.VerorHor == true)
                {
                    if (_game.checkOneSquareRuleinHorizontal(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.horizontalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    ship.isShipPlaced = true;
                    setShipLocation(ship);
                    break;
                }
                else
                {
                    if (_game.checkOneSquareRuleinVertical(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.verticalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    ship.isShipPlaced = true;
                    setShipLocation(ship);
                    break;
                }
            }
            _game.printUserGameArea(gameArea, username, lastname);
        }

        //Place all the user ship with a function
        public void placeAllUserShip(int[,] userGameArea, List<Ship> ships)
        {
            foreach (var ship in ships)
            {
                placementMechanism(userGameArea, ship);
            }
        }

        public string chooseShip(List<Ship> ships)
        {
            string shipId;
            while(true)
            {
                Console.WriteLine("Choose the ship you want to place");
                int i = 1;
                foreach (Ship item in ships)
                {
                    if (item.isShipPlaced == true)
                    {
                        Console.WriteLine(i + ". " + item.Name + " X");
                    }
                    else
                        Console.WriteLine(i + ". " + item.Name);
                    i++;
                }
                Console.Write("Choice: ");
                shipId = Console.ReadLine();
                if (int.Parse(shipId) < 1 || int.Parse(shipId) > 6 || string.IsNullOrEmpty(shipId))
                    continue;
                break;
            }
            return shipId;
        }

        public string VerorHorChoice()
        {
            string verorhor;
            while(true)
            {
                Console.WriteLine("Do you want to place horizontaly or verticaly\n1. Vertical\n2. Horizontal");
                Console.Write("Choice: ");
                verorhor = Console.ReadLine();
                if (_game.isItParsable(verorhor))
                {
                    if (int.Parse(verorhor) == 1 || int.Parse(verorhor) == 2)
                        break;
                    else
                    {
                        Console.WriteLine("Invalid input!!");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input!!");
                    continue;
                }
            }
            return verorhor;
        }

        public (int,int) takeStartCoordinates()
        {
            int X;
            int Y;
            while (true)
            {
                Console.WriteLine("Please enter X start coordinate!");
                Console.Write("X: ");
                var x_axis = Console.ReadLine();

                //only number input validation
                if (_game.isItParsable(x_axis))
                    X = int.Parse(x_axis);
                else
                {
                    Console.WriteLine("You have to enter a number!!!");
                    continue;
                }

                //check boundry of the game area for x
                if (X > 10 || X <= 0)
                {
                    Console.WriteLine("You enter a number out of the coordinate!!");
                    continue;
                }
                Console.WriteLine("Please enter Y start coordinate!");
                Console.Write("Y: ");
                var y_axis = Console.ReadLine();

                //only number input validation
                if (_game.isItParsable(y_axis))
                    Y = int.Parse(y_axis);
                else
                {
                    Console.WriteLine("You have to enter a number!!!");
                    continue;
                }

                //check boundry of the game area for y
                if (Y > 10 || Y <= 0)
                {
                    Console.WriteLine("You enter a number out of the coordinate!!");
                    continue;
                }
                break;
            }
            return (X - 1, Y - 1);
        }

        public void setShipLocation(Ship ship)
        {
            if (ship.VerorHor == false)
            {
                for (int i = ship.StartIndex; i < ship.EndIndex; i++)
                {
                    ship.XLocations.Add(ship.LocationIndex);
                    ship.YLocations.Add(i);
                }
            }
            else
            {
                for (int i = ship.StartIndex; i < ship.EndIndex; i++)
                {
                    ship.XLocations.Add(i);
                    ship.YLocations.Add(ship.LocationIndex);
                }
            }
        }
    }
}
