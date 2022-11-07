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
            for (int i = locationIndex-1; i <= locationIndex+1 ; i++)
            {
                for (int j = startIndex-1; j <= endIndex+1; j++)
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
            while(true)
            {
                Console.WriteLine("Please enter X coordinate!");
                _logger.inputer("X: ");
                var x_axis = Console.ReadLine();
                int X;

                //only number input validation
                if(isItParsable(x_axis))
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
                if (gameArea[X-1, Y-1] == 1)
                {
                    gameArea[X-1, Y-1] = 2;
                    roundCounter++;
                    _logger.gamePrint("You succesfully shoot!!");
                    printComputerGameArea(gameArea);
                    continue;
                }
                //already shot check
                else if (gameArea[X-1, Y-1] == 2)
                {
                    printComputerGameArea(gameArea);
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
                    printUserGameArea(gameArea);
                    continue;
                }
                //already shot check
                else if (gameArea[X, Y] == 2)
                {
                    printUserGameArea(gameArea);
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

        public (string,string) nameAndSurnameInput()
        {
            _logger.inputer("First Name: ");
            string firstname = Console.ReadLine();
            _logger.inputer("Last Name: ");
            string lastname = Console.ReadLine();

            return (firstname, lastname);
        }
    }
}
