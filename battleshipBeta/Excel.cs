using IronXL;

namespace battleshipBeta
{
    internal class Excel
    {
        int row_counter = 2;
        int counter = 0;
        string traning_path = "C:\\Users\\Ismail ALTAY\\source\\repos\\battleshipBeta\\battleshipBeta\\battleship_traning.xlsx";
        string ai_path = "C:\\Users\\Ismail ALTAY\\source\\repos\\battleshipBeta\\battleshipBeta\\battleship_ai.xlsx";
        public void readTraningExcelFile()
        {
            WorkBook workbook = WorkBook.Load(traning_path);
            WorkSheet sheet = workbook.WorkSheets.First();
            Console.WriteLine("Score Table of Traning Mode \n _______________________________________");
            Console.WriteLine(" First Name | Last Name | Duration (Min)|");

            foreach (var cell in sheet["A2:C10"])
            {
                if (string.IsNullOrEmpty(cell.StringValue))
                    continue;
                Console.Write($" {cell.Value} |");
                counter++;
                if(counter == 3)
                {
                    counter = 0;
                    Console.WriteLine("\n");
                }
            }
            Console.WriteLine("________________________________");
        }

        public void writeTraningExcelFile(string firstname, string lastname, double duration)
        {
            WorkBook workbook = WorkBook.Load(traning_path);
            WorkSheet sheet = workbook.WorkSheets.First();

            sheet["A" + row_counter.ToString()].StringValue = firstname;
            sheet["B" + row_counter.ToString()].StringValue = lastname;
            sheet["C" + row_counter.ToString()].DoubleValue = duration;
            workbook.Save();
        }

        public void readAIExcelFile()
        {
            WorkBook workbook = WorkBook.Load(ai_path);
            WorkSheet sheet = workbook.WorkSheets.First();
            Console.WriteLine("Score Table of AI Mode \n _______________________________________");
            Console.WriteLine(" First Name | Last Name | Mode | Duration (Min)|");

            foreach (var cell in sheet["A2:D10"])
            {
                if (string.IsNullOrEmpty(cell.StringValue))
                    continue;
                Console.Write($" {cell.Value} |");
                counter++;
                if (counter == 4)
                {
                    counter = 0;
                    Console.WriteLine("\n");
                }
            }
            Console.WriteLine("________________________________");
        }

        public void writeAIExcelFile(string firstname, string lastname, string mode, double duration)
        {
            WorkBook workbook = WorkBook.Load(traning_path);
            WorkSheet sheet = workbook.WorkSheets.First();

            sheet["A" + row_counter.ToString()].StringValue = firstname;
            sheet["B" + row_counter.ToString()].StringValue = lastname;
            sheet["C" + row_counter.ToString()].StringValue = mode;
            sheet["D" + row_counter.ToString()].DoubleValue = duration;
            workbook.Save();
        }
    }
}
