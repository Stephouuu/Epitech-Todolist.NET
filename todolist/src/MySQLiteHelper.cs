using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Storage;

namespace todolist.src
{
    class MySQLiteHelper
    {
        public static string Database = "todolist.sqlite";

        public void createDatabase()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, Database);
            Debug.WriteLine("path database: " + path);
            if (!File.Exists(path))
            {
                SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), path);
                connection.CreateTable<TodoItem>();
            }
        }

        public void insert(TodoItem item)
        {
            SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), Database);
            connection.RunInTransaction(() =>
            {
                connection.Insert(item);
            });
        }
    }
}
