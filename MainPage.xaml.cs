
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
            {
                AddTask(t);
                UpdateTask(t);
            }
        }

        void AddTask(TaskHolder th)
        {
            th.onComplete += () => HandleCompleteTask(th);
            th.onDelete += () => HandleDeleteTask(th);
            TasksStackLayout.Insert(0, th.GetModel());
        }

        async void HandleAddTask(object sender, EventArgs e)
        {
            if (TaskTitleInput.Text == null || TaskTitleInput.Text == "")
                return;

            TaskHolder th = new(TaskTitleInput.Text);
            TasksData.StorageTasks.Add(th);
            AddTask(th);
            TaskTitleInput.Text = "";

            await TasksData.SaveTasks();
        }

        async void HandleDeleteTask(TaskHolder task)
        {
            TasksData.StorageTasks.Remove(task);
            TasksStackLayout.Remove(task.GetModel());

            await TasksData.SaveTasks();
        }

        async void HandleCompleteTask(TaskHolder task)
        {
            task.Done = !task.Done;
            UpdateTask(task);

            await TasksData.SaveTasks();
        }

        void UpdateTask(TaskHolder th) => th.GetModel().BackgroundColor = th.Done ? new Color(128, 0, 128) : null;
    }
}