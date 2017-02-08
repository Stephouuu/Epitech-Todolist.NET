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

            this.database.initializeDatabase();
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
            DateTime dt = TimeZoneInfo.ConvertTime(DateTime.Parse(date + " " + time), TimeZoneInfo.Local);

            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            {
                error = "The title can not be empty !";
            }
            else if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            {
                error = "The content can not be empty !";
            }
            else if (dt < DateTime.Now)
            {
                error = "The due date must be in the future";
            }

            if (!string.IsNullOrEmpty(error))
            {
                MessageDialog dialog = new MessageDialog(error, "Error");
                dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                IUICommand res = await dialog.ShowAsync();
            }
            else
            {
                database.insert(new TodoItem()
                {
                    title = title,
                    content = content,
                    dateTime = DateTime.Parse(date + " " + time),
                    status = TodoItem.Status.Todo,
                    files = ""
                });

                AlarmManager.addAlarm(DateTime.Parse(date + " " + time), title, content);

                this.ppup.IsOpen = false;
                this.titleBox.Text = "";
                this.contentBox.Text = "";
                this.datePicker.Date = DateTime.Now;
                this.timePicker.Time = new TimeSpan(DateTime.Now.Ticks);
                refresh();
            }
        }

        private void refresh()
        {
            this.todolistView.Items.Clear();
            // set sorting order here
            List<TodoItem> list = database.getAllItem().OrderBy(i => i.dateTime).OrderBy(i => i.status).ToList();
            TodoItem.Status last = TodoItem.Status.None;
            foreach (TodoItem item in list) {
                item.dateTime = TimeZoneInfo.ConvertTime(item.dateTime, TimeZoneInfo.Local);
                if (item.status == TodoItem.Status.Todo && item.dateTime < DateTime.Now)
                {
                    item.status = TodoItem.Status.Overdue;
                    database.updateItem(item);
                }
                Debug.WriteLine("title: " + item.title + " status: " + item.status);
                // check filter here
                item.userFriendlyDateTime = DateTimeManager.getUserFriendlyDateTime(item.dateTime);
                if (last != item.status)
                {
                    item.headerVisibility = "Visible";
                }
                this.todolistView.Items.Add(item);
                last = item.status;
            }

            if (this.todolistView.Items.Count != 0)
            {
                this.noItem.Visibility = Visibility.Collapsed;
                this.noItemText.Visibility = Visibility.Collapsed;
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            // todo
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            // todo
        }

        private void todolistView_ItemClick(object sender, ItemClickEventArgs e)
        {
            TodoItem item = (TodoItem)e.ClickedItem;
            this.Frame.Navigate(typeof(ConsultationPage), "" + item.id);
        }

    }

}
