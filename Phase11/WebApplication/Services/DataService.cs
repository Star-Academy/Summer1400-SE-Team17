using Phase11;
using Phase11.model.database;

namespace WebApplication.Services
{
    public class DataService
    {
        private readonly DataBase _dataBase = new DataBase();
        public DataBase DataBase => _dataBase;
     
    }
}