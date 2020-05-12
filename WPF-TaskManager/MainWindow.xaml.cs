using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_TaskManager.Models;
using WPF_TaskManager.Services;

namespace WPF_TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PATHTasks = $"{Environment.CurrentDirectory}\\tasksDataList.json";
        private readonly string PATHTasksCompleted = $"{Environment.CurrentDirectory}\\tasksCompletedDataList.json";
        private BindingList<TaskModel> _tasksDataList = new BindingList<TaskModel>();
        private BindingList<TaskModel> _tasksCompletedDataList = new BindingList<TaskModel>();
        private FileIOService _fileIOService;
        private FileIOService _fileIOService1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIOService = new FileIOService(PATHTasks);
            _fileIOService1 = new FileIOService(PATHTasksCompleted);
            try
            {
                _tasksDataList = _fileIOService.LoadData();
                _tasksCompletedDataList = _fileIOService1.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }


            dgTasksList.ItemsSource = _tasksDataList;
            dgTasksCompletedList.ItemsSource = _tasksCompletedDataList;

            _tasksDataList.ListChanged += TasksDataList_ListChanged;
            _tasksCompletedDataList.ListChanged += TasksCompletedDataList_ListChanged;
        }
        private void TasksCompletedDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService1.SaveData(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }
        private void TasksDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskModel task = new TaskModel();
            _tasksDataList.Add(task);
        }

        private void ChoiceDate_Loaded(object sender, RoutedEventArgs e)
        {
            ChoiceDate.SelectedDate = DateTime.Today;
        }

        private void IsDone(object sender, PropertyChangedEventArgs e)
        {
            _tasksCompletedDataList.Add((TaskModel)sender);
            _tasksDataList.Remove((TaskModel)sender);
        }

        private void Done_Unchecked(object sender, RoutedEventArgs e)
        {
            DataGridCell task = (DataGridCell)sender;
            _tasksDataList.Add((TaskModel)task.DataContext);
            _tasksCompletedDataList.Remove((TaskModel)task.DataContext);
        }

        private void Done_Checked(object sender, RoutedEventArgs e)
        {
            DataGridCell task = (DataGridCell)sender;
            _tasksCompletedDataList.Add((TaskModel)task.DataContext);
            _tasksDataList.Remove((TaskModel)task.DataContext);
        }
    }
}
