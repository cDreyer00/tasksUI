
using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace tasksUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            TasksData.Init().Wait();
            foreach (var t in TasksData.StorageTasks)
                AddTask(t);
        }

        void AddTask(TaskHolder th)
        {
            TasksStackLayout.Insert(0, th.GetModel());
            TasksStackLayout.Children.Reverse();
        }

        void AddTask(string taskTitle)
        {
            TaskHolder th = new(taskTitle);
            th.onDelete += () => HandleDeleteTask(th);
            th.onComplete += () => HandleCompleteTask(th);

            TasksData.StorageTasks.Add(th);
            TasksStackLayout.Insert(0, th.GetModel());
            TasksStackLayout.Children.Reverse();
            TaskTitleInput.Text = "";

            TasksData.SaveTasks();
        }

        void HandleAddTask(object sender, EventArgs e)
        {
            if (TaskTitleInput.Text == null || TaskTitleInput.Text == "")
                return;

            AddTask(TaskTitleInput.Text);
        }

        void HandleDeleteTask(TaskHolder task)
        {
            TasksData.StorageTasks.Remove(task);
            TasksStackLayout.Remove(task.GetModel());

            TasksData.SaveTasks();
        }

        void HandleCompleteTask(TaskHolder task)
        {
            task.Done = !task.Done;

            TasksData.SaveTasks();
        }
    }
}