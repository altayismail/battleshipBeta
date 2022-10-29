namespace battleshipBeta
{
    internal class Placement
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
                _logger.print($"Random indexing is done for {ship.Name}");

                ship.StartIndex = index.Item2;
                ship.EndIndex = index.Item3;
                ship.LocationIndex = index.Item1;

                var verorhor = _game.randomIndexing(10);
                if (verorhor.Item1 < 5)
                {
                    _logger.print($"{ship.Name} is placed verticaly.");
                    ship.VerorHor = true;
                }
                else
                {
                    _logger.print($"{ship.Name} is placed horizontaly.");
                    ship.VerorHor = false;
                }

                //check vertical or horizontal placement
                if (ship.VerorHor == true)
                {
                    if (_game.checkOneSquareRuleinHorizontal(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.horizontalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    break;
                }
                else
                {
                    if (_game.checkOneSquareRuleinVertical(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.verticalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    break;
                }
            }
        }

        public void placementMechanismForUser(int[,] gameArea, List<Ship> ships)
        {
            while (true)
            {
                //User ship choose mechanism start
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
                var shipId = Console.ReadLine();
                if (int.Parse(shipId) < 1 || int.Parse(shipId) > 6 || string.IsNullOrEmpty(shipId))
                    continue;
                var ship = ships.Where(x => x.Id.ToString() == shipId).Single();
                if (ship.isShipPlaced == true)
                {
                    Console.WriteLine("You have already placed the ship!!!");
                    continue;
                }
                Console.WriteLine("********************************");
                //User ship choose mechanism end
                //Horizontal and Vertical placement choice start
                Console.WriteLine("Do you want to place horizontaly or verticaly\n1. Vertical\n2. Horizontal");
                Console.Write("Choice: ");
                var verorhor = Console.ReadLine();
                if (!_game.isItParsable(verorhor))
                {
                    Console.WriteLine("Invalid input!!");
                    continue;
                }

                if (verorhor == "1")
                {
                    ship.VerorHor = true;
                    _logger.print($"{ship.Name} is vertical.");
                    Console.WriteLine("Please enter X start coordinate!");
                    Console.Write("X: ");
                    var x_axis = Console.ReadLine();
                    int X;

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
                    int Y;

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
                    ship.LocationIndex = X-1;
                    ship.StartIndex = Y-1;
                }
                else if (verorhor == "2")
                {
                    ship.VerorHor = false;
                    _logger.print($"{ship.Name} is horizontal.");
                    Console.WriteLine("Please enter X coordinate!");
                    Console.Write("X: ");
                    var x_axis = Console.ReadLine();
                    int X;

                    //only number input validation
                    if (int.TryParse(x_axis, out X))
                    {
                        X = int.Parse(x_axis);
                    }
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
                    Console.WriteLine("Please enter Y coordinate!");
                    Console.Write("Y: ");
                    var y_axis = Console.ReadLine();
                    int Y;

                    //only number input validation
                    if (int.TryParse(y_axis, out Y))
                    {
                        Y = int.Parse(y_axis);
                    }
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
                    ship.LocationIndex = Y - 1;
                    ship.StartIndex = X - 1;
                }

                ship.EndIndex = ship.StartIndex + ship.Length;
                if (ship.EndIndex >= 10 || ship.EndIndex < 0)
                {
                    Console.WriteLine($"You choose wrong start index for ship, the length of the ship is {ship.Length}. Please try again.");
                    continue;
                }
                //Horizontal and Vertical placement choice start

                _logger.print($"Random indexing is done for {ship.Name}");

                //check vertical or horizontal placement
                if (ship.VerorHor == true)
                {
                    if (_game.checkOneSquareRuleinHorizontal(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.horizontalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    ship.isShipPlaced = true;
                    break;
                }
                else
                {
                    if (_game.checkOneSquareRuleinVertical(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                        continue;
                    _game.verticalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                    ship.isShipPlaced = true;
                    break;
                }
            }
            _game.printUserGameArea(gameArea);
        }
    }
}
