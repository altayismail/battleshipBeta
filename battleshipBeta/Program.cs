using battleshipBeta;

Console.WriteLine("Welcome To Battle Ship!!!!");

Random random = new Random();
Game game = new Game(random);
int[,] gameArea = game.createGameArea();
game.placeAdmiral(gameArea);
game.placeCruiser(gameArea);
game.placeDestroyer(gameArea);
game.placeDestroyer(gameArea);
game.placeAssault(gameArea);
Console.WriteLine("____________________________");
Console.WriteLine("Game Cheat Sheeat");
for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
    {
        Console.Write(gameArea[i, j] + " ");
    }
    Console.Write("\n");
}
Console.WriteLine("_________________________");

while (true)
{
    game.printGameArea(gameArea);
    //Thread.Sleep(1000);
    game.shoot(gameArea);
    if(game.checkAllShipsAreFound(gameArea) == 17)
    {
        Console.WriteLine("Congrats, you found all the ships!!!");
        break;
    }
    if(game.roundCounter == 100)
    {
        Console.WriteLine("Congrats, you found all the ships!!!");
        game.printGameArea(gameArea);
        break;
    }
}
