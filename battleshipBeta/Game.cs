using System;

namespace battleshipBeta
{
    internal class Game
    {
        public int[,] gameArea;
        public int[] triple;
        public int[] doublee;
        public int roundCounter = 0;

        Ship? admiralShip;
        Ship? cruiserShip;
        Ship? destroyerShip;
        Ship? assaultShip;

        private readonly Random _random;

        public Game(Random random)
        {
            _random = random;
        }

        //creating game area
        public int[,] createGameArea()
        {
            gameArea = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameArea[i, j] = 0;
                }
            }
            return gameArea;
        }

        public void placementMechanism(int[,] gameArea, Ship ship)
        {
            var index = randomIndexing(5);
   
            //check vertical or horizontal placement
            if(ship.VerorHor == true)
                horizontalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
            else
                verticalPlacement(gameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
        }

        //placing ship with length 5
        public void placeAdmiral(int[,] gameArea)
        {
            var index = randomIndexing(5);

            admiralShip = new Ship(5,"Admiral", index.Item2, index.Item3, true, index.Item1);
   
            //check vertical or horizontal placement
            if(admiralShip.VerorHor == true)
                horizontalPlacement(gameArea, admiralShip.StartIndex, admiralShip.EndIndex, admiralShip.LocationIndex);
            else
                verticalPlacement(gameArea, admiralShip.StartIndex, admiralShip.EndIndex, admiralShip.LocationIndex);
        }

        //placing ship with length 4
        public void placeCruiser(int[,] gameArea)
        {
            while(true)
            {
                var index = randomIndexing(4);

                cruiserShip = new Ship(4, "Cruiser", index.Item2, index.Item3, false, index.Item1);

                // check vertical or horizontal placement
                if (cruiserShip.VerorHor == true)
                {
                    //check if the double ship overlap with triple ship
                    for (int i = cruiserShip.StartIndex; i < cruiserShip.EndIndex; i++)
                    {
                        if (gameArea[cruiserShip.LocationIndex, i] == 1)
                            continue;
                    }
                    //place the double ship
                    horizontalPlacement(gameArea, cruiserShip.StartIndex, cruiserShip.EndIndex, cruiserShip.LocationIndex);
                    break;
                }
                else
                {
                    //check if the double ship overlap with triple ship
                    for (int i = cruiserShip.StartIndex; i < cruiserShip.EndIndex; i++)
                    {
                        if (gameArea[i, cruiserShip.LocationIndex] == 1)
                            continue;
                    }
                    //place the double ship
                    verticalPlacement(gameArea, cruiserShip.StartIndex, cruiserShip.EndIndex, cruiserShip.LocationIndex);
                    break;
                }
            }
        }

        //placing ship with length 3
        public void placeDestroyer(int[,] gameArea)
        {
            while(true)
            {
                var index = randomIndexing(3);

                destroyerShip = new Ship(3, "Destroyer", index.Item2, index.Item3, false, index.Item1);

                // check vertical or horizontal placement
                if (destroyerShip.VerorHor == true)
                {
                    //check if the double ship overlap with triple ship
                    for (int i = destroyerShip.StartIndex; i < destroyerShip.EndIndex; i++)
                    {
                        if (gameArea[destroyerShip.LocationIndex, i] == 1)
                            continue;
                    }
                    //place the double ship
                    horizontalPlacement(gameArea, destroyerShip.StartIndex, destroyerShip.EndIndex, destroyerShip.LocationIndex);
                    break;
                }
                else
                {
                    //check if the double ship overlap with triple ship
                    for (int i = destroyerShip.StartIndex; i < destroyerShip.EndIndex; i++)
                    {
                        if (gameArea[i, destroyerShip.LocationIndex] == 1)
                            continue;
                    }
                    //place the double ship
                    verticalPlacement(gameArea, destroyerShip.StartIndex, destroyerShip.EndIndex, destroyerShip.LocationIndex);
                    break;
                }
            }
        }
        
        //placing ship with length 2
        public void placeAssault(int[,] gameArea)
        {
            while(true)
            {
                var index = randomIndexing(2);

                assaultShip = new Ship(2, "Assault", index.Item2, index.Item3, false, index.Item1);

                // check vertical or horizontal placement
                if (assaultShip.VerorHor == true)
                {
                    //check if the double ship overlap with triple ship
                    for (int i = assaultShip.StartIndex; i < assaultShip.EndIndex; i++)
                    {
                        if (gameArea[assaultShip.LocationIndex, i] == 1)
                            continue;
                    }
                    //place the double ship
                    horizontalPlacement(gameArea, assaultShip.StartIndex, assaultShip.EndIndex, assaultShip.LocationIndex);
                    break;
                }
                else
                {
                    //check if the double ship overlap with triple ship
                    for (int i = assaultShip.StartIndex; i < assaultShip.EndIndex; i++)
                    {
                        if (gameArea[i, assaultShip.LocationIndex] == 1)
                            continue;
                    }
                    //place the double ship
                    verticalPlacement(gameArea, assaultShip.StartIndex, assaultShip.EndIndex, assaultShip.LocationIndex);
                    break;
                }
            }
        }

        //print game area
        public void printGameArea(int[,] gameArea)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
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
                if (X > 10 || X <= 0)
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

    }
}
