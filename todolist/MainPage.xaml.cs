using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using System.Windows;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using todolist.src;
using System.Diagnostics;

namespace todolist
{
    
    public sealed partial class MainPage : Page
    {
        private MySQLiteHelper database { get; set; } = new MySQLiteHelper();

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size(360, 660);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            this.database.createDatabase();
            refresh();
        }

        private void onAddItemClick(object sender, RoutedEventArgs e)
        {
            this.ppup.IsOpen = true;
        }

        private async void validButton_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            string title = this.titleBox.Text;
            string content = this.contentBox.Text;
            string date = this.datePicker.Date.ToString("dd/MM/yyyy");
            string time = timePicker.Time.ToString();

            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            {
                error = "The title can not be empty !";
            }
            else if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            {
                error = "The content can not be empty !";
            }

            if (!string.IsNullOrEmpty(error))
            {
                MessageDialog dialog = new MessageDialog(error, "Error");
                dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                IUICommand res = await dialog.ShowAsync();
            }
            else
            {
                TodoItem item = new TodoItem(title, content, date + " " + time, TodoItem.Status.Todo);
                database.insert(item);

                AlarmManager.addAlarm(DateTime.Parse(date + " " + time), title, content);

                this.ppup.IsOpen = false;
                this.titleBox.Text = "";
                this.contentBox.Text = "";
                refresh();
            }
        }

        private void refresh()
        {
            List<TodoItem> list = database.getAllItem().OrderBy(i => i.dateTime).ToList();
            foreach (TodoItem item in list) {
                item.dateTime = TimeZoneInfo.ConvertTime(item.dateTime, TimeZoneInfo.Local);
                if (item.status == TodoItem.Status.Todo && item.dateTime < DateTime.Now)
                {
                    item.status = TodoItem.Status.Overdue;
                }
                if (item.status == TodoItem.Status.Todo)
                {
                    this.todolistView.Items.Add(item);
                }
            }

            if (this.todolistView.Items.Count != 0)
            {
                this.noItem.Visibility = Visibility.Collapsed;
                this.noItemText.Visibility = Visibility.Collapsed;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
