using battleshipBeta.Entities;

namespace battleshipBeta
{
    public class Game
    {
        //In the game area, 0 means there is no ship, no shoot, 1 means there is ship but it was not shooted
        //2 means there is a ship and it was shooted, 3 means there is no ship but it was fail shoot
        //verorhor variable means false is vertical placement, true is horizontal placement

        public int[,] getPerfectProbability(List<Ship> ships, int[,] userGameArea)
        {
            int[,] perfectProbability = new int[10, 10];
            foreach (var ship in ships.Where(x => x.isShipSinked == false))
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        for (int k = 0; k < ship.Length; k++)
                        {
                            if (j + (ship.Length - 2) >= 9)
                                break;
                            if (userGameArea[i, j] == 2 || userGameArea[i, j] == 3 || userGameArea[i, j] == 4)
                            {
                                perfectProbability[i, j] = 0;
                                int temp = k;
                                for (k = 0; k < temp; k++)
                                {
                                    perfectProbability[i, j + k] -= 1;
                                }
                                break;
                            }
                            perfectProbability[i, j + k] += 1;
                        }
                    }
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        for (int k = 0; k < ship.Length; k++)
                        {
                            if (j + (ship.Length - 2) >= 9)
                                break;
                            if (userGameArea[j, i] == 2 ||  userGameArea[j, i] == 3 || userGameArea[j, i] == 4)
                            {
                                perfectProbability[j, i] = 0;
                                int temp = k;
                                for (k = 0; k < temp; k++)
                                {
                                    perfectProbability[j + k, i] -= 1;
                                }
                                break;
                            }
                            perfectProbability[j + k, i] += 1;
                        }
                    }
                }
            }
            return perfectProbability;
        }

        List<(int, int)> edgeIndexs = new List<(int, int)>
        {
            (0, 0), (2, 0), (4, 0), (6, 0), (8, 0),
            (0, 2), (0, 4), (0, 6), (0, 8),
            (9, 2), (9, 4), (9, 6), (9, 8),
            (2, 9), (4, 9), (6, 9), (8, 9)
        };

        public (int, int) lastShootedIndex;
        public (int, int) shipFoundStartPoint;

        public bool isShipFound = false;
        public bool lastShotSuccess = false;
        public bool firstSuccessShotChecker = true;
        public string direction;

        public int computerRoundCounter = 1;
        public int userRoundCounter = 1;
        public int randomActivater = 0;

        public Ship computerAdmiral = new Ship(5, "Computer Aircraft Carrier", 1);
        public Ship computerCruiser = new Ship(4, "Computer Battleship", 2);
        public Ship computerDestroyer = new Ship(3, "Computer Cruiser", 3);
        public Ship comptuerDestroyer2 = new Ship(3, "Computer Submarine", 4);
        public Ship computerAssault = new Ship(2, "Computer Destroyer", 5);

        public Ship userAdmiral = new Ship(5, "User Aircraft Carrier", 1);
        public Ship userCruiser = new Ship(4, "User Battleship", 2);
        public Ship userDestroyer = new Ship(3, "User Cruiser", 3);
        public Ship userDestroyer2 = new Ship(3, "User Submarine", 4);
        public Ship userAssault = new Ship(2, "User Destroyer", 5);

        private readonly Random _random;
        private readonly Logger _logger;

        public Game(Random random, Logger logger)
        {
            _random = random;
            _logger = logger;
        }

        //get list of user ships
        public List<Ship> getListOfComputerShip(Ship admiral, Ship Cruiser, Ship Destroyer, Ship Destroyer2, Ship Assault)
        {
            List<Ship> shipListComputer = new List<Ship>();
            shipListComputer.Add(admiral);
            shipListComputer.Add(Cruiser);
            shipListComputer.Add(Destroyer);
            shipListComputer.Add(Destroyer2);
            shipListComputer.Add(Assault);
            return shipListComputer;
        }

        //get list of user ships
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
                        return true;
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
                        return true;
                }
            }
            return false;
        }

        //computer recognize that it should not shoot one squared index
        public void recognizeOneSqureRule(int[,] userGameArea, Ship ship)
        {
            if(ship.VerorHor == false)
            {
                for (int i = ship.LocationIndex - 1; i <= ship.LocationIndex + 1; i++)
                {
                    for (int j = ship.StartIndex - 1; j <= ship.EndIndex; j++)
                    {
                        if (i > 9 || i < 0 || j < 0 || j > 9)
                            continue;
                        if(i == ship.LocationIndex - 1 || i == ship.LocationIndex + 1)
                        {
                            userGameArea[j, i] = 4;
                        }
                        if (i == ship.LocationIndex && j == ship.StartIndex - 1)
                        {
                            userGameArea[j, i] = 4;
                        }
                        else if(i == ship.LocationIndex && j == ship.EndIndex)
                        {
                            userGameArea[j, i] = 4;
                        }
                    }
                }
            }
            else if(ship.VerorHor == true)
            {
                for (int i = ship.LocationIndex - 1; i <= ship.LocationIndex + 1; i++)
                {
                    for (int j = ship.StartIndex - 1; j <= ship.EndIndex; j++)
                    {
                        if (i > 9 || i < 0 || j < 0 || j > 9)
                            continue;
                        if (i == ship.LocationIndex - 1 || i == ship.LocationIndex + 1)
                        {
                            userGameArea[i, j] = 4;
                        }
                            
                        if (i == ship.LocationIndex && j == ship.StartIndex - 1)
                        {
                            userGameArea[i, j] = 4;
                        }
                        else if (i == ship.LocationIndex && j == ship.EndIndex)
                        {
                            userGameArea[i, j] = 4;
                        }  
                    }
                }
            }
        }

        //this function place the ships to game are in horizontal
        public void horizontalPlacement(int[,] gameArea, int startIndex, int endIndex, int locationIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                gameArea[locationIndex, i] = 1;
            }
        }

        //this function place the ships to game are in vertical
        public void verticalPlacement(int[,] gameArea, int startIndex, int endIndex, int locationIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                gameArea[i, locationIndex] = 1;
            }
        }

        //user shoot mechanism
        public void userShoot(int[,] computerGameArea, List<Ship> computerShips)
        {
            while (true)
            {
                checkShipIsFullySinkedforUser(computerShips, computerGameArea);
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
                if (computerGameArea[X - 1, Y - 1] == 1)
                {
                    computerGameArea[X - 1, Y - 1] = 2;
                    _logger.gamePrint("You succesfully shoot!!");
                    printComputerGameArea(computerGameArea);
                    continue;
                }
                //already shot check
                else if (computerGameArea[X - 1, Y - 1] == 2)
                {
                    printComputerGameArea(computerGameArea);
                    continue;
                }
                //already fail shot check
                else if (computerGameArea[X - 1, Y - 1] == 3)
                    break;
                //cross the fail shot
                else
                {
                    computerGameArea[X - 1, Y - 1] = 3;
                    userRoundCounter++;
                    break;
                }
            }
        }

        //this function is focus on when it founds a success shot and try to find other piece of ship
        public (int, int) shootInRow(int X, int Y, int[,] userGameArea)
        {
            if(!(Y - 1 <= 0))
            {
                if (userGameArea[getNorthCoordinate(X, Y, userGameArea).Item1, getNorthCoordinate(X, Y, userGameArea).Item2] != 2 && userGameArea[getNorthCoordinate(X, Y, userGameArea).Item1, getNorthCoordinate(X, Y, userGameArea).Item2] != 3 && userGameArea[getNorthCoordinate(X, Y, userGameArea).Item1, getNorthCoordinate(X, Y, userGameArea).Item2] != 4)
                {
                    direction = "North";
                    return (getNorthCoordinate(X, Y, userGameArea).Item1, getNorthCoordinate(X, Y, userGameArea).Item2);
                }
            }
            if(!(Y + 1 >= 10))
            {
                if (userGameArea[getSouthCoordinate(X, Y, userGameArea).Item1, getSouthCoordinate(X, Y, userGameArea).Item2] != 2 && userGameArea[getSouthCoordinate(X, Y, userGameArea).Item1, getSouthCoordinate(X, Y, userGameArea).Item2] != 3 && userGameArea[getSouthCoordinate(X, Y, userGameArea).Item1, getSouthCoordinate(X, Y, userGameArea).Item2] != 4)
                {
                    direction = "South";
                    return (getSouthCoordinate(X, Y, userGameArea).Item1, getSouthCoordinate(X, Y, userGameArea).Item2);
                }
            }
            if(!(X + 1 >= 10))
            {
                if (userGameArea[getEastCoordinate(X, Y, userGameArea).Item1, getEastCoordinate(X, Y, userGameArea).Item2] != 2 && userGameArea[getEastCoordinate(X, Y, userGameArea).Item1, getEastCoordinate(X, Y, userGameArea).Item2] != 3 && userGameArea[getEastCoordinate(X, Y, userGameArea).Item1, getEastCoordinate(X, Y, userGameArea).Item2] != 4 )
                {
                    direction = "East";
                    return (getEastCoordinate(X, Y, userGameArea).Item1, getEastCoordinate(X, Y, userGameArea).Item2);
                }
            }
            if(!(X - 1 <= 0))
            {
                if (userGameArea[getWestCoordinate(X, Y, userGameArea).Item1, getWestCoordinate(X, Y, userGameArea).Item2] != 2 && userGameArea[getWestCoordinate(X, Y, userGameArea).Item1, getWestCoordinate(X, Y, userGameArea).Item2] != 3 && userGameArea[getWestCoordinate(X, Y, userGameArea).Item1, getWestCoordinate(X, Y, userGameArea).Item2] != 4)
                {
                    direction = "West";
                    return (getWestCoordinate(X, Y, userGameArea).Item1, getWestCoordinate(X, Y, userGameArea).Item2);
                }
            }
            return (X, Y);
        }

        //computer hard level shoot mechanism
        public void computerHardLevelShoot(int[,] userGameArea, List<Ship> userShips, string firstname, string lastname)
        {
            while(true)
            {
                int X;
                int Y;

                checkShipIsFullySinkedforComputer(userShips, userGameArea);

                if (isShipFound == true && lastShotSuccess == true && direction == null)
                {
                    (X, Y) = shootInRow(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                }
                else if (isShipFound == true && lastShotSuccess == false && direction == null)
                {
                    (X, Y) = shipFoundStartPoint;
                    (X, Y) = shootInRow(X, Y, userGameArea);
                }
                else if (isShipFound == false)
                {
                    (X, Y) = randomShootDecider(userGameArea, userShips);
                }
                //after the second success shot, computer finds the ship's other pieces
                else if (direction == "North")
                {
                    (X, Y) = getNorthCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                }
                else if (direction == "South")
                {
                    (X, Y) = getSouthCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                }
                else if (direction == "East")
                {
                    (X, Y) = getEastCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                }
                else if (direction == "West")
                {
                    (X, Y) = getWestCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                }
                else
                {
                    (X, Y) = randomShootDecider(userGameArea, userShips);
                }

                //succesful shoot check
                if (userGameArea[X, Y] == 1)
                {
                    userGameArea[X, Y] = 2;
                    printUserGameArea(userGameArea, firstname, lastname);
                    randomActivater = 0;
                    lastShootedIndex = (X, Y);

                    lastShotSuccess = true;
                    isShipFound = true;
                    

                    if(firstSuccessShotChecker == true)
                    {
                        shipFoundStartPoint = (X, Y);
                        firstSuccessShotChecker = false;
                    }

                    _logger.gamePrint("Computer succesfully shooted!!!");
                    continue;
                }
                //already shot check
                else if (userGameArea[X, Y] == 2)
                {
                    printUserGameArea(userGameArea, firstname, lastname);
                    _logger.printWarning("You are in loop because of game area is 2!!!");
                    continue;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                {
                    _logger.printWarning("You are in a loop because of game area is 3");
                    continue;
                }
                else if (userGameArea[X, Y] == 4)
                {
                    _logger.printWarning("You are in loop where game area is 4!!");
                    continue;
                }
                //cross the fail shot
                else
                {
                    userGameArea[X , Y] = 3;
                    computerRoundCounter++;
                    lastShotSuccess = false;
                    direction = null;
                    break;
                }
            }
        }

        //these functions are return the coordinate index according to the direction
        public (int, int) getNorthCoordinate(int X, int Y, int[,] userGameArea)
        {
            if(Y - 1 < 0 && userGameArea[X,Y] == 3 && userGameArea[X, Y] == 4)
            {
                direction = "South";
                return (shipFoundStartPoint.Item1 + 1, shipFoundStartPoint.Item2);
            }  
            return (X, Y - 1);
        }

        public (int, int) getSouthCoordinate(int X, int Y, int[,] userGameArea)
        {
            if (Y + 1 > 9 && userGameArea[X, Y] == 3 && userGameArea[X, Y] == 4)
            {
                direction = "North";
                return (shipFoundStartPoint.Item1 -1, shipFoundStartPoint.Item2);
            }
            return (X, Y + 1);
        }

        public (int, int) getEastCoordinate(int X, int Y, int[,] userGameArea)
        {
            if (X + 1 > 9 && userGameArea[X, Y] == 3 && userGameArea[X, Y] == 4)
            {
                direction = "West";
                return (shipFoundStartPoint.Item1, shipFoundStartPoint.Item2 - 1);
            }
            return (X + 1, Y);
        }

        public (int, int) getWestCoordinate(int X, int Y, int[,] userGameArea)
        {
            if (X - 1 < 0 && userGameArea[X, Y] == 3 && userGameArea[X, Y] == 4)
            {
                direction = "East";
                return (shipFoundStartPoint.Item1, shipFoundStartPoint.Item2 + 1);
            }
            return (X - 1, Y);
        }

        //computer hard level shoot mechanism
        public void computerMediumLevelShoot(int[,] userGameArea, List<Ship> userShips, string firstname, string lastname)
        {
            while (true)
            {
                int X;
                int Y;

                checkShipIsFullySinkedforComputer(userShips, userGameArea);

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
                    (X, Y) = getNorthCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                else if (direction == "South")
                    (X, Y) = getSouthCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                else if (direction == "East")
                    (X, Y) = getEastCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                else if (direction == "West")
                    (X, Y) = getWestCoordinate(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                else
                {
                    X = _random.Next(10);
                    Y = _random.Next(10);
                }

                //succesful shoot check
                if (userGameArea[X, Y] == 1)
                {
                    userGameArea[X, Y] = 2;
                    printUserGameArea(userGameArea, firstname, lastname);
                    lastShootedIndex = (X, Y);
                    lastShotSuccess = true;
                    isShipFound = true;

                    if (firstSuccessShotChecker == true)
                    {
                        shipFoundStartPoint = (X, Y);
                        firstSuccessShotChecker = false;
                    }
                    _logger.gamePrint("Computer succesfully shooted!!!");
                    continue;
                }
                //already shot check
                else if (userGameArea[X, Y] == 2)
                {
                    printUserGameArea(userGameArea, firstname, lastname);
                    continue;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                    continue;
                //cross the fail shot
                else
                {
                    userGameArea[X, Y] = 3;
                    computerRoundCounter++;
                    lastShotSuccess = false;
                    direction = null;
                    break;
                }
            }
        }

        //computer easy level shoot mechanism
        public void computerEasyLevelShoot(int[,] userGameArea, string firstname, string lastname)
        {
            while (true)
            {
                int X;
                int Y;

                X = _random.Next(10);
                Y = _random.Next(10);

                //succesful shoot check
                if (userGameArea[X, Y] == 1)
                {
                    userGameArea[X, Y] = 2;
                    printUserGameArea(userGameArea, firstname, lastname);
                    _logger.gamePrint("Computer succesfully shooted!!!");
                    continue;
                }
                //already shot check
                else if (userGameArea[X, Y] == 2)
                {
                    printUserGameArea(userGameArea, firstname, lastname);
                    continue;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                    continue;
                //cross the fail shot
                else
                {
                    userGameArea[X, Y] = 3;
                    computerRoundCounter++;
                    break;
                }
            }
        }

        //This is a decider when computer randomly shoots
        public (int, int) randomShootDecider(int[,] userGameArea, List<Ship> userShips)
        {
            int X;
            int Y;

            if (computerRoundCounter > 4)
            {
                int maxProbabilty = getPerfectProbability(userShips, userGameArea).Cast<int>().Max();
                (X, Y) = CoordinatesOf(getPerfectProbability(userShips, userGameArea), maxProbabilty);
            }
            else
            {
                X = _random.Next(10);
                Y = _random.Next(10);
            }
            return (X, Y);
        }

        // This is method for computer try to found ships on Edges
        public (int ,int) tryToFindShipsOnEdges(int[,] userGameArea)
        {
            int X;
            int Y;

            while (true)
            {
                int index = _random.Next(edgeIndexs.Count);
                (X, Y) = edgeIndexs[index];

                if (userGameArea[X, Y] != 2 && userGameArea[X, Y] != 3 && userGameArea[X, Y] != 4)
                {
                    edgeIndexs.RemoveAt(index);
                    return (X, Y);
                }
                else if (edgeIndexs.Count == 0)
                    break;
                else
                {
                    edgeIndexs.RemoveAt(index);
                    continue;
                }
            }

            while (true)
            {
                (X, Y) = (_random.Next(10), _random.Next(10));
                if (userGameArea[X, Y] != 2 && userGameArea[X, Y] != 3 && userGameArea[X, Y] != 4)
                    return (X, Y);
                else
                    continue;
            }
        }

        //When there is a ship that has not sinked yet, AI does not shoot where the last ship cannot be
        public (int, int) whenAShipLeft(int[,] userGameArea, Ship userShip)
        {
            int X;
            int Y;

            for (int i = 0; i < 10; i++)
            {
                int repeatedArea = 0;
                for (int j = 0; j < 10; j++)
                {
                    if (repeatedArea == userShip.Length)
                        return (j, i);
                    else if (userGameArea[j, i] == 0)
                        repeatedArea++;
                    else
                        repeatedArea = 0;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                int repeatedArea = 0;
                for (int j = 0; j < 10; j++)
                {
                    if (repeatedArea == userShip.Length)
                        return (i, j);
                    else if (userGameArea[j, i] == 0)
                        repeatedArea++;
                    else
                        repeatedArea = 0;
                }
            }

            while (true)
            {
                (X, Y) = (_random.Next(10), _random.Next(10));
                if (userGameArea[X, Y] != 2 && userGameArea[X, Y] != 3 && userGameArea[X, Y] != 4)
                    return (X, Y);
                else
                    continue;
            }
        }

        //print computer game area
        public void printComputerGameArea(int[,] computerGameArea)
        {
            Console.WriteLine("____________________________");
            Console.WriteLine("Computer Game Area");
            Console.WriteLine("  | 1 2 3 4 5 6 7 8 9 10");
            Console.WriteLine("_________________________");
            for (int i = 0; i < 10; i++)
            {
                if (i + 1 == 10)
                    Console.Write((i + 1) + "| ");
                else
                    Console.Write((i + 1) + " | ");
                for (int j = 0; j < 10; j++)
                {
                    if (computerGameArea[j, i] == 0)
                        Console.Write("O ");
                    else if (computerGameArea[j, i] == 1)
                        Console.Write("O ");
                    else if (computerGameArea[j, i] == 3)
                        Console.Write("X ");
                    else if (computerGameArea[j, i] == 4)
                        Console.Write("- ");
                    else
                        Console.Write("S ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("_________________________");
        }

        //print user game area
        public void printUserGameArea(int[,] userGameArea, string username, string userlastname)
        {
            Console.WriteLine("*****************************");
            Console.WriteLine($"{username} {userlastname}'s Game Area");
            Console.WriteLine("  | 1 2 3 4 5 6 7 8 9 10");
            Console.WriteLine("_________________________");
            for (int i = 0; i < 10; i++)
            {
                if(i + 1 == 10)
                    Console.Write((i + 1) + "| ");
                else
                    Console.Write((i + 1) + " | ");
                for (int j = 0; j < 10; j++)
                {
                    if (userGameArea[j, i] == 0)
                        Console.Write("O ");
                    else if (userGameArea[j, i] == 1)
                        Console.Write("* ");
                    else if (userGameArea[j, i] == 3)
                        Console.Write("X ");
                    else if (userGameArea[j, i] == 4)
                        Console.Write("- ");
                    else
                        Console.Write("S ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("*****************************");
        }

        //this function checks if all ships are found and help to end game
        public int checkAllShipsAreFound(int[,] gameArea)
        {
            List<int> oneDimension = new List<int>();
            foreach (var a in gameArea)
            {
                oneDimension.Add(a);
            }
            return oneDimension.Count(x => x.Equals(2));
        }

        public bool checkAllShipsAreFound(List<Ship> ships)
        {
            if(ships.All(x => x.isShipSinked == true))
                return true;
            return false;
        }

        //this is a helper function for srandom ship placement
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

        //this function checks if the value is parsable so that app should not crash
        public bool isItParsable(string number)
        {
            int checker;
            if (int.TryParse(number, out checker))
                return true;
            return false;
        }

        //there are couple of time to take this input
        public (string,string) nameAndSurnameInput()
        {
            string firstname;
            string lastname;
            while (true)
            {
                _logger.inputer("First Name: ");
                firstname = Console.ReadLine();

                if (string.IsNullOrEmpty(firstname))
                {
                    _logger.printWarning("Firstname cannot be empty!!");
                    continue;
                }
                    
                _logger.inputer("Last Name: ");
                lastname = Console.ReadLine();

                if (string.IsNullOrEmpty(lastname))
                {
                    _logger.printWarning("Lastname cannot be empty!!");
                    continue;
                }
                break;
            }

            return (firstname, lastname);
        }

        //Following 2 function checks if the game is end or not
        public (bool, bool) checkUserWin(int[,] computerGameArea, int[,] userGameArea, string firstname, string lastname, bool isUserWinner)
        {
            if (checkAllShipsAreFound(computerGameArea) == 17)
            {
                _logger.gamePrint($"Congrats, you found all the ships in {userRoundCounter} round!!!");
                _logger.gamePrint($"Winner: {firstname} {lastname}");
                Console.WriteLine("Last Situation \n___________________________________________");
                printComputerGameArea(computerGameArea);
                printUserGameArea(userGameArea, firstname, lastname);
                isUserWinner = true;
                return (true, isUserWinner);
            }
            return (false, isUserWinner);
        }

        public (bool, bool) checkComputerWin(int[,] userGameArea, int[,] computerGameArea, bool isUserWinner, string firstname, string lastname)
        {
            if (checkAllShipsAreFound(userGameArea) == 17)
            {
                _logger.gamePrint($"Nice try, Computer found all the ships in {computerRoundCounter} round!!!");
                Console.WriteLine("Last Situation \n___________________________________________");
                printComputerGameArea(computerGameArea);
                printUserGameArea(userGameArea, firstname, lastname);
                isUserWinner = false;
                return (true, isUserWinner);
            }
            return (false, isUserWinner);
        }

        //Returns cordinate of a value in 2D array
        public (int, int) CoordinatesOf(int[,] matrix, int value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return (x, y);
                }
            }
            return (-1, -1);
        }

        //this mechanism checks every round, if there is a sinked ship
        public void checkShipIsFullySinkedforComputer(List<Ship> ships, int[,] gameArea)
        {
            foreach (var ship in ships.Where(x => x.isShipSinked == false).ToList<Ship>())
            {
                int shipSinkCounter = 0;
                for (int i = 0; i < ship.Length; i++)
                {
                    if (gameArea[ship.YLocations[i], ship.XLocations[i]] == 2)
                        shipSinkCounter++;
                }

                if (shipSinkCounter == ship.Length)
                {
                    isShipFound = false;
                    ship.isShipSinked = true;
                    direction = null;
                    lastShotSuccess = false;
                    firstSuccessShotChecker = true;
                    _logger.gamePrint($"{ship.Name} is sinked!!");
                    recognizeOneSqureRule(gameArea, ship);
                }
            }
        }

        public void checkShipIsFullySinkedforUser(List<Ship> ships, int[,] gameArea)
        {
            foreach (var ship in ships.Where(x => x.isShipSinked == false).ToList<Ship>())
            {
                int shipSinkCounter = 0;
                for (int i = 0; i < ship.Length; i++)
                {
                    if (gameArea[ship.YLocations[i], ship.XLocations[i]] == 2)
                        shipSinkCounter++;
                }

                if (shipSinkCounter == ship.Length)
                {
                    ship.isShipSinked = true;
                    _logger.gamePrint($"{ship.Name} is sinked!!");
                    recognizeOneSqureRule(gameArea, ship);
                }
            }
        }
    }
}
