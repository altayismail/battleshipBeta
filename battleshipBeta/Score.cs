using battleshipBeta.Database;
using battleshipBeta.Entities;

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
                .Take(5);

            Console.WriteLine("Score List of Tuttorial Mode");
            Console.WriteLine("FIRSTNAME       LASTNAME        DURATION (min)");
            Console.WriteLine("______________________________________________");
            foreach (var score in scores)
            {
                Console.Write(score.Firstname + "\t\t");
                Console.Write(score.Lastname + "\t\t");
                Console.Write(string.Format("{0:0.00}", score.Duration) + "\n");
            }
            Console.WriteLine("______________________________");
        }

        public void getListOfAIScore()
        {
            var scores = _context.excelObjectAIs
                .ToList<ExcelObjectAI>()
                .OrderBy(x => x.Duration)
                .Take(5);

            Console.WriteLine("Score List of AI Mode");
            Console.WriteLine("FIRSTNAME       LASTNAME        MODE            DURATION (min)");
            Console.WriteLine("______________________________________________________________");
            foreach (var score in scores)
            {
                Console.Write(score.Firstname + "\t\t");
                Console.Write(score.Lastname + "\t\t");
                Console.Write(score.Mode + "\t\t");
                Console.Write(string.Format("{0:0.00}", score.Duration) + "\n");
            }
            Console.WriteLine("______________________________");
        }


    }
}
