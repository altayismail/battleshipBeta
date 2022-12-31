using battleshipBeta;
using battleshipBeta.Database;
using Microsoft.AspNetCore.Mvc;

namespace battleship.Controllers
{
    public class ScoreController : Controller
    {
        private readonly Context _context;
        public ScoreController(Context context)
        {
            _context = context;
        }

        public IActionResult GetTuttorialScores()
        {
            Score scores = new Score(_context);
            var tuttorialScores = scores.getTuttorialQuery();
            return View(tuttorialScores);
        }

        public IActionResult GetAIScores()
        {
            Score scores = new Score(_context);
            var aiScores = scores.getAIQuery();
            return View(aiScores);
        }
    }
}
