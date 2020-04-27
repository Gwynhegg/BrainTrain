using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BrainTrain.DataBase
{
    public class Database
    {
        readonly SQLite.SQLiteAsyncConnection _database;

        public Database(string conn_string)
        {
            _database = new SQLiteAsyncConnection(conn_string);
        }

        public Task<Components.User> GetUserAsync()
        {
            return _database.Table<Components.User>().FirstAsync();
        }

        public Task<int> InsertUserAsync(Components.User user)
        {
            return _database.InsertAsync(user);
        }
    }


}
