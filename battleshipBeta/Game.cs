using battleshipBeta.Entities;

namespace battleshipBeta
{
    internal class Game
    {
        //In the game area, 0 means there is no ship, 1 means there is ship but it was not shooted
        //2 means there is a ship and it was shooted, 3 means there is no ship but it was fail shoot
        //verorhor variable means false is vertical placement, true is horizontal placement

        public (int, int) lastShootedIndex;
        public (int, int) shipFoundStartPoint;

        public bool isShipFound = false;
        public bool lastShotSuccess;
        public bool firstSuccessShotChecker = true;
        public string direction;

        public int computerRoundCounter = 1;
        public int userRoundCounter = 1;

        public Ship computerAdmiral = new Ship(5, "Computer Admiral", 1);
        public Ship computerCruiser = new Ship(4, "Computer Cruiser", 2);
        public Ship computerDestroyer = new Ship(3, "Computer Destroyer", 3);
        public Ship comptuerDestroyer2 = new Ship(3, "Computer Destroyer2", 4);
        public Ship computerAssault = new Ship(2, "Computer Assault", 5);

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
                            userGameArea[j, i] = 4;
                        if (i == ship.LocationIndex && j == ship.StartIndex - 1)
                            userGameArea[j, i] = 4;
                        else if(i == ship.LocationIndex && j == ship.EndIndex)
                            userGameArea[j, i] = 4;
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
                            userGameArea[i, j] = 4;
                        if (i == ship.LocationIndex && j == ship.StartIndex - 1)
                            userGameArea[i, j] = 4;
                        else if (i == ship.LocationIndex && j == ship.EndIndex)
                            userGameArea[i, j] = 4;
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
                checkShipIsFullySinked(computerShips, computerGameArea);
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
                    userRoundCounter++;
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
            if(!(Y - 1 < 0))
            {
                if (userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 2 && userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 3 && userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 4)
                {
                    direction = "North";
                    return (getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2);
                }
            }
            if(!(Y + 1 > 9))
            {
                if (userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 2 && userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 3 && userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 4)
                {
                    direction = "South";
                    return (getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2);
                }
            }
            if(!(X + 1 > 9))
            {
                if (userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 2 && userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 3 && userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 4 )
                {
                    direction = "East";
                    return (getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2);
                }
            }
            if(!(X - 1 < 0))
            {
                if (userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 2 && userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 3 && userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 4)
                {
                    direction = "West";
                    return (getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2);
                }
            }
            return (X, Y);
        }

        //this mechanism checks every round, if there is a sinked ship
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
                    _logger.gamePrint($"{ship.Name} is sinked!!");
                    recognizeOneSqureRule(gameArea, ship);
                }
            }
        }

        //computer hard level shoot mechanism
        public void computerHardLevelShoot(int[,] userGameArea, List<Ship> userShips)
        {
            while(true)
            {
                int X;
                int Y;

                checkShipIsFullySinked(userShips, userGameArea);

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

                if(X == null || Y == null)
                {
                    X = _random.Next(10);
                    Y = _random.Next(10);
                }

                //succesful shoot check
                if (userGameArea[X, Y] == 1)
                {
                    userGameArea[X, Y] = 2;
                    computerRoundCounter++;
                    printUserGameArea(userGameArea);
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
                    printUserGameArea(userGameArea);
                    continue;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                    continue;
                else if (userGameArea[X, Y] == 4)
                    continue;
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

        //computer hard level shoot mechanism
        public void computerMediumLevelShoot(int[,] userGameArea, List<Ship> userShips)
        {
            while (true)
            {
                int X;
                int Y;

                checkShipIsFullySinked(userShips, userGameArea);

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
                    computerRoundCounter++;
                    printUserGameArea(userGameArea);
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
                    printUserGameArea(userGameArea);
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
        public void computerEasyLevelShoot(int[,] userGameArea)
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
                    computerRoundCounter++;
                    printUserGameArea(userGameArea);
                    _logger.gamePrint("Computer succesfully shooted!!!");
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
                    userGameArea[X, Y] = 3;
                    computerRoundCounter++;
                    break;
                }
            }
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
                    else if (userGameArea[j, i] == 4)
                        Console.Write("- ");
                    else
                        Console.Write("S ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("_________________________");
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

        //these functions are return the coordinate index according to the direction
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

        //Following 4 function checks if the game is end or not
        public bool checkUserWin(int[,] computerGameArea, string firstname, string lastname, bool isUserWinner)
        {
            if (checkAllShipsAreFound(computerGameArea) == 17)
            {
                _logger.gamePrint("Congrats, you found all the ships!!!");
                _logger.gamePrint($"Winner: {firstname} {lastname}");
                Console.WriteLine("Last Situation \n___________________________________________");
                printComputerGameArea(computerGameArea);
                printUserGameArea(computerGameArea);
                isUserWinner = true;
                return true;
            }
            return false;
        }

        public bool checkComputerWin(int[,] userGameArea, bool isUserWinner)
        {
            if (checkAllShipsAreFound(userGameArea) == 17)
            {
                _logger.gamePrint("Nice try, Computer found all the ships!!!");
                Console.WriteLine("Last Situation \n___________________________________________");
                printComputerGameArea(userGameArea);
                printUserGameArea(userGameArea);
                isUserWinner = false;
                return true;
            }
            return false;
        }

        //public bool checkComputerHoundredRound(int[,] userGameArea, bool isUserWinner)
        //{
        //    if (computerRoundCounter == 100)
        //    {
        //        _logger.gamePrint("Nice try, Computer found all the ships!!!");
        //        Console.WriteLine("Last Situation \n___________________________________________");
        //        printComputerGameArea(userGameArea);
        //        printUserGameArea(userGameArea);
        //        isUserWinner == false;
        //        return true;
        //    }
        //    return false;
        //}

        //public bool checkUserHoundredRound(int[,] computerGameArea, string firstname, string lastname, bool isUserWinner)
        //{
        //    if (userRoundCounter == 100)
        //    {
        //        _logger.gamePrint("Congrats, you found all the ships!!!");
        //        _logger.gamePrint($"Winner: {firstname} {lastname}");
        //        Console.WriteLine("Last Situation \n___________________________________________");
        //        printComputerGameArea(computerGameArea);
        //        printUserGameArea(computerGameArea);
        //        isUserWinner = true;
        //        return true;
        //    }
        //    return false;
        //}
    }
}
