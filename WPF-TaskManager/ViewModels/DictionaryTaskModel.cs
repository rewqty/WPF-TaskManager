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
using WPF_TaskManager;

namespace WPF_TaskManager.ViewModels
{
    class DictionaryTaskModel
    {
        private Dictionary<DateTime, BindingList<TaskModel>[]> _tasksDataDictionary = new Dictionary<DateTime, BindingList<TaskModel>[]>();
        public Dictionary<DateTime, BindingList<TaskModel>[]> TasksDataDictionary
        {
            get { return _tasksDataDictionary; }
            set
            {
                if (_tasksDataDictionary == value)
                    return;
                _tasksDataDictionary = value;
            }
        }
        public static DateTime SelectedDate = DateTime.Today;

        private readonly static string PATHTasks = $"{Environment.CurrentDirectory}\\tasksDataDictionary.json";
        private readonly static FileIOService FileIOService = new FileIOService(PATHTasks);
        public void CheckFileAndDirectory()
        {
            try
            {
                if (FileIOService.LoadData() != null) TasksDataDictionary = FileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (!TasksDataDictionary.ContainsKey(DateTime.Today))
            {
                BindingList<TaskModel>[] tasksArray = { new BindingList<TaskModel>(), new BindingList<TaskModel>() };
                TasksDataDictionary.Add(DateTime.Today, tasksArray);
            }

        }
        public void Save()
        {
            try
            {
                FileIOService.SaveData(TasksDataDictionary);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void MoveTaskToCompleted(object sender)
        {
            DataGridCell task = (DataGridCell)sender;
            TasksDataDictionary[SelectedDate][1].Add((TaskModel)task.DataContext);
            TasksDataDictionary[SelectedDate][0].Remove((TaskModel)task.DataContext);
        }

        public void MoveTaskToIncompleted(object sender)
        {
            DataGridCell task = (DataGridCell)sender;
            TasksDataDictionary[SelectedDate][0].Add((TaskModel)task.DataContext);
            TasksDataDictionary[SelectedDate][1].Remove((TaskModel)task.DataContext);
        }

        public void DateUpdate(DateTime date)
        {
            SelectedDate = date;

            BindingList<TaskModel>[] tasksArray = { new BindingList<TaskModel>(), new BindingList<TaskModel>() };

            if (!TasksDataDictionary.ContainsKey(SelectedDate))
            {
                TasksDataDictionary.Add(SelectedDate, tasksArray);
            }
        }

        public void TaskAdd()
        {
            TaskModel task = new TaskModel();
            TasksDataDictionary[SelectedDate][0].Add(task);
        }
    }
}
