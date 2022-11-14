using battleshipBeta;
using battleshipBeta.Database;
using battleshipBeta.Entities;

Random random = new Random();
Logger logger = new Logger();
Game game = new Game(random, logger);
Placement placement = new Placement(game, logger, random);
Context context = new Context();
Score score = new Score(context);

List<Ship> userShips = game.getListOfUserShip(game.userAdmiral, game.userCruiser, game.userDestroyer, game.userDestroyer2, game.userAssault);
List<Ship> computerShips = game.getListOfComputerShip(game.computerAdmiral, game.computerCruiser, game.computerDestroyer, game.comptuerDestroyer2, game.computerAssault);
int[,] computerGameArea = game.createGameArea();

placement.placementMechanism(computerGameArea, game.computerAdmiral);
placement.placementMechanism(computerGameArea, game.computerCruiser);
placement.placementMechanism(computerGameArea, game.computerDestroyer);
placement.placementMechanism(computerGameArea, game.comptuerDestroyer2);
placement.placementMechanism(computerGameArea, game.computerAssault);

Console.WriteLine("Welcome To Battle Ship!!!!");
while (true)
{
    menuScript();
    logger.inputer("Choice: ");
    var gameModeChoice = Console.ReadLine();
    if (gameModeChoice == "5")
        break;

    Console.WriteLine("____________________________");
    switch (gameModeChoice)
    {
        case "1":
            Console.WriteLine("Welcome to the Traning Mode...");
            Console.WriteLine("____________________________");
            var (t_firstname, t_lastname) = game.nameAndSurnameInput();

            DateTime t_startTime = DateTime.Now;
            while (true)
            {
                game.printComputerGameArea(computerGameArea);
                game.userShoot(computerGameArea, computerShips);
                if (game.checkUserWin(computerGameArea, t_firstname, t_lastname))
                    break;
                if (game.checkUserHoundredRound(computerGameArea, t_firstname, t_lastname))
                    break;
            }

            DateTime t_endTime = DateTime.Now;
            double t_duration = (t_endTime - t_startTime).TotalMinutes;

            ExcelObjectTuttorial T_score = new ExcelObjectTuttorial()
                { Firstname = t_firstname, Lastname = t_lastname, Duration = t_duration };
            score.createScoreforTuttorial(T_score);
            continue;

        case "2":
            Console.WriteLine("Welcome to the AI Mode...");
            Console.WriteLine("____________________________");
            var (ai_firstname, ai_lastname) = game.nameAndSurnameInput();

            int[,] userGameArea = game.createGameArea();

            Console.WriteLine("Do you want to place the ships randomly?");
            logger.inputer("Yes (y), No (n): ");
            string placement_type_choice = Console.ReadLine();

            if(placement_type_choice == "y")
            {
                placement.placementMechanism(userGameArea, game.userAdmiral);
                placement.placementMechanism(userGameArea, game.userCruiser);
                placement.placementMechanism(userGameArea, game.userDestroyer);
                placement.placementMechanism(userGameArea, game.userDestroyer2);
                placement.placementMechanism(userGameArea, game.userAssault);
            }
            else if(placement_type_choice == "n")
            {
                for (int i = 0; i < userShips.Count; i++)
                {
                    placement.placementMechanismForUser(userGameArea, userShips);
                }
            }

            DateTime ai_startTime = DateTime.Now;
            while (true)
            {
                game.printComputerGameArea(computerGameArea);
                game.printUserGameArea(userGameArea);
                game.userShoot(computerGameArea, computerShips);
                game.computerShoot(userGameArea, userShips);
                if (game.checkUserWin(computerGameArea,ai_firstname, ai_lastname))
                    break;
                if (game.checkComputerWin(userGameArea))
                    break;
                if (game.checkUserHoundredRound(computerGameArea, ai_firstname, ai_lastname))
                    break;
                if (game.checkComputerHoundredRound(userGameArea))
                    break;
            }

            DateTime ai_endTime = DateTime.Now;
            double ai_duration = (ai_endTime - ai_startTime).TotalMinutes;

            ExcelObjectAI ai_score = new ExcelObjectAI()
                { Firstname = ai_firstname, Lastname = ai_lastname, Duration = ai_duration, Mode = "Hard" };
            score.createScoreforAI(ai_score);

            continue;
        case "3":
            score.getListOfTuttorialScore();
            continue;
        case "4":
            score.getListOfAIScore();
            continue;
        default:
            Console.WriteLine("Please enter valid choice!!!");
            continue;
    }
}

Console.WriteLine("You have successfully exit.");
void menuScript()
{
    Console.WriteLine("Please select the game mode");
    Console.WriteLine("____________________________");
    Console.WriteLine("1. Traning Mode\n2. AI Mode\n3. Past Results of Traning Mode\n4. Past Result of AI Mode\n5. Quit");
}

//Console.WriteLine("____________________________");
//Console.WriteLine("Game Cheat Sheeat");

//for (int i = 0; i < 10; i++)
//{
//    for (int j = 0; j < 10; j++)
//    {
//        Console.Write(computerGameArea[j, i] + " ");
//    }
//    Console.Write("\n");
//}
