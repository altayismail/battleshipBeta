using battleshipBeta;
using battleshipBeta.Database;
using battleshipBeta.Entities;

Random random = new Random();
Logger logger = new Logger();
Game game = new Game(random, logger);
Placement placement = new Placement(game);
Context context = new Context();
Score score = new Score(context);

Console.WriteLine("Welcome To Battle Ship!!!!");
while (true)
{
    Ship computerAircraft = new Ship(5, "Computer Aircraft Carrier", 1);
    Ship computerBattleship = new Ship(4, "Computer Battleship", 2);
    Ship computerCruiser = new Ship(3, "Computer Cruiser", 3);
    Ship computerSubmarine = new Ship(3, "Computer Submarine", 4);
    Ship computerDestroyer = new Ship(2, "Computer Destroyer", 5);

    Ship userAircraft = new Ship(5, "User Aircraft Carrier", 1);
    Ship userBattleship = new Ship(4, "User Battleship", 2);
    Ship userCruiser = new Ship(3, "User Cruiser", 3);
    Ship userSubmarine = new Ship(3, "User Submarine", 4);
    Ship userDestroyer = new Ship(2, "User Destroyer", 5);

    int computerRoundCounter = 0;
    int userRoundCounter = 0;

    List<Ship> computerShips = game.getListOfComputerShip(computerAircraft, computerBattleship, computerCruiser, computerSubmarine, computerDestroyer);
    List<Ship> userShips = game.getListOfUserShip(userAircraft, userBattleship, userCruiser, userSubmarine, userDestroyer);

    int[,] computerGameArea = game.createGameArea();
    int[,] userGameArea = game.createGameArea();

    menuScript();
    logger.inputer("Choice: ");
    var gameModeChoice = Console.ReadLine();
    if (gameModeChoice == "5")
        break;


    Console.WriteLine("____________________________");
    switch (gameModeChoice)
    {
        case "1":
            placement.placeAllComputerShip(computerGameArea, computerShips);
            Console.WriteLine("Welcome to the Traning Mode...");
            Console.WriteLine("____________________________");
            var (t_firstname, t_lastname) = game.nameAndSurnameInput();

            DateTime t_startTime = DateTime.Now;
            while (true)
            {
                game.printComputerGameArea(computerGameArea);
                game.userShoot(computerGameArea, computerShips);
                userRoundCounter++;
                if (game.checkUserWin(computerShips, t_firstname, t_lastname, computerGameArea, computerGameArea, true, userRoundCounter).Item1)
                    break;
            }

            DateTime t_endTime = DateTime.Now;
            double t_duration = (t_endTime - t_startTime).TotalMinutes;

            ExcelObjectTuttorial T_score = new ExcelObjectTuttorial()
            { Firstname = t_firstname, Lastname = t_lastname, Duration = t_duration, PlayedTime = t_startTime };
            score.createScoreforTuttorial(T_score);
            continue;

        case "2":
            modeMenuScript();
            logger.inputer("Choice: ");
            var gameLevelChoice = Console.ReadLine();
            if (gameLevelChoice == "4")
                break;
            Console.WriteLine("____________________________");
            switch (gameLevelChoice)
            {
                case "1":
                    Console.WriteLine("Welcome to the AI Easy Level...");
                    Console.WriteLine("____________________________");
                    var (easy_firstname, easy_lastname) = game.nameAndSurnameInput();
                    placement.placeAllComputerShip(computerGameArea, computerShips);
                    
                    string easy_placement_type_choice = null;
                    while (true)
                    {
                        Console.WriteLine("Do you want to place the ships randomly?");
                        logger.inputer("Yes (y), No (n): ");
                        easy_placement_type_choice = Console.ReadLine();
                        if (easy_placement_type_choice == "y" || easy_placement_type_choice == "n")
                            break;
                        else
                        {
                            logger.printWarning("Please enter valid input!!!");
                            continue;
                        }
                    }

                    if (easy_placement_type_choice == "y")
                        placement.placeAllUserShip(userGameArea, userShips);
                    else if (easy_placement_type_choice == "n")
                    {
                        for (int i = 0; i < userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(userGameArea, userShips, easy_firstname, easy_lastname);
                        }
                    }

                    bool easy_isUserWinner = false;
                    DateTime easy_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(computerGameArea);
                        game.printUserGameArea(userGameArea, easy_firstname, easy_lastname);
                        game.userShoot(computerGameArea, computerShips);
                        userRoundCounter++;
                        if (game.checkUserWin(computerShips,  easy_firstname, easy_lastname, computerGameArea, userGameArea, easy_isUserWinner, userRoundCounter).Item1)
                            break;
                        game.computerEasyLevelShoot(userGameArea, easy_firstname, easy_lastname);
                        computerRoundCounter++;
                        if (game.checkComputerWin(userShips, userGameArea, computerGameArea, easy_isUserWinner, computerRoundCounter).Item1)
                            break;
                    }

                    DateTime easy_endTime = DateTime.Now;
                    double easy_duration = (easy_endTime - easy_startTime).TotalMinutes;

                    ExcelObjectAI easy_score = new ExcelObjectAI()
                    { Firstname = easy_firstname, Lastname = easy_lastname, Duration = easy_duration, Mode = Modes.Mode.Easy.ToString(), isUserWinner = easy_isUserWinner , PlayedTime = easy_startTime};
                    score.createScoreforAI(easy_score);
                    break;
                case "2":
                    Console.WriteLine("Welcome to the AI Medium Level...");
                    Console.WriteLine("____________________________");
                    var (mid_firstname, mid_lastname) = game.nameAndSurnameInput();
                    placement.placeAllComputerShip(computerGameArea, computerShips);

                    string mid_placement_type_choice = null;
                    while (true)
                    {
                        Console.WriteLine("Do you want to place the ships randomly?");
                        logger.inputer("Yes (y), No (n): ");
                        mid_placement_type_choice = Console.ReadLine();
                        if (mid_placement_type_choice == "y" || mid_placement_type_choice == "n")
                            break;
                        else
                        {
                            logger.printWarning("Please enter valid input!!!");
                            continue;
                        }
                    }

                    if (mid_placement_type_choice == "y")
                        placement.placeAllUserShip(userGameArea, userShips);
                    else if (mid_placement_type_choice == "n")
                    {
                        for (int i = 0; i < userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(userGameArea, userShips, mid_firstname, mid_lastname);
                        }
                    }

                    bool mid_isUserWinner = false;
                    DateTime mid_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(computerGameArea);
                        game.printUserGameArea(userGameArea, mid_firstname, mid_lastname);
                        game.userShoot(computerGameArea, computerShips);
                        userRoundCounter++;
                        if (game.checkUserWin(computerShips, mid_firstname, mid_lastname, computerGameArea, userGameArea, mid_isUserWinner, userRoundCounter).Item1)
                            break;
                        game.computerHardLevelShoot(userGameArea, userShips, mid_firstname, mid_lastname, computerRoundCounter);
                        computerRoundCounter++;
                        if (game.checkComputerWin(userShips, computerGameArea, userGameArea, mid_isUserWinner, computerRoundCounter).Item1)
                            break;
                    }

                    DateTime mid_endTime = DateTime.Now;
                    double mid_duration = (mid_endTime - mid_startTime).TotalMinutes;

                    ExcelObjectAI mid_score = new ExcelObjectAI()
                    { Firstname = mid_firstname, Lastname = mid_lastname, Duration = mid_duration, Mode = Modes.Mode.Medium.ToString(), isUserWinner = mid_isUserWinner, PlayedTime = mid_startTime };
                    score.createScoreforAI(mid_score);

                    break;
                case "3":
                    Console.WriteLine("Welcome to the AI Hard Level...");
                    Console.WriteLine("____________________________");
                    var (hard_firstname, hard_lastname) = game.nameAndSurnameInput();
                    placement.placeAllComputerShip(computerGameArea, computerShips);

                    string hard_placement_type_choice = null;
                    while (true)
                    {
                        Console.WriteLine("Do you want to place the ships randomly?");
                        logger.inputer("Yes (y), No (n): ");
                        hard_placement_type_choice = Console.ReadLine();
                        if (hard_placement_type_choice == "y" || hard_placement_type_choice == "n")
                            break;
                        else
                        {
                            logger.printWarning("Please enter valid input!!!");
                            continue;
                        }
                    }

                    if (hard_placement_type_choice == "y")
                        placement.placeAllUserShip(userGameArea, userShips);
                    else if (hard_placement_type_choice == "n")
                    {
                        for (int i = 0; i < userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(userGameArea, userShips, hard_firstname, hard_lastname);
                        }
                    }

                    bool hard_isUserWinner = false;
                    DateTime hard_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(computerGameArea);
                        game.printUserGameArea(userGameArea, hard_firstname, hard_lastname);
                        game.userShoot(computerGameArea, computerShips);
                        userRoundCounter++;
                        if (game.checkUserWin(computerShips, hard_firstname, hard_lastname, computerGameArea, userGameArea, hard_isUserWinner, userRoundCounter).Item1)
                            break;
                        game.computerHardLevelShoot(userGameArea, userShips, hard_firstname, hard_lastname, computerRoundCounter);
                        computerRoundCounter++;
                        if (game.checkComputerWin(userShips, computerGameArea, userGameArea, hard_isUserWinner, computerRoundCounter).Item1)
                            break;
                        Console.WriteLine("_________________________");
                        for (int i = 0; i < 10; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                Console.Write(game.getPerfectProbability(userShips, userGameArea)[j, i] + " ");
                            }
                            Console.Write("\n");
                        }
                        Console.WriteLine("*****************************");
                    }

                    DateTime hard_endTime = DateTime.Now;
                    double hard_duration = (hard_endTime - hard_startTime).TotalMinutes;

                    ExcelObjectAI hard_score = new ExcelObjectAI()
                    { Firstname = hard_firstname, Lastname = hard_lastname, Duration = hard_duration, Mode = Modes.Mode.Hard.ToString(), isUserWinner = game.checkUserWin(computerShips, hard_firstname, hard_lastname, computerGameArea, userGameArea, hard_isUserWinner, userRoundCounter).Item2, PlayedTime = hard_startTime };
                    score.createScoreforAI(hard_score);

                    break;
                default:
                    Console.WriteLine("Please enter valid choice!!!");
                    continue;
            }
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

void modeMenuScript()
{
    Console.WriteLine("Please select the AI level");
    Console.WriteLine("____________________________");
    Console.WriteLine("1. Easy\n2. Medium\n3. Hard\n4. Quit");
}
