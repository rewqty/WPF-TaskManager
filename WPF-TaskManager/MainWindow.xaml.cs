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
using WPF_TaskManager.ViewModels;

namespace WPF_TaskManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DictionaryTaskModel dict = new DictionaryTaskModel();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dict.Load();
            dgTasksList.ItemsSource = dict.TasksDataDictionary[DateTime.Today][0];
            dgTasksCompletedList.ItemsSource = dict.TasksDataDictionary[DateTime.Today][1];
            dict.TasksDataDictionary[DateTime.Today][0].ListChanged += TasksDataList_ListChanged;
            dict.TasksDataDictionary[DateTime.Today][1].ListChanged += TasksDataList_ListChanged;

        }
        private void TasksDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                dict.Save();
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            dict.TaskAdd();
        }

        private void ChoiceDate_Loaded(object sender, RoutedEventArgs e)
        {
            ChoiceDate.SelectedDate = DateTime.Today;
        }

        private void Done_Unchecked(object sender, RoutedEventArgs e)
        {
            dict.MoveTaskToIncompleted(sender);
        }

        private void Done_Checked(object sender, RoutedEventArgs e)
        {
            dict.MoveTaskToCompleted(sender);
        }

        private void ChoiceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dict.DateUpdate(ChoiceDate.SelectedDate.Value);
            dict.TasksDataDictionary[DictionaryTaskModel.SelectedDate][0].ListChanged += TasksDataList_ListChanged;
            dict.TasksDataDictionary[DictionaryTaskModel.SelectedDate][1].ListChanged += TasksDataList_ListChanged;
            dgTasksList.ItemsSource = dict.TasksDataDictionary[DictionaryTaskModel.SelectedDate][0];
            dgTasksCompletedList.ItemsSource = dict.TasksDataDictionary[DictionaryTaskModel.SelectedDate][1];
        }
    }
}
