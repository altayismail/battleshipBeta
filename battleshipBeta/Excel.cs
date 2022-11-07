using IronXL;
using Range = IronXL.Range;

namespace battleshipBeta
{
    internal class Excel
    {
        private readonly Logger _logger;

        public Excel(Logger logger)
        {
            _logger = logger;
        }

        int row_counter = 1;
        int counter = 0;

        string traning_path = "C:\\Users\\Ismail ALTAY\\source\\repos\\battleshipBeta\\battleshipBeta\\battleship_traning.xlsx";
        string ai_path = "C:\\Users\\Ismail ALTAY\\source\\repos\\battleshipBeta\\battleshipBeta\\battleship_ai.xlsx";
        public void readTraningExcelFile()
        {
            WorkBook workbook = WorkBook.Load(traning_path);
            WorkSheet sheet = workbook.WorkSheets.First();

            Range range = sheet["A2:C6"];
            Range range2 = range.SortByColumn("C", SortOrder.Descending);

            Console.WriteLine("Score Table of Traning Mode \n_______________________________________");
            Console.WriteLine("Name | Lastname | Duration (Min)");

            foreach (var cell in range2)
            {
                Console.WriteLine(cell.Value);
            }

            Console.WriteLine("________________________________");
        }

        public void writeTraningExcelFile(string firstname, string lastname, double duration)
        {
            WorkBook workbook = WorkBook.Load(traning_path);
            WorkSheet sheet = workbook.WorkSheets.First();

            foreach (var cell in sheet["A2:C6"])
            {
                if (string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    _logger.print("Cell is null");
                        row_counter = (row_counter / 3) + 2;
                    sheet["A" + row_counter.ToString()].StringValue = firstname;
                    sheet["B" + row_counter.ToString()].StringValue = lastname;
                    sheet["C" + row_counter.ToString()].DoubleValue = duration;
                    break;
                }
                else
                {
                    _logger.print("Row countter has worked.");
                    row_counter++;
                }
                    
            }
            workbook.Save();
        }

        public void readAIExcelFile()
        {
            WorkBook workbook = WorkBook.Load(ai_path);
            WorkSheet sheet = workbook.WorkSheets.First();

            List<RangeRow> rangeRows = new List<RangeRow>();
            rangeRows = sheet.Rows.ToList<RangeRow>();
            rangeRows.Remove(rangeRows[0]);
            rangeRows.OrderBy(x => x.SortByColumn(3, SortOrder.Ascending));

            Console.WriteLine("Score Table of Traning Mode \n_______________________________________");
            Console.WriteLine("Name   Lastname Duration (Min)");

            foreach (var rows in rangeRows)
            {
                Console.WriteLine(rows.ToString());
            }
            Console.WriteLine("________________________________");
        }

        public void writeAIExcelFile(string firstname, string lastname, string mode, double duration)
        {
            WorkBook workbook = WorkBook.Load(traning_path);
            WorkSheet sheet = workbook.WorkSheets.First();

            foreach (var cell in sheet["A2:C10"])
            {
                if (string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    _logger.print("Cell is null");
                    row_counter = (row_counter / 3) + 1;
                    sheet["A" + row_counter.ToString()].StringValue = firstname;
                    sheet["B" + row_counter.ToString()].StringValue = lastname;
                    sheet["C" + row_counter.ToString()].StringValue = mode;
                    sheet["D" + row_counter.ToString()].DoubleValue = duration;
                    break;
                }
                else
                {
                    _logger.print("Row countter has worked.");
                    row_counter++;
                }

            }
            workbook.Save();
        }
    }
}
