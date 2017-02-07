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
using System.Collections.ObjectModel;

namespace todolist.src
{
    class MySQLiteHelper
    {
        public static string DatabaseName = "todolist.sqlite";
        private string databasePath;

        public void createDatabase()
        {
            databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseName);
            Debug.WriteLine("path database: " + databasePath);
            if (!File.Exists(databasePath))
            {
                using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), databasePath))
                {
                    connection.CreateTable<TodoItem>();
                }
            }
        }

        public void insert(TodoItem item)
        {
            using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), databasePath))
            {
                connection.RunInTransaction(() =>
                {
                    connection.Insert(item);
                });
            }
        }

        public ObservableCollection<TodoItem> getAllItem()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(new SQLitePlatformWinRT(), databasePath))
                {
                    List<TodoItem> list = connection.Table<TodoItem>().ToList();
                    ObservableCollection<TodoItem> todolist = new ObservableCollection<TodoItem>(list);
                    return todolist;
                }
            }
            catch
            {
                return null;
            }

        }
    }
}
