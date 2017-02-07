using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace todolist
{
    class TodoItem
    {
        public enum Status
        {
            Overdue = 1,
            Todo = 2,
            Done = 4
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
        public Status status { set; get; }

        public TodoItem()
        {
            id = 0;
            title = "A Title";
            content = "A Content";
            dateTime = DateTime.Now;
        }

        public TodoItem(string title, string content, string dateTime, Status status)
        {
            id = 0;
            this.title = title;
            this.content = content;
            this.dateTime = DateTime.Parse(dateTime);
            this.status = status;
        }


    }
}
