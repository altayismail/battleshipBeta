using battleship.Abstract;
using battleship.DataAccess;
using battleship.Models;

namespace battleship.Concrete
{
    public class ComputerRepo : IComputerRepo
    {
        public ComputerRepo() 
        {
            using(var context = new MemoryDatabase())
            {
                context.SaveChanges();
            }
        }   
        public List<Computer> getComputers()
        {
            throw new NotImplementedException();
        }
    }
}
