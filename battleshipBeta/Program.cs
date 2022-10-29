using battleshipBeta;

Random random = new Random();
Logger logger = new Logger();
Game game = new Game(random, logger);
Placement placement = new Placement(game, logger, random);

Ship admiral = new Ship(5, "Admiral", 1);
Ship cruiser = new Ship(4, "Cruiser", 2);
Ship destroyer = new Ship(3, "Destroyer", 3);
Ship destroyer2 = new Ship(3, "Destroyer2", 4);
Ship assault = new Ship(2, "Assault", 5);

List<Ship> ships = new List<Ship>();
ships.Add(admiral);
ships.Add(cruiser);
ships.Add(destroyer);
ships.Add(destroyer2);
ships.Add(assault);

int[,] computerGameArea = game.createGameArea();

placement.placementMechanism(computerGameArea, admiral);
//game.placementMechanism(computerGameArea, admiral);
logger.print("Admiral is planted...");
placement.placementMechanism(computerGameArea, cruiser);
//game.placementMechanism(computerGameArea, cruiser);
logger.print("Cruiser is planted...");
placement.placementMechanism(computerGameArea, destroyer);
//game.placementMechanism(computerGameArea, destroyer);
logger.print("Destroyer is planted...");
placement.placementMechanism(computerGameArea, destroyer2);
//game.placementMechanism(computerGameArea, destroyer2);
logger.print("Destroyer2 is planted...");
placement.placementMechanism(computerGameArea, assault);
//game.placementMechanism(computerGameArea, assault);
logger.print("Assault is planted...");

Console.WriteLine("____________________________");
Console.WriteLine("Game Cheat Sheeat");

for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 10; j++)
    {
        Console.Write(computerGameArea[j, i] + " ");
    }
    Console.Write("\n");
}
Console.WriteLine("_________________________");
Console.WriteLine("Welcome To Battle Ship!!!!");
while (true)
{  
    Console.WriteLine("Please select the game mode");
    Console.WriteLine("____________________________");
    Console.WriteLine("1. Traning Mode\n2. AI Mode\n3. Quit");
    Console.Write("Choice: ");
    var gameModeChoice = Console.ReadLine();
    Console.WriteLine("____________________________");
    switch (gameModeChoice)
    {
        case "1":
            Console.WriteLine("Welcome to the Traning Mode...");
            Console.WriteLine("____________________________");
            while (true)
            {
                Console.WriteLine("____________________________");
                game.printComputerGameArea(computerGameArea);
                game.shoot(computerGameArea);
                if (game.checkAllShipsAreFound(computerGameArea) == 17)
                {
                    Console.WriteLine("Congrats, you found all the ships!!!");
                    break;
                }
                if (game.roundCounter == 100)
                {
                    Console.WriteLine("Congrats, you found all the ships!!!");
                    game.printComputerGameArea(computerGameArea);
                    break;
                }
            }
            break;
        case "2":
            Console.WriteLine("Welcome to the AI Mode...");
            Console.WriteLine("____________________________");
            int[,] userGameArea = game.createGameArea();
            for (int i = 0; i < ships.Count; i++)
            {
                placement.placementMechanismForUser(userGameArea, ships);
                //game.placementMechanismForUser(userGameArea, ships);
            }
            while (true)
            {
                Console.WriteLine("____________________________");
                Console.WriteLine("Computer Game Area");
                game.printComputerGameArea(computerGameArea);
                Console.WriteLine("*****************************");
                Console.WriteLine("User Game Area");
                game.printUserGameArea(userGameArea);
                game.shoot(computerGameArea);
                game.computerShoot(userGameArea);
                if (game.checkAllShipsAreFound(computerGameArea) == 17 || game.checkAllShipsAreFound(userGameArea) == 17)
                {
                    Console.WriteLine("Congrats, you found all the ships!!!");
                    break;
                }
                if (game.roundCounter == 200)
                {
                    Console.WriteLine("Congrats, you found all the ships!!!");
                    game.printComputerGameArea(userGameArea);
                    break;
                }
            }
            break;
        case "3":
            break;
        default:
            Console.WriteLine("Please enter valid choice!!!");
            continue;
    }
}
