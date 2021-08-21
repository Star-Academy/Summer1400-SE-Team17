using Phase11;

namespace WebApplication.Services
{
    public class DataService
    {
        private readonly Database _database = new Database();
        public Database Database => _database;
     
    }
}