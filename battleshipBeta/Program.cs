﻿using battleshipBeta;
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
            int[,] computerGameArea = game.createGameArea();
            placement.placeAllComputerShip(computerGameArea, computerShips);
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
            modeMenuScript();
            logger.inputer("Choice: ");
            var gameLevelChoice = Console.ReadLine();
            if (gameLevelChoice == "4")
                break;
            switch (gameLevelChoice)
            {
                case "1":
                    Console.WriteLine("Welcome to the AI Easy Level...");
                    Console.WriteLine("____________________________");
                    var (easy_firstname, easy_lastname) = game.nameAndSurnameInput();
                    int[,] easy_computerGameArea = game.createGameArea();
                    placement.placeAllComputerShip(easy_computerGameArea, computerShips);
                    int[,] easy_userGameArea = game.createGameArea();

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
                        placement.placeAllUserShip(easy_userGameArea, userShips);
                    else if (easy_placement_type_choice == "n")
                    {
                        for (int i = 0; i < userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(easy_userGameArea, userShips);
                        }
                    }

                    DateTime easy_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(easy_computerGameArea);
                        game.printUserGameArea(easy_userGameArea);
                        game.userShoot(easy_computerGameArea, computerShips);
                        game.computerEasyLevelShoot(easy_userGameArea);
                        if (game.checkUserWin(easy_computerGameArea, easy_firstname, easy_lastname))
                            break;
                        if (game.checkComputerWin(easy_userGameArea))
                            break;
                        if (game.checkUserHoundredRound(easy_computerGameArea, easy_firstname, easy_lastname))
                            break;
                        if (game.checkComputerHoundredRound(easy_userGameArea))
                            break;
                    }

                    DateTime easy_endTime = DateTime.Now;
                    double easy_duration = (easy_endTime - easy_startTime).TotalMinutes;

                    ExcelObjectAI easy_score = new ExcelObjectAI()
                    { Firstname = easy_firstname, Lastname = easy_lastname, Duration = easy_duration, Mode = "Easy" };
                    score.createScoreforAI(easy_score);
                    break;
                case "2":
                    Console.WriteLine("Welcome to the AI Medium Level...");
                    Console.WriteLine("____________________________");
                    var (mid_firstname, mid_lastname) = game.nameAndSurnameInput();
                    int[,] mid_computerGameArea = game.createGameArea();
                    placement.placeAllComputerShip(mid_computerGameArea, computerShips);
                    int[,] mid_userGameArea = game.createGameArea();

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
                        placement.placeAllUserShip(mid_userGameArea, userShips);
                    else if (mid_placement_type_choice == "n")
                    {
                        for (int i = 0; i < userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(mid_userGameArea, userShips);
                        }
                    }

                    DateTime mid_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(mid_computerGameArea);
                        game.printUserGameArea(mid_userGameArea);
                        game.userShoot(mid_computerGameArea, computerShips);
                        game.computerHardLevelShoot(mid_userGameArea, userShips);
                        if (game.checkUserWin(mid_computerGameArea, mid_firstname, mid_lastname))
                            break;
                        if (game.checkComputerWin(mid_userGameArea))
                            break;
                        if (game.checkUserHoundredRound(mid_computerGameArea, mid_firstname, mid_lastname))
                            break;
                        if (game.checkComputerHoundredRound(mid_userGameArea))
                            break;
                    }

                    DateTime mid_endTime = DateTime.Now;
                    double mid_duration = (mid_endTime - mid_startTime).TotalMinutes;

                    ExcelObjectAI mid_score = new ExcelObjectAI()
                    { Firstname = mid_firstname, Lastname = mid_lastname, Duration = mid_duration, Mode = "Medium" };
                    score.createScoreforAI(mid_score);

                    break;
                    break;
                case "3":
                    Console.WriteLine("Welcome to the AI Hard Level...");
                    Console.WriteLine("____________________________");
                    var (hard_firstname, hard_lastname) = game.nameAndSurnameInput();
                    int[,] hard_computerGameArea = game.createGameArea();
                    placement.placeAllComputerShip(hard_computerGameArea, computerShips);
                    int[,] hard_userGameArea = game.createGameArea();

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
                        placement.placeAllUserShip(hard_userGameArea, userShips);
                    else if (hard_placement_type_choice == "n")
                    {
                        for (int i = 0; i < userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(hard_userGameArea, userShips);
                        }
                    }

                    DateTime hard_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(hard_computerGameArea);
                        game.printUserGameArea(hard_userGameArea);
                        game.userShoot(hard_computerGameArea, computerShips);
                        game.computerHardLevelShoot(hard_userGameArea, userShips);
                        if (game.checkUserWin(hard_computerGameArea, hard_firstname, hard_lastname))
                            break;
                        if (game.checkComputerWin(hard_userGameArea))
                            break;
                        if (game.checkUserHoundredRound(hard_computerGameArea, hard_firstname, hard_lastname))
                            break;
                        if (game.checkComputerHoundredRound(hard_userGameArea))
                            break;
                    }

                    DateTime hard_endTime = DateTime.Now;
                    double hard_duration = (hard_endTime - hard_startTime).TotalMinutes;

                    ExcelObjectAI hard_score = new ExcelObjectAI()
                    { Firstname = hard_firstname, Lastname = hard_lastname, Duration = hard_duration, Mode = "Hard" };
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
