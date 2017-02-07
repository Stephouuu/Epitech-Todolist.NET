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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

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

            this.ppup.Height = 660 / 1.25f;
            this.ppup.Width = Window.Current.Bounds.Width / 2;

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
                TodoItem item = new TodoItem(title, content);
                database.insert(item);
                this.ppup.IsOpen = false;
                this.titleBox.Text = "";
                this.contentBox.Text = "";
                refresh();
            }
        }

        private void refresh()
        {
            this.todolistView.ItemsSource = database.getAllItem().OrderByDescending(i => i.Id).ToList();
            if (this.todolistView.Items.Count != 0)
            {
                this.noItem.Visibility = Visibility.Collapsed;
                this.noItemText.Visibility = Visibility.Collapsed;
            }
        }
    }

}
