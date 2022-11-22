﻿using battleshipBeta;
using battleshipBeta.Database;
using battleshipBeta.Entities;

Random random = new Random();
Logger logger = new Logger();
Game game = new Game(random, logger);
Placement placement = new Placement(game, logger, random);
Context context = new Context();
Score score = new Score(context);

Console.WriteLine("Welcome To Battle Ship!!!!");
while (true)
{
    menuScript();
    logger.inputer("Choice: ");
    var gameModeChoice = Console.ReadLine();
    if (gameModeChoice == "6")
        break;

    Console.WriteLine("____________________________");
    switch (gameModeChoice)
    {
        case "1":
            List<Ship> t_computerShips = game.getListOfComputerShip(game.computerAdmiral, game.computerCruiser, game.computerDestroyer, game.comptuerDestroyer2, game.computerAssault);
            int[,] computerGameArea = game.createGameArea();
            placement.placeAllComputerShip(computerGameArea, t_computerShips);
            Console.WriteLine("Welcome to the Traning Mode...");
            Console.WriteLine("____________________________");
            var (t_firstname, t_lastname) = game.nameAndSurnameInput();

            DateTime t_startTime = DateTime.Now;
            while (true)
            {
                game.printComputerGameArea(computerGameArea);
                game.userShoot(computerGameArea, t_computerShips);
                if (game.checkUserWin(computerGameArea, computerGameArea, t_firstname, t_lastname, true).Item1)
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
            Console.WriteLine("____________________________");
            switch (gameLevelChoice)
            {
                case "1":
                    Console.WriteLine("Welcome to the AI Easy Level...");
                    Console.WriteLine("____________________________");
                    List<Ship> easy_userShips = game.getListOfUserShip(game.userAdmiral, game.userCruiser, game.userDestroyer, game.userDestroyer2, game.userAssault);
                    List<Ship> easy_computerShips = game.getListOfComputerShip(game.computerAdmiral, game.computerCruiser, game.computerDestroyer, game.comptuerDestroyer2, game.computerAssault);
                    var (easy_firstname, easy_lastname) = game.nameAndSurnameInput();
                    int[,] easy_computerGameArea = game.createGameArea();
                    placement.placeAllComputerShip(easy_computerGameArea, easy_computerShips);
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
                        placement.placeAllUserShip(easy_userGameArea, easy_userShips);
                    else if (easy_placement_type_choice == "n")
                    {
                        for (int i = 0; i < easy_userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(easy_userGameArea, easy_userShips, easy_firstname, easy_lastname);
                        }
                    }

                    bool easy_isUserWinner = false;
                    DateTime easy_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(easy_computerGameArea);
                        game.printUserGameArea(easy_userGameArea, easy_firstname, easy_lastname);
                        game.userShoot(easy_computerGameArea, easy_computerShips);
                        if (game.checkUserWin(easy_computerGameArea, easy_userGameArea, easy_firstname, easy_lastname, easy_isUserWinner).Item1)
                            break;
                        game.computerEasyLevelShoot(easy_userGameArea, easy_firstname, easy_lastname);
                        if (game.checkComputerWin(easy_userGameArea, easy_computerGameArea, easy_isUserWinner, easy_firstname, easy_firstname).Item1)
                            break;
                    }

                    DateTime easy_endTime = DateTime.Now;
                    double easy_duration = (easy_endTime - easy_startTime).TotalMinutes;

                    ExcelObjectAI easy_score = new ExcelObjectAI()
                    { Firstname = easy_firstname, Lastname = easy_lastname, Duration = easy_duration, Mode = "Easy", isUserWinner = easy_isUserWinner };
                    score.createScoreforAI(easy_score);
                    break;
                case "2":
                    Console.WriteLine("Welcome to the AI Medium Level...");
                    Console.WriteLine("____________________________");
                    List<Ship> mid_userShips = game.getListOfUserShip(game.userAdmiral, game.userCruiser, game.userDestroyer, game.userDestroyer2, game.userAssault);
                    List<Ship> mid_computerShips = game.getListOfComputerShip(game.computerAdmiral, game.computerCruiser, game.computerDestroyer, game.comptuerDestroyer2, game.computerAssault);
                    var (mid_firstname, mid_lastname) = game.nameAndSurnameInput();
                    int[,] mid_computerGameArea = game.createGameArea();
                    placement.placeAllComputerShip(mid_computerGameArea, mid_computerShips);
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
                        placement.placeAllUserShip(mid_userGameArea, mid_userShips);
                    else if (mid_placement_type_choice == "n")
                    {
                        for (int i = 0; i < mid_userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(mid_userGameArea, mid_userShips, mid_firstname, mid_lastname);
                        }
                    }

                    bool mid_isUserWinner = false;
                    DateTime mid_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(mid_computerGameArea);
                        game.printUserGameArea(mid_userGameArea, mid_firstname, mid_lastname);
                        game.userShoot(mid_computerGameArea, mid_computerShips);
                        if (game.checkUserWin(mid_computerGameArea, mid_userGameArea, mid_firstname, mid_lastname, mid_isUserWinner).Item1)
                            break;
                        game.computerHardLevelShoot(mid_userGameArea, mid_userShips, mid_firstname, mid_lastname);
                        if (game.checkComputerWin(mid_userGameArea, mid_computerGameArea, mid_isUserWinner, mid_firstname, mid_lastname).Item1)
                            break;
                    }

                    DateTime mid_endTime = DateTime.Now;
                    double mid_duration = (mid_endTime - mid_startTime).TotalMinutes;

                    ExcelObjectAI mid_score = new ExcelObjectAI()
                    { Firstname = mid_firstname, Lastname = mid_lastname, Duration = mid_duration, Mode = "Medium", isUserWinner = mid_isUserWinner };
                    score.createScoreforAI(mid_score);

                    break;
                case "3":
                    Console.WriteLine("Welcome to the AI Hard Level...");
                    Console.WriteLine("____________________________");

                    List<Ship> hard_userShips = game.getListOfUserShip(game.userAdmiral, game.userCruiser, game.userDestroyer, game.userDestroyer2, game.userAssault);
                    List<Ship> hard_computerShips = game.getListOfComputerShip(game.computerAdmiral, game.computerCruiser, game.computerDestroyer, game.comptuerDestroyer2, game.computerAssault);
                    var (hard_firstname, hard_lastname) = game.nameAndSurnameInput();
                    int[,] hard_computerGameArea = game.createGameArea();
                    placement.placeAllComputerShip(hard_computerGameArea, hard_computerShips);
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
                        placement.placeAllUserShip(hard_userGameArea, hard_userShips);
                    else if (hard_placement_type_choice == "n")
                    {
                        for (int i = 0; i < hard_userShips.Count; i++)
                        {
                            placement.placementMechanismForUser(hard_userGameArea, hard_userShips, hard_firstname, hard_lastname);
                        }
                    }

                    bool hard_isUserWinner = false;
                    DateTime hard_startTime = DateTime.Now;
                    while (true)
                    {
                        game.printComputerGameArea(hard_computerGameArea);
                        game.printUserGameArea(hard_userGameArea, hard_firstname, hard_lastname);
                        game.userShoot(hard_computerGameArea, hard_computerShips);
                        if (game.checkUserWin(hard_computerGameArea, hard_userGameArea, hard_firstname, hard_lastname, hard_isUserWinner).Item1)
                            break;
                        game.computerHardLevelShoot(hard_userGameArea, hard_userShips, hard_firstname, hard_lastname);
                        if (game.checkComputerWin(hard_userGameArea, hard_computerGameArea, hard_isUserWinner, hard_firstname, hard_lastname).Item1)
                            break;
                    }

                    DateTime hard_endTime = DateTime.Now;
                    double hard_duration = (hard_endTime - hard_startTime).TotalMinutes;

                    ExcelObjectAI hard_score = new ExcelObjectAI()
                    { Firstname = hard_firstname, Lastname = hard_lastname, Duration = hard_duration, Mode = "Hard", isUserWinner = game.checkUserWin(hard_computerGameArea, hard_userGameArea, hard_firstname, hard_lastname, hard_isUserWinner).Item2 };
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
        case "5":
            Console.WriteLine("Welcome to the AI Simulator...");
            Console.WriteLine("____________________________");

            List<Ship> computerShips2 = game.getListOfComputerShip(game.computerAdmiral, game.computerCruiser, game.computerDestroyer, game.comptuerDestroyer2, game.computerAssault);
            List<Ship> computerShips3 = game.getListOfComputerShip(game.computerAdmiral, game.computerCruiser, game.computerDestroyer, game.comptuerDestroyer2, game.computerAssault);
            int[,] computerGameArea3 = game.createGameArea();
            int[,] computerGameArea2 = game.createGameArea();
            placement.placeAllComputerShip(computerGameArea3, computerShips3);
            placement.placeAllComputerShip(computerGameArea2, computerShips2);
          
            while (true)
            {
                game.printUserGameArea(computerGameArea3, "Computer 1", "");
                game.printUserGameArea(computerGameArea2, "Computer 2", "");
                game.computerHardLevelShoot(computerGameArea3, computerShips3, "Computer 1", "");
                if (game.checkComputerWin(computerGameArea2, computerGameArea3, false, "Computer 2", "").Item1)
                    break;
                game.computerHardLevelShoot(computerGameArea2, computerShips2, "Computer 1", "");
                if (game.checkComputerWin(computerGameArea3, computerGameArea2, false, "Computer 2", "").Item1)
                    break;
                Thread.Sleep(500);
            }

            logger.gamePrint("Round Number: " + game.computerRoundCounter);
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
    Console.WriteLine("1. Traning Mode\n2. AI Mode\n3. Past Results of Traning Mode\n4. Past Result of AI Mode\n5. AI Simulator\n6. Quit");
}

void modeMenuScript()
{
    Console.WriteLine("Please select the AI level");
    Console.WriteLine("____________________________");
    Console.WriteLine("1. Easy\n2. Medium\n3. Hard\n4. Quit");
}
