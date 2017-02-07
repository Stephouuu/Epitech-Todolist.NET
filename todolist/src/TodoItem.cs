using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todolist
{
    class TodoItem
    {
        [SQLite.Net.Attributes.PrimaryKey, SQLite.Net.Attributes.AutoIncrement]
        public int Id { get; set; }

        [SQLite.Net.Attributes.NotNull]
        public string Title { set; get; }

        [SQLite.Net.Attributes.NotNull]
        public string Content { set; get; }

        /*[SQLite.Net.Attributes.NotNull]
        public DateTime DateTime { set; get; }*/

        public TodoItem()
        {
            Id = 0;
            Title = "A Title";
            Content = "A Content";
            //DateTime = DateTime.Now;
        }

        public TodoItem(string title, string content)
        {
            Id = 0;
            Title = title;
            Content = content;
        }


    }
}
