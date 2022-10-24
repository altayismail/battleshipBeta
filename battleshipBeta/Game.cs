using System;

namespace battleshipBeta
{
    internal class Game
    {
        public int[,] gameArea;
        public int[] triple;
        public int[] doublee;
        public int roundCounter = 0;

        Ship? tripleShip;
        Ship? doubleeShip;

        private readonly Random _random;

        public Game(Random random)
        {
            _random = random;
        }

        //creating game area
        public int[,] createGameArea()
        {
            gameArea = new int[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    gameArea[i, j] = 0;
                }
            }
            return gameArea;
        }

        //placing ship with length 3
        public void placeTriple(int[,] gameArea)
        {
            var locationIndex = _random.Next(5);
            var startIndex = _random.Next(5);
            var endIndex = startIndex + 3;

            //check if placing is in game area
            if(endIndex > 5)
            {
                int tempIndex = startIndex;
                startIndex -= 3;
                endIndex = tempIndex;
            }

            tripleShip = new Ship(3,"Triple", startIndex, endIndex, true, locationIndex);
   
            //check vertical or horizontal placement
            if(tripleShip.VerorHor == true)
            {
                for (int i = tripleShip.StartIndex; i < tripleShip.EndIndex; i++)
                {
                    gameArea[tripleShip.LocationIndex, i] = 1;
                }
            }
            else
            {
                for (int i = tripleShip.StartIndex; i < tripleShip.EndIndex; i++)
                {
                    gameArea[i, tripleShip.LocationIndex] = 1;
                }
            }
        }

        //placing ship with length 2
        public void placeDouble(int[,] gameArea)
        {
            while(true)
            {
                var locationIndex = _random.Next(5);
                var startIndex = _random.Next(5);
                var endIndex = startIndex + 2;

                //check if placing is in game area
                if (endIndex > 5)
                {
                    int tempIndex = startIndex;
                    startIndex -= 2;
                    endIndex = tempIndex;
                }

                doubleeShip = new Ship(2, "Double", startIndex, endIndex, false, locationIndex);

                // check vertical or horizontal placement
                if (doubleeShip.VerorHor == true)
                {
                    //check if the double ship overlap with triple ship
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        if (gameArea[doubleeShip.LocationIndex, i] == 1)
                            continue;
                    }
                    //place the double ship
                    for (int i = doubleeShip.StartIndex; i < doubleeShip.EndIndex; i++)
                    {
                        gameArea[doubleeShip.LocationIndex, i] = 1;
                    }
                    break;
                }
                else
                {
                    //check if the double ship overlap with triple ship
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        if (gameArea[i, doubleeShip.LocationIndex] == 1)
                            continue;
                    }
                    //place the double ship
                    for (int i = doubleeShip.StartIndex; i < doubleeShip.EndIndex; i++)
                    {
                        gameArea[i, doubleeShip.LocationIndex] = 1;
                    }
                    break;
                }
            }
        }

        //print game area
        public void printGameArea(int[,] gameArea)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (gameArea[i, j] == 0)
                        Console.Write("0 ");
                    else if (gameArea[i, j] == 1)
                        Console.Write("0 ");
                    else if (gameArea[i, j] == 3)
                        Console.Write("X ");
                    else
                        Console.Write("S ");
                }
                Console.Write("\n");
            }
            Console.WriteLine("_________________________");

        }

        //shoot mechanism
        public void shoot(int[,] gameArea)
        {
            while(true)
            {
                Console.WriteLine("Please enter Y coordinate!");
                Console.Write("Y: ");
                var x_axis = Console.ReadLine();
                int X;

                //only number input validation
                if(int.TryParse(x_axis, out X))
                {
                    X = int.Parse(x_axis);
                }
                else
                {
                    Console.WriteLine("You have to enter a number!!!");
                    continue;
                }

                //check boundry of the game area for x
                if (X >= 5 || X < 0)
                {
                    Console.WriteLine("You enter a number out of the coordinate!!");
                    continue;
                }

                Console.WriteLine("Please enter X coordinate!");
                Console.Write("X: ");
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
                if (Y >= 5 || Y < 0)
                {
                    Console.WriteLine("You enter a number out of the coordinate!!");
                    continue;
                }

                //succesful shoot check
                if (gameArea[X, Y] == 1)
                {
                    gameArea[X, Y] = 2;
                    roundCounter++;
                    Console.WriteLine("You succesfully shoot!!");
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
                    gameArea[X, Y] = 3;
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
    }
}
