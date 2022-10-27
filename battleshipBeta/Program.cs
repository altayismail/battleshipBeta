using battleshipBeta;

Console.WriteLine("Welcome To Battle Ship!!!!");

Random random = new Random();
Logger logger = new Logger();
Game game = new Game(random, logger);

Ship admiral = new Ship(5, "Admiral");
Ship cruiser = new Ship(4, "Cruiser");
Ship destroyer = new Ship(3, "Destroyer");
Ship destroyer2 = new Ship(3, "Destroyer");
Ship assault = new Ship(2, "Assault");

int[,] gameArea = game.createGameArea();

//game.placeAdmiral(gameArea);
//game.placeCruiser(gameArea);
//game.placeDestroyer(gameArea);
//game.placeDestroyer(gameArea);
//game.placeAssault(gameArea);

game.placementMechanism(gameArea, admiral);
logger.print("Admiral is planted...");
game.placementMechanism(gameArea, cruiser);
logger.print("Cruiser is planted...");
game.placementMechanism(gameArea, destroyer);
logger.print("Destroyer is planted...");
game.placementMechanism(gameArea, destroyer2);
logger.print("Destroyer2 is planted...");
game.placementMechanism(gameArea, assault);
logger.print("Assault is planted...");

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
