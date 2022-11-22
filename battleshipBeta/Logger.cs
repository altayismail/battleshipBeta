namespace battleshipBeta
{
    public class Logger
    {
        public void print(string message)
        {
            Console.WriteLine("[INFO]: " + message);
        }

        public void printWarning(string message)
        {
            Console.WriteLine("[Warning] : " + message);
        }

        public void gamePrint(string message)
        {
            Console.WriteLine("[GAME INFO] : " + message);
        }

        public void inputer(string message)
        {
            Console.Write("[INPUT] " + message);
        }
    }
}
