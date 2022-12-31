using battleshipBeta.Database;
using battleshipBeta.Entities;
using System.Security.Cryptography.X509Certificates;

namespace battleshipBeta
{
    public class Score
    {
        private readonly Context _context;

        public Score(Context context)
        {
            _context = context;
        }

        public void createScoreforAI(ExcelObjectAI score) 
        {
            if(score is not null)
                _context.excelObjectAIs.Add(score);
            _context.SaveChanges();
        }

        public void createScoreforTuttorial(ExcelObjectTuttorial score)
        {
            if (score is not null)
                _context.excelObjectTuttorials.Add(score);
            _context.SaveChanges();
        }

        public void getListOfTuttorialScore()
        {
            var scores = _context.excelObjectTuttorials
                .ToList<ExcelObjectTuttorial>()
                .OrderBy(x => x.Duration)
                .Take(10);

            Console.WriteLine("Score List of Tuttorial Mode");
            Console.WriteLine("FIRSTNAME       LASTNAME        DURATION (min)          Played Day/Time");
            Console.WriteLine("________________________________________________________________________");
            foreach (var score in scores)
            {
                Console.Write(score.Firstname + "\t\t");
                Console.Write(score.Lastname + "\t\t");
                Console.Write(string.Format("{0:0.00}", score.Duration) + "\t\t\t");
                Console.Write(score.PlayedTime.ToString() + "\n");
            }
            Console.WriteLine("______________________________");
        }

        public List<ExcelObjectTuttorial> getTuttorialQuery()
        {
            var scores = _context.excelObjectTuttorials
                .ToList<ExcelObjectTuttorial>()
                .OrderBy(x => x.Duration)
                .Take(10);

            return scores.ToList<ExcelObjectTuttorial>();
        }

        public List<ExcelObjectAI> getAIQuery()
        {
            var scores = _context.excelObjectAIs
                .ToList<ExcelObjectAI>()
                .OrderBy(x => x.Duration)
                .Take(10);

            return scores.ToList<ExcelObjectAI>();
        }

        public void getListOfAIScore()
        {
            var scores = _context.excelObjectAIs
                .ToList<ExcelObjectAI>()
                .OrderBy(x => x.Duration)
                .Take(10);

            Console.WriteLine("Score List of AI Mode");
            Console.WriteLine("FIRSTNAME       LASTNAME        MODE            DURATION (min)          Played Day/Time         RESULT");
            Console.WriteLine("_______________________________________________________________________________________________________");
            foreach (var score in scores)
            {
                Console.Write(score.Firstname + "\t\t");
                Console.Write(score.Lastname + "\t\t");
                Console.Write(score.Mode + "\t\t");
                Console.Write(string.Format("{0:0.00}", score.Duration) + "\t\t\t");
                Console.Write(score.PlayedTime.ToString() + "\t");
                if (score.isUserWinner == false) 
                    Console.Write("Loser" + "\n");
                else
                    Console.Write("Winner" + "\n");

            }
            Console.WriteLine("______________________________");
        }
    }
}
