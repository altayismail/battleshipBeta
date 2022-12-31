using battleship.DataAccess;
using battleship.Models;
using battleshipBeta;
using battleshipBeta.Database;
using battleshipBeta.Entities;
using Microsoft.AspNetCore.Mvc;

namespace battleship.Controllers
{
    public class GameController : Controller
    {
        Game game = new Game(new Random(), new Logger());
        Placement placement = new Placement(new Game(new Random(), new Logger()));

        private readonly Context _context;
        private readonly MemoryDatabase _memoryDatabase;

        public GameController(Context context, MemoryDatabase memoryDatabase)
        {
            _context = context;
            _memoryDatabase= memoryDatabase;
        }
        
        public IActionResult UserPlacementforHard()
        {
            ViewData["UserGameArea"] = Board.UserGameArea;
            return View();
        }
        [HttpPost]
        public IActionResult UserPlacementforHard(User user)
        {
            ViewData["UserGameArea"] = Board.UserGameArea;
            var userShips = game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer);
            if (user.isRandomPlacement == true)
                placement.placeAllUserShip(Board.UserGameArea, userShips);
            var ship = userShips.Where(x => x.Name.Replace(" ","") == user.ShipChoice).Single();

            if(ship.isShipPlaced == true)
            {
                ViewBag.Warning = $"{ship.Name} has already been placed!!!";
                return View();
            }
            ship.VerorHor = bool.Parse(user.strVerOrHor);
            if (ship.VerorHor == true)
            {
                (ship.LocationIndex, ship.StartIndex) = (user.YCoordinate, user.XCoordinate);
            }
            else if (ship.VerorHor == false)
            {
                (ship.StartIndex, ship.LocationIndex) = (user.YCoordinate, user.XCoordinate);
            }

            ship.EndIndex = ship.StartIndex + ship.Length;
            if (ship.EndIndex >= 10 || ship.EndIndex <= 0)
            {
                ViewBag.ErrorMessage = ($"You choose wrong start index for ship, the length of the ship is {ship.Length}. Please try again.");
                return View();
            }
            //Horizontal and Vertical placement choice end
            //check vertical or horizontal placement
            if (ship.VerorHor == false)
            {
                if (placement.checkOneSquareRuleinHorizontal(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                {
                    ViewBag.ErrorMessage = ($"There is an overlap on ships or placement denied because of One Square Rule.");
                    return View();
                }
                placement.horizontalPlacement(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                ship.isShipPlaced = true;
                placement.setShipLocation(ship);
            }
            else
            {
                if (placement.checkOneSquareRuleinVertical(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                {
                    ViewBag.ErrorMessage = ($"There is an overlap on ships or placement denied because of One Square Rule.");
                    return View();
                }
                placement.verticalPlacement(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                ship.isShipPlaced = true;
                placement.setShipLocation(ship);
            }

            if(userShips.All(x => x.isShipPlaced == true))
                return RedirectToAction("HardLevel");
            return RedirectToAction("UserPlacementforHard");
        }

        public IActionResult UserRandomPlacementforHard()
        {
            var userShips = game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer);
            foreach (var ship in userShips)
            {
                placement.placementMechanism(Board.UserGameArea, ship);
            }
            return RedirectToAction("HardLevel");
        }

        public IActionResult HardLevel()
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["ComputerRoundCounter"] = _memoryDatabase.Computers.ToList().Last().ComputerRoundCounter;
            ViewData["ComputerGameArea"] = Board.ComputerGameArea;
            ViewData["UserGameArea"] = Board.UserGameArea;
            return View();
        }
        [HttpPost]
        public IActionResult HardLevel(CoordinateIndex index)
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["ComputerRoundCounter"] = _memoryDatabase.Computers.ToList().Last().ComputerRoundCounter;

            if(GameSettings.isUserTurn == true)
            {
                (ViewData["ShipInfo"],GameSettings.isUserTurn) = game.userShoot(Board.ComputerGameArea,
                game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer),
                index.X, index.Y);

                if (game.checkUserWin(game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer), Board.ComputerGameArea))
                {
                    Score score = new Score(_context);
                    _memoryDatabase.Users.Last().EndTime = DateTime.Now;
                    _memoryDatabase.SaveChanges();
                    ExcelObjectAI ai_score = new ExcelObjectAI()
                    { Firstname = _memoryDatabase.Users.Last().Firstname, Lastname = _memoryDatabase.Users.Last().Lastname, Duration = (_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes, PlayedTime = _memoryDatabase.Users.Last().StartTime, isUserWinner = true, Mode = "Hard" };
                    score.createScoreforAI(ai_score);
                    return RedirectToAction("GetAIScores", "Score");
                }
            }

            if(GameSettings.isUserTurn == false)
            {
                while(GameSettings.isUserTurn == false)
                {
                    (GameSettings.lastShootedIndex,
                    GameSettings.shipFoundStartPoint,
                    GameSettings.isShipFound,
                    GameSettings.lastShotSuccess,
                    GameSettings.firstSuccessShotChecker,
                    GameSettings.threeAreaChecker,
                    GameSettings.direction,
                    GameSettings.randomActivater,
                    GameSettings.isUserTurn,
                    GameSettings.ShipInfo) = game.computerHardLevelShoot(Board.UserGameArea,
                    game.getListOfUserShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer),
                    _memoryDatabase.Computers.Last().ComputerRoundCounter++,
                    GameSettings.lastShootedIndex,
                    GameSettings.shipFoundStartPoint,
                    GameSettings.isShipFound,
                    GameSettings.lastShotSuccess,
                    GameSettings.firstSuccessShotChecker,
                    GameSettings.threeAreaChecker,
                    GameSettings.direction,
                    GameSettings.randomActivater,
                    GameSettings.isUserTurn);

                    ViewBag.ShipInfo = GameSettings.ShipInfo;

                    if (game.checkComputerWin(game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer), Board.UserGameArea))
                    {
                        Score score = new Score(_context);
                        _memoryDatabase.Users.Last().EndTime = DateTime.Now;
                        _memoryDatabase.SaveChanges();
                        ExcelObjectAI ai_score = new ExcelObjectAI()
                        { Firstname = _memoryDatabase.Users.Last().Firstname, Lastname = _memoryDatabase.Users.Last().Lastname, Duration = (_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes, PlayedTime = _memoryDatabase.Users.Last().StartTime, isUserWinner = false, Mode = "Hard" };
                        score.createScoreforAI(ai_score);
                        return RedirectToAction("GetAIScores", "Score");
                    }
                }
            }

            _memoryDatabase.Users.Last().UserRoundCounter++;
            _memoryDatabase.SaveChanges();

            return RedirectToAction("HardLevel", "Game");
        }

        public IActionResult UserPlacementforMedium()
        {
            ViewData["UserGameArea"] = Board.UserGameArea;
            return View();
        }
        [HttpPost]
        public IActionResult UserPlacementforMedium(User user)
        {
            ViewData["UserGameArea"] = Board.UserGameArea;
            var userShips = game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer);
            if (user.isRandomPlacement == true)
                placement.placeAllUserShip(Board.UserGameArea, userShips);
            var ship = userShips.Where(x => x.Name.Replace(" ", "") == user.ShipChoice).Single();

            if (ship.isShipPlaced == true)
            {
                ViewBag.Warning = $"{ship.Name} has already been placed!!!";
                return View();
            }
            ship.VerorHor = bool.Parse(user.strVerOrHor);
            if (ship.VerorHor == true)
            {
                (ship.LocationIndex, ship.StartIndex) = (user.YCoordinate, user.XCoordinate);
            }
            else if (ship.VerorHor == false)
            {
                (ship.StartIndex, ship.LocationIndex) = (user.YCoordinate, user.XCoordinate);
            }

            ship.EndIndex = ship.StartIndex + ship.Length;
            if (ship.EndIndex >= 10 || ship.EndIndex <= 0)
            {
                ViewBag.ErrorMessage = ($"You choose wrong start index for ship, the length of the ship is {ship.Length}. Please try again.");
                return View();
            }
            //Horizontal and Vertical placement choice end
            //check vertical or horizontal placement
            if (ship.VerorHor == false)
            {
                if (placement.checkOneSquareRuleinHorizontal(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                {
                    ViewBag.ErrorMessage = ($"There is an overlap on ships or placement denied because of One Square Rule.");
                    return View();
                }
                placement.horizontalPlacement(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                ship.isShipPlaced = true;
                placement.setShipLocation(ship);
            }
            else
            {
                if (placement.checkOneSquareRuleinVertical(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                {
                    ViewBag.ErrorMessage = ($"There is an overlap on ships or placement denied because of One Square Rule.");
                    return View();
                }
                placement.verticalPlacement(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                ship.isShipPlaced = true;
                placement.setShipLocation(ship);
            }

            if (userShips.All(x => x.isShipPlaced == true))
                return RedirectToAction("MediumLevel");
            return RedirectToAction("UserPlacementforMedium");
        }

        public IActionResult UserRandomPlacementforMedium()
        {
            var userShips = game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer);
            foreach (var ship in userShips)
            {
                placement.placementMechanism(Board.UserGameArea, ship);
            }
            return RedirectToAction("MediumLevel");
        }

        public IActionResult MediumLevel()
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["ComputerRoundCounter"] = _memoryDatabase.Computers.ToList().Last().ComputerRoundCounter;
            ViewData["ComputerGameArea"] = Board.ComputerGameArea;
            ViewData["UserGameArea"] = Board.UserGameArea;
            return View();
        }
        [HttpPost]
        public IActionResult MediumLevel(CoordinateIndex index)
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["ComputerRoundCounter"] = _memoryDatabase.Computers.ToList().Last().ComputerRoundCounter;

            if (GameSettings.isUserTurn == true)
            {
                (ViewData["ShipInfo"], GameSettings.isUserTurn) = game.userShoot(Board.ComputerGameArea,
                game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer),
                index.X, index.Y);

                if (game.checkUserWin(game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer), Board.ComputerGameArea))
                {
                    Score score = new Score(_context);
                    _memoryDatabase.Users.Last().EndTime = DateTime.Now;
                    _memoryDatabase.SaveChanges();
                    ExcelObjectAI ai_score = new ExcelObjectAI()
                    { Firstname = _memoryDatabase.Users.Last().Firstname, Lastname = _memoryDatabase.Users.Last().Lastname, Duration = (_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes, PlayedTime = _memoryDatabase.Users.Last().StartTime, isUserWinner = true, Mode = "Medium" };
                    score.createScoreforAI(ai_score);
                    return RedirectToAction("GetAIScores", "Score");
                }
            }

            if (GameSettings.isUserTurn == false)
            {
                while (GameSettings.isUserTurn == false)
                {
                    (GameSettings.lastShootedIndex,
                    GameSettings.shipFoundStartPoint,
                    GameSettings.isShipFound,
                    GameSettings.lastShotSuccess,
                    GameSettings.firstSuccessShotChecker,
                    GameSettings.threeAreaChecker,
                    GameSettings.direction,
                    GameSettings.randomActivater,
                    GameSettings.isUserTurn,
                    GameSettings.ShipInfo) = game.computerMediumLevelShoot(Board.UserGameArea,
                    game.getListOfUserShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer),
                    _memoryDatabase.Computers.Last().ComputerRoundCounter++,
                    GameSettings.lastShootedIndex,
                    GameSettings.shipFoundStartPoint,
                    GameSettings.isShipFound,
                    GameSettings.lastShotSuccess,
                    GameSettings.firstSuccessShotChecker,
                    GameSettings.threeAreaChecker,
                    GameSettings.direction,
                    GameSettings.randomActivater,
                    GameSettings.isUserTurn);
                    ViewBag.ShipInfo = GameSettings.ShipInfo;

                    if (game.checkComputerWin(game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer), Board.UserGameArea))
                    {
                        Score score = new Score(_context);
                        _memoryDatabase.Users.Last().EndTime = DateTime.Now;
                        _memoryDatabase.SaveChanges();
                        ExcelObjectAI ai_score = new ExcelObjectAI()
                        { Firstname = _memoryDatabase.Users.Last().Firstname, Lastname = _memoryDatabase.Users.Last().Lastname, Duration = (_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes, PlayedTime = _memoryDatabase.Users.Last().StartTime, isUserWinner = false, Mode = "Medium" };
                        score.createScoreforAI(ai_score);
                        return RedirectToAction("GetAIScores", "Score");
                    }
                }
            }

            _memoryDatabase.Users.Last().UserRoundCounter++;
            _memoryDatabase.SaveChanges();

            return RedirectToAction("MediumLevel", "Game");
        }
        public IActionResult UserPlacementforEasy()
        {
            ViewData["UserGameArea"] = Board.UserGameArea;
            return View();
        }
        [HttpPost]
        public IActionResult UserPlacementforEasy(User user)
        {
            ViewData["UserGameArea"] = Board.UserGameArea;
            var userShips = game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer);
            if (user.isRandomPlacement == true)
                placement.placeAllUserShip(Board.UserGameArea, userShips);
            var ship = userShips.Where(x => x.Name.Replace(" ", "") == user.ShipChoice).Single();

            if (ship.isShipPlaced == true)
            {
                ViewBag.Warning = $"{ship.Name} has already been placed!!!";
                return View();
            }
            ship.VerorHor = bool.Parse(user.strVerOrHor);
            if (ship.VerorHor == true)
            {
                (ship.LocationIndex, ship.StartIndex) = (user.YCoordinate, user.XCoordinate);
            }
            else if (ship.VerorHor == false)
            {
                (ship.StartIndex, ship.LocationIndex) = (user.YCoordinate, user.XCoordinate);
            }

            ship.EndIndex = ship.StartIndex + ship.Length;
            if (ship.EndIndex >= 10 || ship.EndIndex <= 0)
            {
                ViewBag.ErrorMessage = ($"You choose wrong start index for ship, the length of the ship is {ship.Length}. Please try again.");
                return View();
            }
            //Horizontal and Vertical placement choice end
            //check vertical or horizontal placement
            if (ship.VerorHor == false)
            {
                if (placement.checkOneSquareRuleinHorizontal(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                {
                    ViewBag.ErrorMessage = ($"There is an overlap on ships or placement denied because of One Square Rule.");
                    return View();
                }
                placement.horizontalPlacement(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                ship.isShipPlaced = true;
                placement.setShipLocation(ship);
            }
            else
            {
                if (placement.checkOneSquareRuleinVertical(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex))
                {
                    ViewBag.ErrorMessage = ($"There is an overlap on ships or placement denied because of One Square Rule.");
                    return View();
                }
                placement.verticalPlacement(Board.UserGameArea, ship.StartIndex, ship.EndIndex, ship.LocationIndex);
                ship.isShipPlaced = true;
                placement.setShipLocation(ship);
            }

            if (userShips.All(x => x.isShipPlaced == true))
                return RedirectToAction("EasyLevel");
            return RedirectToAction("UserPlacementforEasy");
        }
        public IActionResult UserRandomPlacementforEasy()
        {
            var userShips = game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer);
            foreach (var ship in userShips)
            {
                placement.placementMechanism(Board.UserGameArea, ship);
            }
            return RedirectToAction("EasyLevel");
        }
        public IActionResult EasyLevel()
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["ComputerRoundCounter"] = _memoryDatabase.Computers.ToList().Last().ComputerRoundCounter;
            ViewData["ComputerGameArea"] = Board.ComputerGameArea;
            ViewData["UserGameArea"] = Board.UserGameArea;
            return View();
        }
        [HttpPost]
        public IActionResult EasyLevel(CoordinateIndex index)
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["ComputerRoundCounter"] = _memoryDatabase.Computers.ToList().Last().ComputerRoundCounter;

            if (GameSettings.isUserTurn == true)
            {
                (ViewData["ShipInfo"], GameSettings.isUserTurn) = game.userShoot(Board.ComputerGameArea,
                game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer),
                index.X, index.Y);

                if (game.checkUserWin(game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer), Board.ComputerGameArea))
                {
                    Score score = new Score(_context);
                    _memoryDatabase.Users.Last().EndTime = DateTime.Now;
                    _memoryDatabase.SaveChanges();
                    ExcelObjectAI ai_score = new ExcelObjectAI()
                    { Firstname = _memoryDatabase.Users.Last().Firstname, Lastname = _memoryDatabase.Users.Last().Lastname, Duration = (_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes, PlayedTime = _memoryDatabase.Users.Last().StartTime, isUserWinner = true, Mode = "Easy" };
                    score.createScoreforAI(ai_score);
                    return RedirectToAction("GetAIScores", "Score");
                }
            }

            if (GameSettings.isUserTurn == false)
            {
                while (GameSettings.isUserTurn == false)
                {
                    GameSettings.isUserTurn = game.computerEasyLevelShoot(Board.UserGameArea, GameSettings.isUserTurn);

                    if (game.checkComputerWin(game.getListOfComputerShip(Shipper.userAircraft, Shipper.userBattleship, Shipper.userCruiser, Shipper.userSubmarine, Shipper.userDestroyer), Board.UserGameArea))
                    {
                        Score score = new Score(_context);
                        _memoryDatabase.Users.Last().EndTime = DateTime.Now;
                        _memoryDatabase.SaveChanges();
                        ExcelObjectAI ai_score = new ExcelObjectAI()
                        { Firstname = _memoryDatabase.Users.Last().Firstname, Lastname = _memoryDatabase.Users.Last().Lastname, Duration = (_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes, PlayedTime = _memoryDatabase.Users.Last().StartTime, isUserWinner = false, Mode = "Easy" };
                        score.createScoreforAI(ai_score);
                        return RedirectToAction("GetAIScores", "Score");
                    }
                }
            }

            _memoryDatabase.Users.Last().UserRoundCounter++;
            _memoryDatabase.SaveChanges();

            return RedirectToAction("EasyLevel", "Game");
        }

        public IActionResult GetUserNameforAI()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetUserNameforAI(User user)
        {
            _memoryDatabase.Users.Add(user);
            user.StartTime = DateTime.Now;
            _memoryDatabase.SaveChanges();
            _memoryDatabase.Computers.Add(new Computer
            {
                ComputerRoundCounter = 0,
                isWinner = false
            });
            _memoryDatabase.SaveChanges();
            Board.UserGameArea = game.createGameArea();
            Board.ComputerGameArea = game.createGameArea();

            var ships = game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer);
            foreach (var ship in ships)
            {
                placement.placementMechanism(Board.ComputerGameArea, ship);
            }
            

            if (user.AILevel == "Easy" && user.isRandomPlacement == false)
                return RedirectToAction("UserPlacementforEasy");
            else if(user.AILevel == "Easy" && user.isRandomPlacement == true)
                return RedirectToAction("UserRandomPlacementforEasy");
            else if (user.AILevel == "Medium" && user.isRandomPlacement == false)
                return RedirectToAction("UserPlacementforMedium");
            else if(user.AILevel == "Medium" && user.isRandomPlacement == true)
                return RedirectToAction("UserRandomPlacementforMedium");
            else if(user.AILevel == "Hard" && user.isRandomPlacement == true)
                    return RedirectToAction("UserRandomPlacementforHard");
            return RedirectToAction("UserPlacementforHard");
        }

        public IActionResult GetUserName()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetUserName(User user)
        {
            _memoryDatabase.Users.Add(user);
            user.StartTime = DateTime.Now;
            _memoryDatabase.SaveChanges();
            Board.ComputerGameArea = game.createGameArea();
            var ships = game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer);
            foreach (var ship in ships)
            {
                placement.placementMechanism(Board.ComputerGameArea, ship);
            }
            return RedirectToAction("TuttorialMode");
        }
        public IActionResult TuttorialMode()
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["GameArea"] = Board.ComputerGameArea;
            return View();
        }
        [HttpPost]
        public IActionResult TuttorialMode(CoordinateIndex index)
        {
            ViewData["UserName"] = _memoryDatabase.Users.ToList().Last().Firstname + " " +
                                    _memoryDatabase.Users.ToList().Last().Lastname;
            ViewData["UserRoundCounter"] = _memoryDatabase.Users.ToList().Last().UserRoundCounter;
            ViewData["GameArea"] = Board.ComputerGameArea;

            ViewData["ShipInfo"] = game.userShoot(Board.ComputerGameArea,
                game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer),
                index.X, index.Y);
            if (game.checkUserWin(game.getListOfComputerShip(Shipper.computerAircraft, Shipper.computerBattleship, Shipper.computerCruiser, Shipper.computerSubmarine, Shipper.computerDestroyer), Board.ComputerGameArea))
            {
                Score score = new Score(_context);
                _memoryDatabase.Users.Last().EndTime = DateTime.Now;
                _memoryDatabase.SaveChanges();
                ExcelObjectTuttorial T_score = new ExcelObjectTuttorial()
                { Firstname = _memoryDatabase.Users.Last().Firstname, Lastname = _memoryDatabase.Users.Last().Lastname, Duration = (_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes, PlayedTime = _memoryDatabase.Users.Last().StartTime };
                score.createScoreforTuttorial(T_score);
                ViewBag.WinMessage = $"Congrats, You found the all the ships in {(_memoryDatabase.Users.Last().EndTime - _memoryDatabase.Users.Last().StartTime).TotalMinutes} minutes.";
                return RedirectToAction("GetTuttorialScores", "Score");
            }

            _memoryDatabase.Users.Last().UserRoundCounter++;
            _memoryDatabase.SaveChanges();

            return RedirectToAction("TuttorialMode","Game");
        }
    }
}
