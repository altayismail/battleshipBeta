using battleshipBeta;

Random random = new Random();
Logger logger = new Logger();
Game game = new Game(random, logger);
Placement placement = new Placement(game, logger, random);
Excel excel = new Excel();

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
logger.print("Admiral is planted...");
placement.placementMechanism(computerGameArea, cruiser);
logger.print("Cruiser is planted...");
placement.placementMechanism(computerGameArea, destroyer);
logger.print("Destroyer is planted...");
placement.placementMechanism(computerGameArea, destroyer2);
logger.print("Destroyer2 is planted...");
placement.placementMechanism(computerGameArea, assault);
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
    Console.WriteLine("1. Traning Mode\n2. AI Mode\n3. Past Results of Traning Mode\n4. Past Result of AI Mode\n5. Quit");
    Console.Write("Choice: ");
    var gameModeChoice = Console.ReadLine();
    if (gameModeChoice == "5")
        break;

    Console.WriteLine("____________________________");
    switch (gameModeChoice)
    {
        case "1":
            Console.WriteLine("Welcome to the Traning Mode...");
            Console.WriteLine("____________________________");
            Console.Write("First Name: ");
            string firstname_traning = Console.ReadLine();
            Console.Write("Last Name: ");
            string lastname_traning = Console.ReadLine();
            DateTime startTime_traning = DateTime.Now;
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
            DateTime endTime_traning = DateTime.Now;
            double duration_traning = (endTime_traning.Minute - startTime_traning.Minute);
            excel.writeTraningExcelFile(firstname_traning, lastname_traning, duration_traning);
            continue;
        case "2":
            Console.WriteLine("Welcome to the AI Mode...");
            Console.WriteLine("____________________________");
            Console.Write("First Name: ");
            string firstname = Console.ReadLine();
            Console.Write("Last Name: ");
            string lastname = Console.ReadLine();
            int[,] userGameArea = game.createGameArea();
            for (int i = 0; i < ships.Count; i++)
            {
                placement.placementMechanismForUser(userGameArea, ships);
            }
            DateTime startTime = DateTime.Now;
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
            DateTime endTime = DateTime.Now;
            double duration = (endTime.Minute - startTime.Minute);
            excel.writeAIExcelFile(firstname, lastname, "Easy", duration);
            continue;
        case "3":
            excel.readTraningExcelFile();
            continue;
        case "4":
            excel.readAIExcelFile();
            continue;
        default:
            Console.WriteLine("Please enter valid choice!!!");
            continue;
    }
}
Console.WriteLine("You have successfully exit.");
