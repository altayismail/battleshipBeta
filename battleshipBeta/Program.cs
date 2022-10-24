using battleshipBeta;

Console.WriteLine("Welcome To Battle Ship!!!!");
Console.WriteLine("If you want to play, enter yes.");
Console.Write("Answer: ");
var verify = Console.ReadLine();

if(verify == "yes")
{
    Random random = new Random();
    Game game = new Game(random);
    int[,] gameArea = game.createGameArea();
    game.placeTriple(gameArea);
    game.placeDouble(gameArea);

    while (true)
    {
        game.printGameArea(gameArea);
        //Thread.Sleep(1000);
        game.shoot(gameArea);
        if(game.checkAllShipsAreFound(gameArea) == 5)
        {
            Console.WriteLine("Congrats, you found all the ships!!!");
            break;
        }
        if(game.roundCounter == 25)
        {
            Console.WriteLine("Congrats, you found all the ships!!!");
            game.printGameArea(gameArea);
            break;
        }
    }
}
