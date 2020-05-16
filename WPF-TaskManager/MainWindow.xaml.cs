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
        private Dictionary<DateTime, BindingList<TaskModel>[]> _tasksDataDictionary = new Dictionary<DateTime, BindingList<TaskModel>[]>();
        private DateTime _selectedDate = DateTime.Today;
        private readonly string PATHTasks = $"{Environment.CurrentDirectory}\\tasksDataDictionary.json";
        private FileIOService _fileIOService;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_tasksDataDictionary.ContainsKey(DateTime.Today))
            {
                _tasksDataDictionary.Add(DateTime.Today, new BindingList<TaskModel>[2]);
            }

            _fileIOService = new FileIOService(PATHTasks);
            try
            {
                _tasksDataDictionary = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }

            dgTasksList.ItemsSource = _tasksDataDictionary[DateTime.Today][0];
            dgTasksCompletedList.ItemsSource = _tasksDataDictionary[DateTime.Today][1];

            _tasksDataDictionary[DateTime.Today][0].ListChanged += TasksDataList_ListChanged;
            _tasksDataDictionary[DateTime.Today][1].ListChanged += TasksCompletedDataList_ListChanged;
        }
        private void TasksCompletedDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(_tasksDataDictionary);
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
                    _fileIOService.SaveData(_tasksDataDictionary);
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
            _tasksDataDictionary[_selectedDate][0].Add(task);
        }

        private void ChoiceDate_Loaded(object sender, RoutedEventArgs e)
        {
            ChoiceDate.SelectedDate = DateTime.Today;
        }

        private void Done_Unchecked(object sender, RoutedEventArgs e)
        {
            DataGridCell task = (DataGridCell)sender;
            _tasksDataDictionary[_selectedDate][0].Add((TaskModel)task.DataContext);
            _tasksDataDictionary[_selectedDate][1].Remove((TaskModel)task.DataContext);
        }

        private void Done_Checked(object sender, RoutedEventArgs e)
        {
            DataGridCell task = (DataGridCell)sender;
            _tasksDataDictionary[_selectedDate][1].Add((TaskModel)task.DataContext);
            _tasksDataDictionary[_selectedDate][0].Remove((TaskModel)task.DataContext);
        }

        private void ChoiceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedDate = ChoiceDate.SelectedDate.Value;
            BindingList<TaskModel>[] tasksArray = { new BindingList<TaskModel>(), new BindingList<TaskModel>()};

            if (!_tasksDataDictionary.ContainsKey(_selectedDate))
            {
                _tasksDataDictionary.Add(_selectedDate, tasksArray);
            }

            dgTasksList.ItemsSource = _tasksDataDictionary[_selectedDate][0];
            dgTasksCompletedList.ItemsSource = _tasksDataDictionary[_selectedDate][1];

            _tasksDataDictionary[_selectedDate][0].ListChanged += TasksDataList_ListChanged;
            _tasksDataDictionary[_selectedDate][1].ListChanged += TasksCompletedDataList_ListChanged;
        }
    }
}
