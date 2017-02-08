using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections;
using todolist.src;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Windows.Storage.AccessCache;
using Windows.Storage;

namespace todolist
{
    public sealed partial class ConsultationPage : Page
    {
        private MySQLiteHelper database { get; set; } = new MySQLiteHelper();
        private TodoItem item;

        public ObservableCollection<AdditionalFile> files { get; set; }

        public ConsultationPage()
        {
            database.initializeDatabase();

            files = new ObservableCollection<AdditionalFile>();
            //files.Add(new AdditionalFile() { name = "Add new file" });
            

            this.DataContext = this;
            this.InitializeComponent();
        }

        private void refresh()
        {
            if (item.status == TodoItem.Status.Done)
            {
                ValidButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ValidButton.Visibility = Visibility.Visible;
            }

            if (item != null)
            {
                if (item.title.Length <= 12)
                {
                    titleBar.Text = "Consulting '" + item.title + "'";
                }
                else
                {
                    titleBar.Text = "Consulting '" + item.title.Substring(0, 12) + "...'";
                }
                titleBox.Text = item.title;
                contentBox.Text = item.content;
                datePicker.Date = item.dateTime;
                timePicker.Time = new TimeSpan(item.dateTime.Ticks);

                refreshFiles();
            }
        }

        private void refreshFiles()
        {
            files.Clear();

            List<String> filesPath = StaticTools.DeserializeFile(item.files);
            if (filesPath != null)
            {
                foreach (string path in filesPath)
                {
                    if (path.Length > 0)
                    {
                        string name = Path.GetFileName(path);
                        files.Add(new AdditionalFile()
                        {
                            name = name,
                            path = path
                        });
                    }
                }
            }
            files.Add(new AdditionalFile() { name = "Add new item" });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string id = e.Parameter as string;
            item = database.getItem(int.Parse(id));
            item.dateTime = TimeZoneInfo.ConvertTime(item.dateTime, TimeZoneInfo.Local);
            refresh();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditButton.Visibility = Visibility.Collapsed;
            ValidButton.Visibility = Visibility.Visible;

            titleBox.IsEnabled = true;
            contentBox.IsEnabled = true;
            datePicker.IsEnabled = true;
            timePicker.IsEnabled = true;
        }

        private async void Valid_Click(object sender, RoutedEventArgs e)
        {
            if (EditButton.Visibility == Visibility.Visible)
            {
                item.status = TodoItem.Status.Done;
                database.updateItem(item);
                this.Frame.GoBack();
            }
            else
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
                    EditButton.Visibility = Visibility.Visible;
                    
                    titleBox.IsEnabled = false;
                    contentBox.IsEnabled = false;
                    datePicker.IsEnabled = false;
                    timePicker.IsEnabled = false;

                    item.title = title;
                    item.content = content;
                    item.dateTime = dt;
                    item.status = TodoItem.Status.Todo;
                    item.files = StaticTools.SerializeFiles(StaticTools.GetFilePathFromStructure(files.ToList()));
                    database.updateItem(item);
                    refresh();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            database.deleteItem(item.id);
            Frame.GoBack();
        }

        private async void filesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            AdditionalFile file = (AdditionalFile)e.ClickedItem;
            int idx = files.IndexOf(file);
            if (idx == files.Count - 1)
            {
                if (EditButton.Visibility == Visibility.Collapsed)
                {
                    var picker = new Windows.Storage.Pickers.FileOpenPicker();
                    picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                    picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                    picker.FileTypeFilter.Add("*");

                    StorageFile selectedFile = await picker.PickSingleFileAsync();
                    if (selectedFile != null)
                    {
                        StorageApplicationPermissions.FutureAccessList.Add(selectedFile);
                        if (item.files.Length > 0)
                            item.files += ";" + selectedFile.Path;
                        else
                            item.files += selectedFile.Path;
                        refreshFiles();
                    }
                }
            }
            else
            {
                StorageFile selectedFile = await StorageFile.GetFileFromPathAsync(file.path);
                if (selectedFile != null)
                {
                    await Windows.System.Launcher.LaunchFileAsync(selectedFile);
                }
            }
        }
    }

    public class AdditionalFile
    {
        public string name { set; get; }
        public string path { set; get; }
    }
}
