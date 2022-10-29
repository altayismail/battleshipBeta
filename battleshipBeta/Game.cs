namespace battleshipBeta
{
    internal class Game
    {
        public int[,] gameArea;
        public int roundCounter = 0;

        private readonly Random _random;
        private readonly Logger _logger;

        public Game(Random random, Logger logger)
        {
            _random = random;
            _logger = logger;
        }

        //creating game area
        public int[,] createGameArea()
        {
            gameArea = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameArea[j, i] = 0;
                }
            }
            _logger.print("Game area created...");
            return gameArea;
        }

        public bool checkOneSquareRuleinHorizontal(int[,] gameArea, int startIndex, int endIndex, int locationIndex)
        {
            for (int i = locationIndex - 1; i <= locationIndex + 1; i++)
            {
                for (int j = startIndex - 1; j <= endIndex + 1; j++)
                {
                    if (i > 9 || i < 0 || j < 0 || j > 9)
                        continue;
                    if (gameArea[i, j] == 1)
                    {
                        Console.WriteLine("Placement is blocked by ONE SQUARE RULE!!!");
                        return true;
                    }
                }
            }
            return false;
        }

        public bool checkOneSquareRuleinVertical(int[,] gameArea, int startIndex, int endIndex, int locationIndex)
        {
            for (int i = locationIndex-1; i <= locationIndex+1 ; i++)
            {
                for (int j = startIndex-1; j <= endIndex+1; j++)
                {
                    if (i > 9 || i < 0 || j < 0 || j > 9)
                        continue;
                    if (gameArea[j, i] == 1)
                    {
                        Console.WriteLine("Placement is blocked by ONE SQUARE RULE!!!");
                        return true;
                    }    
                }
            }
            return false;
        }

        //print game area
        public void printComputerGameArea(int[,] gameArea)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (gameArea[j, i] == 0)
                        Console.Write("0 ");
                    else if (gameArea[j, i] == 1)
                        Console.Write("0 ");
                    else if (gameArea[j, i] == 3)
                        Console.Write("X ");
                    else
                        Console.Write("S ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("_________________________");
        }

        public void printUserGameArea(int[,] userGameArea)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (gameArea[j, i] == 0)
                        Console.Write("0 ");
                    else if (gameArea[j, i] == 1)
                        Console.Write("* ");
                    else if (gameArea[j, i] == 3)
                        Console.Write("X ");
                    else
                        Console.Write("S ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("_________________________");
        }

        //user shoot mechanism
        public void shoot(int[,] gameArea)
        {
            while(true)
            {
                Console.WriteLine("Please enter X coordinate!");
                Console.Write("X: ");
                var x_axis = Console.ReadLine();
                int X;

                //only number input validation
                if(isItParsable(x_axis))
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

                Console.WriteLine("Please enter Y coordinate!");
                Console.Write("Y: ");
                var y_axis = Console.ReadLine();
                int Y;

                //only number input validation
                if (isItParsable(y_axis))
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

                //succesful shoot check
                if (gameArea[X-1, Y-1] == 1)
                {
                    gameArea[X-1, Y-1] = 2;
                    roundCounter++;
                    Console.WriteLine("You succesfully shoot!!");
                    break;
                }
                //already shot check
                else if (gameArea[X-1, Y-1] == 2)
                {
                    continue;
                }
                //already fail shot check
                else if (gameArea[X-1, Y-1] == 3)
                    break;
                //cross the fail shot
                else
                {
                    gameArea[X-1, Y-1] = 3;
                    roundCounter++;
                    break;
                } 
            }
        }

        public void computerShoot(int[,] gameArea)
        {
            while(true)
            {
                var X = _random.Next(10);
                var Y = _random.Next(10);
                //succesful shoot check
                if (gameArea[X, Y] == 1)
                {
                    gameArea[X, Y] = 2;
                    roundCounter++;
                    break;
                }
                //already shot check
                else if (gameArea[X, Y] == 2)
                {
                    continue;
                }
                //already fail shot check
                else if (gameArea[X, Y] == 3)
                    break;
                //cross the fail shot
                else
                {
                    gameArea[X , Y] = 3;
                    roundCounter++;
                    break;
                }
            }
        }

        public int checkAllShipsAreFound(int[,] gameArea)
        {
            List<int> oneDimension = new List<int>();
            foreach (var a in gameArea)
            {
                oneDimension.Add(a);
            }
            return oneDimension.Count(x => x.Equals(2));
        }

        public void horizontalPlacement(int[,] gameArea, int startIndex, int endIndex, int locationIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                gameArea[locationIndex, i] = 1;
            }
        }

        public void verticalPlacement(int[,] gameArea, int startIndex, int endIndex, int locationIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                gameArea[i, locationIndex] = 1;
            }
        }

        public (int,int,int) randomIndexing(int shipLength)
        {
            var locationIndex = _random.Next(10);
            var startIndex = _random.Next(10);
            var endIndex = startIndex + shipLength;

            if(endIndex > 10)
            {
                int tempIndex = startIndex;
                startIndex -= shipLength;
                endIndex = tempIndex;
            }
            return (locationIndex,startIndex,endIndex);
        }

        public bool isItParsable(string number)
        {
            int checker;
            if (int.TryParse(number, out checker))
                return true;
            return false;
        }

        ////dynamic placement mechanism
        //public void placementMechanism(int[,] gameArea, Ship ship)
        //{
        //    while (true)
        //    {
        //        var index = randomIndexing(ship.Length);
        //        _logger.print($"Random indexing is done for {ship.Name}");

        //        ship.StartIndex = index.Item2;
        //        ship.EndIndex = index.Item3;
        //        ship.LocationIndex = index.Item1;

        //        var verorhor = randomIndexing(10);
        //        if(verorhor.Item1 < 5)
        //        {
        //            _logger.print($"{ship.Name} is placed verticaly.");
        //            ship.VerorHor = true;
        //        }
        //        else
        //        {
        //            _logger.print($"{ship.Name} is placed horizontaly.");
        //            ship.VerorHor = false;
        //        }

        //        //check vertical or horizontal placement
        //        if (ship.VerorHor == true)
        //        {
        //            if (checkOneSquareRuleinHorizontal(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
        //                continue;
        //            horizontalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
        //            break;
        //        }   
        //        else
        //        {
        //            if (checkOneSquareRuleinVertical(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
        //                continue;
        //            verticalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
        //            break;
        //        }     
        //    }
        //}

        //public void placementMechanismForUser(int[,] gameArea, List<Ship> ships)
        //{
        //    while (true)
        //    {
        //        //User ship choose mechanism start
        //        Console.WriteLine("Choose the ship you want to place");
        //        int i = 1;
        //        foreach (Ship item in ships)
        //        {
        //            if(item.isShipPlaced == true)
        //            {
        //                Console.WriteLine(i + ". " + item.Name + " X");
        //            }
        //            else
        //                Console.WriteLine(i + ". " + item.Name);
        //            i++;
        //        }
        //        Console.WriteLine("Choice: ");
        //        var shipId = Console.ReadLine();
        //        if (int.Parse(shipId) < 1 || int.Parse(shipId) > 6)
        //            continue;
        //        var ship = ships.Where(x => x.Id.ToString() == shipId).Single();
        //        if (ship.isShipPlaced == true)
        //        {
        //            Console.WriteLine("You have already placed the ship!!!");
        //            continue;
        //        }
        //        //User ship choose mechanism end
        //        //Horizontal and Vertical placement choice start
        //        Console.WriteLine("Do you want to place horizontaly or verticaly\n1. Vertical\n2. Horizontal");
        //        Console.Write("Choice: ");
        //        var verorhor = Console.ReadLine();
        //        if (!isItParsable(verorhor))
        //        {
        //            Console.WriteLine("Invalid input!!");
        //            continue;
        //        }

        //        if (verorhor == "1")
        //        {
        //            ship.VerorHor = false;
        //            _logger.print($"{ship.Name} is vertical.");
        //            Console.WriteLine("Please enter X coordinate!");
        //            Console.Write("X: ");
        //            var x_axis = Console.ReadLine();
        //            int X;

        //            //only number input validation
        //            if (isItParsable(x_axis))
        //                X = int.Parse(x_axis);
        //            else
        //            {
        //                Console.WriteLine("You have to enter a number!!!");
        //                continue;
        //            }

        //            //check boundry of the game area for x
        //            if (X > 10 || X <= 0)
        //            {
        //                Console.WriteLine("You enter a number out of the coordinate!!");
        //                continue;
        //            }
        //            Console.WriteLine("Please enter Y coordinate!");
        //            Console.Write("Y: ");
        //            var y_axis = Console.ReadLine();
        //            int Y;

        //            //only number input validation
        //            if (isItParsable(y_axis))
        //                Y = int.Parse(y_axis);
        //            else
        //            {
        //                Console.WriteLine("You have to enter a number!!!");
        //                continue;
        //            }

        //            //check boundry of the game area for y
        //            if (Y > 10 || Y <= 0)
        //            {
        //                Console.WriteLine("You enter a number out of the coordinate!!");
        //                continue;
        //            }
        //            ship.LocationIndex = Y-1;
        //            ship.StartIndex = X-1;
        //        } 
        //        else if(verorhor == "2")
        //        {
        //            ship.VerorHor = true;
        //            _logger.print($"{ship.Name} is horizontal.");
        //            Console.WriteLine("Please enter X coordinate!");
        //            Console.Write("X: ");
        //            var x_axis = Console.ReadLine();
        //            int X;

        //            //only number input validation
        //            if (int.TryParse(x_axis, out X))
        //            {
        //                X = int.Parse(x_axis);
        //            }
        //            else
        //            {
        //                Console.WriteLine("You have to enter a number!!!");
        //                continue;
        //            }

        //            //check boundry of the game area for x
        //            if (X > 10 || X <= 0)
        //            {
        //                Console.WriteLine("You enter a number out of the coordinate!!");
        //                continue;
        //            }
        //            Console.WriteLine("Please enter Y coordinate!");
        //            Console.Write("Y: ");
        //            var y_axis = Console.ReadLine();
        //            int Y;

        //            //only number input validation
        //            if (int.TryParse(y_axis, out Y))
        //            {
        //                Y = int.Parse(y_axis);
        //            }
        //            else
        //            {
        //                Console.WriteLine("You have to enter a number!!!");
        //                continue;
        //            }

        //            //check boundry of the game area for y
        //            if (Y > 10 || Y <= 0)
        //            {
        //                Console.WriteLine("You enter a number out of the coordinate!!");
        //                continue;
        //            }
        //            ship.LocationIndex = X-1;
        //            ship.StartIndex = Y-1;
        //        }

        //        ship.EndIndex = ship.StartIndex + ship.Length;
        //        if (ship.EndIndex >= 10 || ship.EndIndex < 0)
        //        {
        //            Console.WriteLine($"You choose wrong start index for ship, the length of the ship is {ship.Length}. Please try again.");
        //            continue;
        //        }
        //        //Horizontal and Vertical placement choice start

        //        _logger.print($"Random indexing is done for {ship.Name}");

        //        //check vertical or horizontal placement
        //        if (ship.VerorHor == true)
        //        {
        //            if (checkOneSquareRuleinHorizontal(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
        //                continue;
        //            horizontalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
        //            ship.isShipPlaced = true;
        //            break;
        //        }
        //        else
        //        {
        //            if (checkOneSquareRuleinVertical(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
        //                continue;
        //            verticalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
        //            ship.isShipPlaced = true;
        //            break;
        //        }
        //    }
        //    printUserGameArea(gameArea);
        //}
    }
}
