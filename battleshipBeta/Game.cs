using battleshipBeta.Entities;

namespace battleshipBeta
{
    internal class Game
    {
        //In the game area, 0 means there is no ship, 1 means there is ship but it was not shooted
        //2 means there is a ship and it was shooted, 3 means there is no ship but it was fail shoot

        public (int, int) lastShootedIndex;
        public (int, int) shipFoundStartPoint;

        public bool isShipFound = false;
        public bool lastShotSuccess;
        public bool firstSuccessShotChecker = true;
        public string direction;

        public int roundCounter = 0;

        public Ship admiral = new Ship(5, "Admiral", 1);
        public Ship cruiser = new Ship(4, "Cruiser", 2);
        public Ship destroyer = new Ship(3, "Destroyer", 3);
        public Ship destroyer2 = new Ship(3, "Destroyer2", 4);
        public Ship assault = new Ship(2, "Assault", 5);

        public Ship userAdmiral = new Ship(5, "User Admiral", 1);
        public Ship userCruiser = new Ship(4, "User Cruiser", 2);
        public Ship userDestroyer = new Ship(3, "User Destroyer", 3);
        public Ship userDestroyer2 = new Ship(3, "User Destroyer2", 4);
        public Ship userAssault = new Ship(2, "User Assault", 5);

        private readonly Random _random;
        private readonly Logger _logger;

        public Game(Random random, Logger logger)
        {
            _random = random;
            _logger = logger;
        }

        public List<Ship> getListOfUserShip(Ship admiral, Ship Cruiser, Ship Destroyer, Ship Destroyer2, Ship Assault)
        {
            List<Ship> shipList = new List<Ship>();
            shipList.Add(admiral);
            shipList.Add(Cruiser);
            shipList.Add(Destroyer);
            shipList.Add(Destroyer2);
            shipList.Add(Assault);
            return shipList;
        }

        //creating game area
        public int[,] createGameArea()
        {
            int [,] gameArea = new int[10, 10];
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

        //check overlap and one square rule for horizontal placement
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
                        _logger.print("Placement is blocked by ONE SQUARE RULE!!!");
                        return true;
                    }
                }
            }
            return false;
        }
        //check overlap and one square rule for vertical placement
        public bool checkOneSquareRuleinVertical(int[,] gameArea, int startIndex, int endIndex, int locationIndex)
        {
            for (int i = locationIndex - 1; i <= locationIndex + 1; i++)
            {
                for (int j = startIndex - 1; j <= endIndex + 1; j++)
                {
                    if (i > 9 || i < 0 || j < 0 || j > 9)
                        continue;
                    if (gameArea[j, i] == 1)
                    {
                        _logger.print("Placement is blocked by ONE SQUARE RULE!!!");
                        return true;
                    }
                }
            }
            return false;
        }

        //print computer game area
        public void printComputerGameArea(int[,] computerGameArea)
        {
            Console.WriteLine("____________________________");
            Console.WriteLine("Computer Game Area");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (computerGameArea[j, i] == 0)
                        Console.Write("0 ");
                    else if (computerGameArea[j, i] == 1)
                        Console.Write("0 ");
                    else if (computerGameArea[j, i] == 3)
                        Console.Write("X ");
                    else
                        Console.Write("S ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("_________________________");
        }
        //print user game area
        public void printUserGameArea(int[,] userGameArea)
        {
            Console.WriteLine("*****************************");
            Console.WriteLine("User Game Area");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (userGameArea[j, i] == 0)
                        Console.Write("0 ");
                    else if (userGameArea[j, i] == 1)
                        Console.Write("* ");
                    else if (userGameArea[j, i] == 3)
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
            while (true)
            {
                Console.WriteLine("Please enter X coordinate!");
                _logger.inputer("X: ");
                var x_axis = Console.ReadLine();
                int X;

                //only number input validation
                if (isItParsable(x_axis))
                    X = int.Parse(x_axis);
                else
                {
                    _logger.printWarning("You have to enter a number!!!");
                    continue;
                }

                //check boundry of the game area for x
                if (X > 10 || X <= 0)
                {
                    _logger.printWarning("You enter a number out of the coordinate!!");
                    continue;
                }

                Console.WriteLine("Please enter Y coordinate!");
                _logger.inputer("Y: ");
                var y_axis = Console.ReadLine();
                int Y;

                //only number input validation
                if (isItParsable(y_axis))
                    Y = int.Parse(y_axis);
                else
                {
                    _logger.printWarning("You have to enter a number!!!");
                    continue;
                }

                //check boundry of the game area for y
                if (Y > 10 || Y <= 0)
                {
                    _logger.printWarning("You enter a number out of the coordinate!!");
                    continue;
                }

                //succesful shoot check
                if (gameArea[X - 1, Y - 1] == 1)
                {
                    gameArea[X - 1, Y - 1] = 2;
                    roundCounter++;
                    _logger.gamePrint("You succesfully shoot!!");
                    printComputerGameArea(gameArea);
                    continue;
                }
                //already shot check
                else if (gameArea[X - 1, Y - 1] == 2)
                {
                    printComputerGameArea(gameArea);
                    continue;
                }
                //already fail shot check
                else if (gameArea[X - 1, Y - 1] == 3)
                    break;
                //cross the fail shot
                else
                {
                    gameArea[X - 1, Y - 1] = 3;
                    roundCounter++;
                    break;
                }
            }
        }

        public (int, int) shootInRow(int X, int Y, int[,] userGameArea)
        {
            if(!(Y - 1 < 0))
            {
                if (userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 2 && userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 3)
                {
                    direction = "North";
                    return (getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2);
                }
            }
            if(!(Y + 1 > 10))
            {
                if (userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 2 && userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 3)
                {
                    direction = "South";
                    return (getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2);
                }
            }
            if(!(X + 1 > 10))
            {
                if (userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 2 && userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 3)
                {
                    direction = "East";
                    return (getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2);
                }
            }
            if(!(X - 1 < 0))
            {
                if (userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 2 && userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 3)
                {
                    direction = "West";
                    return (getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2);
                }
            }
            return (X, Y);
        }

        public void checkShipIsFullySinked(List<Ship> ships, int[,] gameArea)
        {
            foreach (var ship in ships.Where(x => x.isShipSinked == false).ToList<Ship>())
            {
                int shipSinkCounter = 0;
                for (int i = 0; i < ship.Length; i++)
                {
                    if (gameArea[ship.YLocations[i],ship.XLocations[i]] == 2)
                        shipSinkCounter++;
                }

                if (shipSinkCounter == ship.Length)
                {
                    isShipFound = false;
                    ship.isShipSinked = true;
                    direction = null;
                    lastShotSuccess = false;
                    firstSuccessShotChecker = true;
                }
            }
        }

        //computer shoot mechanism
        public void computerShoot(int[,] userGameArea, List<Ship> ships)
        {
            while(true)
            {
                int X;
                int Y;

                checkShipIsFullySinked(ships, userGameArea);

                if (isShipFound == true && lastShotSuccess == true && direction == null)
                    (X, Y) = shootInRow(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                else if (lastShotSuccess == false && isShipFound == true && direction == null)
                {
                    (X, Y) = shipFoundStartPoint;
                    (X, Y) = shootInRow(X, Y, userGameArea);
                }
                else if (isShipFound == false)
                {
                    X = _random.Next(10);
                    Y = _random.Next(10);
                }
                //after the first shoot, computer decides which way it should try
                else if (direction == "North")
                    (X, Y) = getNorthCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2);
                else if (direction == "South")
                    (X, Y) = getSouthCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2);
                else if (direction == "East")
                    (X, Y) = getEastCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2);
                else if (direction == "West")
                    (X, Y) = getWestCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2);
                else
                {
                    X = _random.Next(10);
                    Y = _random.Next(10);
                }

                //succesful shoot check
                if (userGameArea[X, Y] == 1)
                {
                    userGameArea[X, Y] = 2;
                    roundCounter++;
                    printUserGameArea(userGameArea);
                    lastShootedIndex = (X, Y);
                    lastShotSuccess = true;
                    isShipFound = true;

                    if(firstSuccessShotChecker == true)
                    {
                        shipFoundStartPoint = (X, Y);
                        firstSuccessShotChecker = false;
                    }
                    continue;
                }
                //already shot check
                else if (userGameArea[X, Y] == 2)
                {
                    printUserGameArea(userGameArea);
                    continue;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                    continue;
                //cross the fail shot
                else
                {
                    userGameArea[X , Y] = 3;
                    roundCounter++;
                    lastShotSuccess = false;
                    direction = null;
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

        public (string,string) nameAndSurnameInput()
        {
            _logger.inputer("First Name: ");
            string firstname = Console.ReadLine();
            _logger.inputer("Last Name: ");
            string lastname = Console.ReadLine();

            return (firstname, lastname);
        }

        public (int, int) getNorthCoordinate(int X, int Y)
        {
            return (X, Y - 1);
        }

        public (int, int) getSouthCoordinate(int X, int Y)
        {
            return (X, Y + 1);
        }

        public (int, int) getEastCoordinate(int X, int Y)
        {
            return (X + 1, Y);
        }

        public (int, int) getWestCoordinate(int X, int Y)
        {
            return (X - 1, Y);
        }
    }
}
