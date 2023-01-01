using battleshipBeta.Entities;

namespace battleshipBeta
{
    public class Game
    {
        //In the game area, 0 means there is no ship, no shoot, 1 means there is ship but it was not shooted
        //2 means there is a ship and it was shooted, 3 means there is no ship but it was fail shoot
        //verorhor variable means false is vertical placement, true is horizontal placement

        public (int, int) lastShootedIndex;
        public (int, int) shipFoundStartPoint;

        public bool isShipFound = false;
        public bool lastShotSuccess = false;
        public bool firstSuccessShotChecker = true;
        public bool threeAreaChecker = false;
        public string direction;

        public int randomActivater = 0;

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

        //user shoot mechanism
        public void userShoot(int[,] computerGameArea, List<Ship> computerShips)
        {
            while (true)
            {
                checkShipIsFullySinkedforUser(computerShips, computerGameArea);

                if (checkAllShipsAreFound(computerShips))
                    break;
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
                    break;
                }
            }
        }

        //user shoot mechanism for .NET MVC
        public (string, bool) userShoot(int[,] computerGameArea, List<Ship> computerShips, int X, int Y)
        {
            bool isUserTurn = false;
            while (true)
            {
                checkShipIsFullySinkedforUser(computerShips, computerGameArea);

                if (checkAllShipsAreFound(computerShips))
                    break;

                //succesful shoot check
                if (computerGameArea[X, Y] == 1)
                {
                    isUserTurn = true;
                    computerGameArea[X, Y] = 2;
                    _logger.gamePrint("You succesfully shoot!!");
                    break;
                }
                //already shot check
                else if (computerGameArea[X, Y] == 2)
                {
                    break;
                }
                //already fail shot check
                else if (computerGameArea[X, Y] == 3)
                    break;
                //cross the fail shot
                else
                {
                    computerGameArea[X, Y] = 3;
                    break;
                }
            }
            return (checkShipIsFullySinkedforUserMVC(computerShips, computerGameArea), isUserTurn);
        }

        //computer hard level shoot mechanism
        public void computerHardLevelShoot(int[,] userGameArea, List<Ship> userShips, string firstname, string lastname,int computerRoundCounter)
        {
            while(true)
            {
                int X;
                int Y;

                checkShipIsFullySinkedforComputer(userShips, userGameArea);

                if (checkAllShipsAreFound(userShips))
                    break;

                if (isShipFound == true && lastShotSuccess == true && direction == null)
                {
                    (X, Y) = shootInRow(lastShootedIndex.Item1, lastShootedIndex.Item2, userGameArea);
                }
                else if (isShipFound == true && lastShotSuccess == false && direction == null)
                {
                    (X, Y) = shipFoundStartPoint;
                    (X, Y) = shootInRow(X, Y, userGameArea);
                }
                else if (threeAreaChecker == true)
                {
                    (X, Y) = shipFoundStartPoint;
                    (X, Y) = shootInRow(X, Y, userGameArea);
                    threeAreaChecker = false;
                }
                else if (isShipFound == false)
                {
                    (X, Y) = randomShootDecider(userGameArea, userShips, computerRoundCounter);
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
                    (X, Y) = randomShootDecider(userGameArea, userShips, computerRoundCounter);
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
                    threeAreaChecker = true;
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
                    lastShotSuccess = false;
                    direction = null;
                    break;
                }
            }
        }

        //computer hard level shoot mechanism MVC
        public ((int,int),(int, int), bool, bool, bool, bool, string, int, bool, string) computerHardLevelShoot(int[,] userGameArea, List<Ship> userShips, int computerRoundCounter, (int, int) lastShootedIndexMVC, (int, int) shipFoundStartPointMVC, bool isShipFoundMVC, bool lastShotSuccessMVC, bool firstSuccessShotCheckerMVC, bool threeAreaCheckerMVC, string directionMVC, int randomActivaterMVC, bool isUserTurn)
        {
            string sinkedShip = null;
            while (true)
            {
                int X;
                int Y;

                (isShipFoundMVC, directionMVC, lastShotSuccessMVC, firstSuccessShotCheckerMVC, sinkedShip) = checkShipIsFullySinkedforComputer(userShips, userGameArea,isShipFoundMVC, lastShotSuccessMVC,firstSuccessShotCheckerMVC,directionMVC);

                if (checkAllShipsAreFound(userShips))
                    break;

                if (isShipFoundMVC == true && lastShotSuccessMVC == true && directionMVC == null)
                {
                    ((X, Y),directionMVC) = shootInRow(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, directionMVC);
                }
                else if (isShipFoundMVC == true && lastShotSuccessMVC == false && directionMVC == null)
                {
                    (X, Y) = shipFoundStartPointMVC;
                    ((X, Y), directionMVC) = shootInRow(X, Y, userGameArea, directionMVC);
                }
                else if (isShipFoundMVC == false)
                {
                    (X, Y) = randomShootDecider(userGameArea, userShips, computerRoundCounter);
                }
                //after the second success shot, computer finds the ship's other pieces
                else if (directionMVC == "North")
                {
                    (X, Y, directionMVC) = getNorthCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else if (directionMVC == "South")
                {
                    (X, Y, directionMVC) = getSouthCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else if (directionMVC == "East")
                {
                    (X, Y, directionMVC) = getEastCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else if (directionMVC == "West")
                {
                    (X, Y, directionMVC) = getWestCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else
                {
                    (X, Y) = randomShootDecider(userGameArea, userShips, computerRoundCounter);
                }

                //succesful shoot check
                if (userGameArea[X, Y] == 1)
                {
                    userGameArea[X, Y] = 2;
                    randomActivaterMVC = 0;
                    lastShootedIndexMVC = (X, Y);

                    lastShotSuccessMVC = true;
                    isShipFoundMVC = true;
                    isUserTurn = false;


                    if (firstSuccessShotCheckerMVC == true)
                    {
                        shipFoundStartPointMVC = (X, Y);
                        firstSuccessShotCheckerMVC = false;
                    }

                    _logger.gamePrint("Computer succesfully shooted!!!");
                    break;
                }
                //already shot check
                else if (userGameArea[X, Y] == 2)
                {
                    isUserTurn = true;
                    _logger.printWarning("You are in loop because of game area is 2!!!");
                    break;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                {
                    isUserTurn = true;
                    _logger.printWarning("You are in a loop because of game area is 3");
                    threeAreaCheckerMVC = true;
                    break;
                }
                else if (userGameArea[X, Y] == 4)
                {
                    isUserTurn = true;
                    _logger.printWarning("You are in loop where game area is 4!!");
                    break;
                }
                //cross the fail shot
                else
                {
                    isUserTurn = true;
                    userGameArea[X, Y] = 3;
                    lastShotSuccessMVC = false;
                    directionMVC = null;
                    break;
                }
            }
            return (lastShootedIndexMVC, shipFoundStartPointMVC, isShipFoundMVC, lastShotSuccessMVC, firstSuccessShotCheckerMVC, threeAreaCheckerMVC, directionMVC, randomActivaterMVC, isUserTurn, sinkedShip);
        }

        //this function is focus on when it founds a success shot and try to find other piece of ship
        public (int, int) shootInRow(int X, int Y, int[,] userGameArea)
        {
            if (!(Y - 1 <= -1))
            {
                if (userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 2 && userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 3 && userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 4)
                {
                    direction = "North";
                    return (getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2);
                }
            }
            if (!(Y + 1 >= 10))
            {
                if (userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 2 && userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 3 && userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 4)
                {
                    direction = "South";
                    return (getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2);
                }
            }
            if (!(X + 1 >= 10))
            {
                if (userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 2 && userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 3 && userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 4)
                {
                    direction = "East";
                    return (getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2);
                }
            }
            if (!(X - 1 <= -1))
            {
                if (userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 2 && userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 3 && userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 4)
                {
                    direction = "West";
                    return (getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2);
                }
            }
            return (X, Y);
        }

        public ((int, int), string) shootInRow(int X, int Y, int[,] userGameArea, string directionMVC)
        {
            if (!(Y - 1 <= -1))
            {
                if (userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 2 && userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 3 && userGameArea[getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2] != 4)
                {
                    directionMVC = "North";
                    return ((getNorthCoordinate(X, Y).Item1, getNorthCoordinate(X, Y).Item2),directionMVC);
                }
            }
            if (!(Y + 1 >= 10))
            {
                if (userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 2 && userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 3 && userGameArea[getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2] != 4)
                {
                    directionMVC = "South";
                    return ((getSouthCoordinate(X, Y).Item1, getSouthCoordinate(X, Y).Item2), directionMVC);
                }
            }
            if (!(X + 1 >= 10))
            {
                if (userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 2 && userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 3 && userGameArea[getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2] != 4)
                {
                    directionMVC = "East";
                    return ((getEastCoordinate(X, Y).Item1, getEastCoordinate(X, Y).Item2), directionMVC);
                }
            }
            if (!(X - 1 <= -1))
            {
                if (userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 2 && userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 3 && userGameArea[getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2] != 4)
                {
                    directionMVC = "West";
                    return ((getWestCoordinate(X, Y).Item1, getWestCoordinate(X, Y).Item2), directionMVC);
                }
            }
            return ((X, Y),directionMVC);
        }

        //these functions are return the coordinate index according to the direction
        //there are override functions which are for MVC arch and you can recognize them from MVC tags in variables
        public (int, int) getNorthCoordinate(int X, int Y, int[,] userGameArea)
        {
            if(Y - 1 < 0)
            {
                direction = "South";
                return (shipFoundStartPoint.Item1, shipFoundStartPoint.Item2 + 1 );
            }  
            if(userGameArea[X, Y - 1] == 3 || userGameArea[X, Y - 1] == 4)
            {
                direction = "South";
                return (shipFoundStartPoint.Item1, shipFoundStartPoint.Item2 + 1);
            }
            return (X, Y - 1);
        }

        public (int, int, string) getNorthCoordinate(int X, int Y, int[,] userGameArea, (int,int) shipStartPointMVC, string directionMVC)
        {
            if (Y - 1 < 0)
            {
                directionMVC = "South";
                return (shipStartPointMVC.Item1, shipStartPointMVC.Item2 + 1, directionMVC);
            }
            if (userGameArea[X, Y - 1] == 3 || userGameArea[X, Y - 1] == 4)
            {
                directionMVC = "South";
                return (shipStartPointMVC.Item1, shipStartPointMVC.Item2 + 1, directionMVC);
            }
            return (X, Y - 1, directionMVC);
        }

        public (int, int) getNorthCoordinate(int X, int Y)
        {
            return (X, Y - 1);
        }

        public (int, int) getSouthCoordinate(int X, int Y, int[,] userGameArea)
        {
            if (Y + 1 > 9)
            {
                direction = "North";
                return (shipFoundStartPoint.Item1, shipFoundStartPoint.Item2 - 1);
            }
            if(userGameArea[X, Y + 1] == 3 || userGameArea[X, Y + 1] == 4)
            {
                direction = "North";
                return (shipFoundStartPoint.Item1, shipFoundStartPoint.Item2 - 1);
            }
            return (X, Y + 1);
        }

        public (int, int, string) getSouthCoordinate(int X, int Y, int[,] userGameArea, (int,int) shipFoundStartPointMVC, string directionMVC)
        {
            if (Y + 1 > 9)
            {
                directionMVC = "North";
                return (shipFoundStartPointMVC.Item1, shipFoundStartPointMVC.Item2 - 1, directionMVC);
            }
            if (userGameArea[X, Y + 1] == 3 || userGameArea[X, Y + 1] == 4)
            {
                directionMVC = "North";
                return (shipFoundStartPointMVC.Item1, shipFoundStartPointMVC.Item2 - 1, directionMVC);
            }
            return (X, Y + 1, directionMVC);
        }

        public (int, int) getSouthCoordinate(int X, int Y)
        {
            return (X, Y + 1);
        }

        public (int, int) getEastCoordinate(int X, int Y, int[,] userGameArea)
        {
            if (X + 1 > 9)
            {
                direction = "West";
                return (shipFoundStartPoint.Item1 - 1, shipFoundStartPoint.Item2);
            }
            if(userGameArea[X + 1, Y] == 3 || userGameArea[X + 1, Y] == 4)
            {
                direction = "West";
                return (shipFoundStartPoint.Item1 - 1, shipFoundStartPoint.Item2);
            }
            return (X + 1, Y);
        }

        public (int, int, string) getEastCoordinate(int X, int Y, int[,] userGameArea, (int,int) shipFoundStartPointMVC, string directionMVC)
        {
            if (X + 1 > 9)
            {
                directionMVC = "West";
                return (shipFoundStartPointMVC.Item1 - 1, shipFoundStartPointMVC.Item2, directionMVC);
            }
            if (userGameArea[X + 1, Y] == 3 || userGameArea[X + 1, Y] == 4)
            {
                directionMVC = "West";
                return (shipFoundStartPointMVC.Item1 - 1, shipFoundStartPointMVC.Item2, directionMVC);
            }
            return (X + 1, Y, directionMVC);
        }

        public (int, int) getEastCoordinate(int X, int Y)
        {
            return (X + 1, Y);
        }

        public (int, int) getWestCoordinate(int X, int Y, int[,] userGameArea)
        {
            if (X - 1 < 0)
            {
                direction = "East";
                return (shipFoundStartPoint.Item1 + 1, shipFoundStartPoint.Item2);
            }
            if(userGameArea[X - 1, Y] == 3 || userGameArea[X - 1, Y] == 4)
            {
                direction = "East";
                return (shipFoundStartPoint.Item1 + 1, shipFoundStartPoint.Item2);
            }
            return (X - 1, Y);
        }

        public (int, int, string) getWestCoordinate(int X, int Y, int[,] userGameArea, (int, int) shipFoundStartPointMVC, string directionMVC)
        {
            if (X - 1 < 0)
            {
                directionMVC = "East";
                return (shipFoundStartPointMVC.Item1 + 1, shipFoundStartPointMVC.Item2, directionMVC);
            }
            if (userGameArea[X - 1, Y] == 3 || userGameArea[X - 1, Y] == 4)
            {
                directionMVC = "East";
                return (shipFoundStartPointMVC.Item1 + 1, shipFoundStartPointMVC.Item2, directionMVC);
            }
            return (X - 1, Y, directionMVC);
        }

        public (int, int) getWestCoordinate(int X, int Y)
        {
            return (X - 1, Y);
        }

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
                            if (j + k > 9 || j + (ship.Length - 2) >= 9)
                                break;
                            if (userGameArea[i, j + k] == 2 || userGameArea[i, j + k] == 3 || userGameArea[i, j + k] == 4)
                            {
                                int temp = k;
                                for (k = 0; k < temp; k++)
                                {
                                    perfectProbability[i, j + k] -= 1;
                                }
                                perfectProbability[i, j + k] = 0;
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
                            if (j + k > 9 || j + (ship.Length - 2) >= 9)
                                break;
                            if (userGameArea[j + k, i] == 2 || userGameArea[j + k, i] == 3 || userGameArea[j + k, i] == 4)
                            {
                                int temp = k;
                                for (k = 0; k < temp; k++)
                                {
                                    perfectProbability[j + k, i] -= 1;
                                }
                                perfectProbability[j + k, i] = 0;
                                break;
                            }
                            perfectProbability[j + k, i] += 1;
                        }
                    }
                }
            }
            return perfectProbability;
        }

        //This is a shoot decider when computer does not focus on a ship
        public (int, int) randomShootDecider(int[,] userGameArea, List<Ship> userShips, int computerRoundCounter)
        {
            int X;
            int Y;

            if (computerRoundCounter > 0)
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

        //computer medium level shoot mechanism
        public void computerMediumLevelShoot(int[,] userGameArea, List<Ship> userShips, string firstname, string lastname)
        {
            while (true)
            {
                int X;
                int Y;

                checkShipIsFullySinkedforComputer(userShips, userGameArea);

                if (checkAllShipsAreFound(userShips))
                    break;

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
                    lastShotSuccess = false;
                    direction = null;
                    break;
                }
            }
        }

        public ((int, int), (int, int), bool, bool, bool, bool, string, int, bool, string) computerMediumLevelShoot(int[,] userGameArea, List<Ship> userShips, int computerRoundCounter, (int, int) lastShootedIndexMVC, (int, int) shipFoundStartPointMVC, bool isShipFoundMVC, bool lastShotSuccessMVC, bool firstSuccessShotCheckerMVC, bool threeAreaCheckerMVC, string directionMVC, int randomActivaterMVC, bool isUserTurn)
        {
            string sinkedShip = null;
            while (true)
            {
                int X;
                int Y;

                (isShipFoundMVC, directionMVC, lastShotSuccessMVC, firstSuccessShotCheckerMVC, sinkedShip) = checkShipIsFullySinkedforComputer(userShips, userGameArea, isShipFoundMVC, lastShotSuccessMVC, firstSuccessShotCheckerMVC, directionMVC);

                if (checkAllShipsAreFound(userShips))
                    break;

                if (isShipFoundMVC == true && lastShotSuccessMVC == true && directionMVC == null)
                {
                    ((X, Y), directionMVC) = shootInRow(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, directionMVC);
                }
                else if (isShipFoundMVC == true && lastShotSuccessMVC == false && directionMVC == null)
                {
                    (X, Y) = shipFoundStartPointMVC;
                    ((X, Y), directionMVC) = shootInRow(X, Y, userGameArea, directionMVC);
                }
                else if (threeAreaCheckerMVC == true)
                {
                    (X, Y) = shipFoundStartPointMVC;
                    ((X, Y), directionMVC) = shootInRow(X, Y, userGameArea, directionMVC);
                    threeAreaCheckerMVC = false;
                }
                else if (isShipFoundMVC == false)
                {
                    X = _random.Next(10);
                    Y = _random.Next(10);
                }
                //after the second success shot, computer finds the ship's other pieces
                else if (directionMVC == "North")
                {
                    (X, Y, directionMVC) = getNorthCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else if (directionMVC == "South")
                {
                    (X, Y, directionMVC) = getSouthCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else if (directionMVC == "East")
                {
                    (X, Y, directionMVC) = getEastCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else if (directionMVC == "West")
                {
                    (X, Y, directionMVC) = getWestCoordinate(lastShootedIndexMVC.Item1, lastShootedIndexMVC.Item2, userGameArea, shipFoundStartPointMVC, directionMVC);
                }
                else
                {
                    X = _random.Next(10);
                    Y = _random.Next(10);
                }

                //succesful shoot check
                if (userGameArea[X, Y] == 1)
                {
                    userGameArea[X, Y] = 2;
                    lastShootedIndexMVC = (X, Y);

                    lastShotSuccessMVC = true;
                    isShipFoundMVC = true;
                    isUserTurn = false;


                    if (firstSuccessShotCheckerMVC == true)
                    {
                        shipFoundStartPointMVC = (X, Y);
                        firstSuccessShotCheckerMVC = false;
                    }

                    _logger.gamePrint("Computer succesfully shooted!!!");
                    break;
                }
                //already shot check
                else if (userGameArea[X, Y] == 2)
                {
                    isUserTurn = true;
                    _logger.printWarning("You are in loop because of game area is 2!!!");
                    break;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                {
                    isUserTurn = true;
                    _logger.printWarning("You are in a loop because of game area is 3");
                    threeAreaCheckerMVC = true;
                    break;
                }
                else if (userGameArea[X, Y] == 4)
                {
                    isUserTurn = true;
                    _logger.printWarning("You are in loop where game area is 4!!");
                    break;
                }
                //cross the fail shot
                else
                {
                    isUserTurn = true;
                    userGameArea[X, Y] = 3;
                    lastShotSuccessMVC = false;
                    directionMVC = null;
                    break;
                }
            }
            return (lastShootedIndexMVC, shipFoundStartPointMVC, isShipFoundMVC, lastShotSuccessMVC, firstSuccessShotCheckerMVC, threeAreaCheckerMVC, directionMVC, randomActivaterMVC, isUserTurn, sinkedShip);
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
                    break;
                }
            }
        }

        public bool computerEasyLevelShoot(int[,] userGameArea, bool isUserTurn)
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
                    isUserTurn = false;
                    userGameArea[X, Y] = 2;
                    _logger.gamePrint("Computer succesfully shooted!!!");
                    break;
                }
                //already shot check
                else if (userGameArea[X, Y] == 2)
                {
                    isUserTurn = true;
                    break;
                }
                //already fail shot check
                else if (userGameArea[X, Y] == 3)
                {
                    isUserTurn = true;
                    break;
                }
                //cross the fail shot
                else
                {
                    isUserTurn = true;
                    userGameArea[X, Y] = 3;
                    break;
                }
            }
            return isUserTurn;
        }

        //computer recognize that it should not shoot one squared index
        public void recognizeOneSqureRule(int[,] userGameArea, Ship ship)
        {
            if (ship.VerorHor == false)
            {
                for (int i = ship.LocationIndex - 1; i <= ship.LocationIndex + 1; i++)
                {
                    for (int j = ship.StartIndex - 1; j <= ship.EndIndex; j++)
                    {
                        if (i > 9 || i < 0 || j < 0 || j > 9)
                            continue;
                        if (i == ship.LocationIndex - 1 || i == ship.LocationIndex + 1)
                        {
                            userGameArea[j, i] = 4;
                        }
                        if (i == ship.LocationIndex && j == ship.StartIndex - 1)
                        {
                            userGameArea[j, i] = 4;
                        }
                        else if (i == ship.LocationIndex && j == ship.EndIndex)
                        {
                            userGameArea[j, i] = 4;
                        }
                    }
                }
            }
            else if (ship.VerorHor == true)
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
        public bool checkAllShipsAreFound(List<Ship> ships)
        {
            if(ships.All(x => x.isShipSinked == true))
                return true;
            return false;
        }

        public (bool,bool) checkUserWin(List<Ship> ships, string firstname, string lastname, int[,] computerGameArea, int[,] userGameArea, bool isUserWinner, int userRoundCounter)
        {
            if(checkAllShipsAreFound(ships))
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

        // Override function for user win check for .NET MVC
        public bool checkUserWin(List<Ship> ships, int[,] computerGameArea)
        {
            if (checkAllShipsAreFound(ships))
            {
                return true;
            }
            return false;
        }

        public (bool,bool) checkComputerWin(List<Ship> ships, int[,] computerGameArea, int[,] userGameArea, bool isUserWinner, int computerRoundCounter)
        {
            if (checkAllShipsAreFound(ships))
            {
                _logger.gamePrint($"Nice try, Computer found all the ships in {computerRoundCounter} round!!!");
                Console.WriteLine("Last Situation \n___________________________________________");
                printComputerGameArea(computerGameArea);
                printUserGameArea(userGameArea, "User", "");
                isUserWinner = false;
                return (true, isUserWinner);
            }
            return (false, isUserWinner);
        }

        public bool checkComputerWin(List<Ship> ships, int[,] userGameArea)
        {
            if (checkAllShipsAreFound(ships))
            {
                return true;
            }
            return false;
        }

        //this is a helper function for random ship placement
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

        public (bool,string,bool,bool,string) checkShipIsFullySinkedforComputer(List<Ship> ships, int[,] gameArea, bool isShipFoundMVC, bool lastShotSuccessMVC, bool firstSuccessShotCheckerMVC, string directionMVC)
        {
            string sinkedShip = null;
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
                    isShipFoundMVC = false;
                    ship.isShipSinked = true;
                    directionMVC = null;
                    lastShotSuccessMVC = false;
                    firstSuccessShotCheckerMVC = true;
                    sinkedShip = $"{ship.Name} is sinked!!";
                    recognizeOneSqureRule(gameArea, ship);
                }
            }
            return (isShipFoundMVC, directionMVC, lastShotSuccessMVC, firstSuccessShotCheckerMVC, sinkedShip);
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

        public string checkShipIsFullySinkedforUserMVC(List<Ship> ships, int[,] gameArea)
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
                    recognizeOneSqureRule(gameArea, ship);
                    return $"{ship.Name} is sinked!!";
                }
            }
            return null;
        }
    }
}
