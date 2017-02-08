using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using todolist.src;

namespace todolist
{
    class TodoItem
    {
        private static string RED = "#FFE50000";
        private static string GREEN = "#FF009900";
        private static string BLUE = "#FF3F51B5";

        public enum Status
        {
            None = 0,
            Overdue = 1,
            Done = 2,
            Todo = 4
        }

        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int id { get; set; }

        [SQLite.Net.Attributes.NotNull]
        public string title { set; get; }

        [SQLite.Net.Attributes.NotNull]
        public string content { set; get; }

        [SQLite.Net.Attributes.NotNull]
        public DateTime dateTime { set; get; }

        [SQLite.Net.Attributes.NotNull]
        public Status status
        {
            get
            {
                return _status;
            }

            set
            {
                this._status = value;
                retrieveColor();
            }
        }

        [SQLite.Net.Attributes.NotNull]
        public string color { set; get; }

        [SQLite.Net.Attributes.NotNull]
        public string files { set; get; }

        public string headerVisibility { set; get; }

        public string userFriendlyDateTime { set; get; }

        private Status _status;
        
        public TodoItem()
        {
            id = 0;
            title = "A Title";
            content = "A Content";
            dateTime = DateTime.Now;
            status = Status.Todo;
            color = BLUE;
            headerVisibility = "Collapsed";
        }

        private void retrieveColor()
        {
            switch (_status)
            {
                case Status.Done:
                    color = GREEN;
                    break;
                case Status.Todo:
                    color = BLUE;
                    break;
                case Status.Overdue:
                    color = RED;
                    break;
            }
        }


    }
}
