using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Linq;
using Xamarin.Forms;


namespace BrainTrain.DataBase
{
    //Класс базы данных, отвечающий за соединение, создание и открытие таблиц, выгрузку и запись данных в базу.
    public class Database
    {
        //Переменная соединения с базой данных.
        readonly SQLiteAsyncConnection _database;

        //Конструктор класса, принимающий путь до базы данных в качестве параметра.
        public Database(string path)
        {
            //Создаем соединение и необходимые таблицы, если они отсутствуют.
            _database = new SQLiteAsyncConnection(path);
            _database.CreateTableAsync <Components.memoryTable>().Wait();
            _database.CreateTableAsync<Components.memoryText>().Wait();
            _database.CreateTableAsync<Components.fastCalculating>().Wait();
            _database.CreateTableAsync<Components.fastSummarize>().Wait();
            _database.CreateTableAsync<Components.attentionSwitch>().Wait();
            _database.CreateTableAsync<Components.colorAttention>().Wait();
        }


        //Данные методы предназначены для выгрузки данных из конкретной таблицы конкретного упражнения.
        public Task<List<Components.memoryTable>> getMem()
            {
            return _database.Table<Components.memoryTable>().ToListAsync();
            }
        public Task<List<Components.memoryText>> getText()
        {
            return _database.Table<Components.memoryText>().ToListAsync();
        }

        public Task<List<Components.fastCalculating>> getCalc()
        {
            return _database.Table<Components.fastCalculating>().ToListAsync();
        }

        public Task<List<Components.fastSummarize>> getSum()
        {
            return _database.Table<Components.fastSummarize>().ToListAsync();
        }

        public Task<List<Components.attentionSwitch>> getAtt()
        {
            return _database.Table<Components.attentionSwitch>().ToListAsync();
        }

        public Task<List<Components.colorAttention>> getColor()
        {
            return _database.Table<Components.colorAttention>().ToListAsync();
        }


        //Данные методы предназначены для записи значений в определенную таблицу базы данных.
        public Task<int> SaveNoteAsync(Components.fastSummarize item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> SaveNoteAsync(Components.fastCalculating item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> SaveNoteAsync(Components.memoryTable item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> SaveNoteAsync(Components.memoryText item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> SaveNoteAsync(Components.attentionSwitch item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> SaveNoteAsync(Components.colorAttention item)
        {
            if (item.ID != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

    }


}
