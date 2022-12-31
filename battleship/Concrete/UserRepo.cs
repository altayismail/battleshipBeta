using battleship.Abstract;
using battleship.DataAccess;
using battleship.Models;

namespace battleship.Concrete
{
    public class UserRepo : IUserRepo
    {
        public UserRepo() 
        {
            using(var context = new MemoryDatabase())
            {
                context.SaveChanges();
            }
        }

        public List<User> getUsers()
        {
            throw new NotImplementedException();
        }
    }
}
