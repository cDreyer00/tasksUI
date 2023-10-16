
using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace tasksUI
{
    public partial class MainPage : ContentPage
    {
        List<TaskModel> tasks = new();
        public MainPage()
        {
            InitializeComponent();
            TaskService.onRequestFail += OnRequestFail;
        }

        void OnRequestFail(HttpRequestMessage message)
        {
            TaskTitleInput.Text = message.ToString();
        }

        void AddTask(TaskModel th)
        {
            th.onComplete += () => HandleCompleteTask(th);
            th.onDelete += () => HandleDeleteTask(th);
            TasksStackLayout.Insert(0, th.GetVisualModel());
        }

        async void HandleAddTask(object sender, EventArgs e)
        {
            if (TaskTitleInput.Text == null || TaskTitleInput.Text == "")
                return;

            TaskModel th = new(TaskTitleInput.Text);
            TaskService.StorageTasks.Add(th);
            AddTask(th);
            TaskTitleInput.Text = "";
            
            tasks.Add(th);
            await TaskService.SaveTasks(tasks);
        }

        async void HandleDeleteTask(TaskModel task)
        {
            TaskService.StorageTasks.Remove(task);
            TasksStackLayout.Remove(task.GetVisualModel());

            await TaskService.SaveTasks(tasks);
        }

        async void HandleCompleteTask(TaskModel task)
        {
            task.Done = !task.Done;
            UpdateTask(task);

            await TaskService.SaveTasks(tasks);
        }

        void UpdateTask(TaskModel th) => th.GetVisualModel().BackgroundColor = th.Done ? new Color(128, 0, 128) : null;

        async void LoadCache(object sender, EventArgs e)
        {
            TasksStackLayout.Children.Clear();

            tasks = await TaskService.GetCachedTasks();
            foreach (var task in tasks){
                AddTask(task);
                UpdateTask(task);
            }
        }

        async void LoadDb(object sender, EventArgs e)
        {
            TasksStackLayout.Children.Clear();

            tasks = await TaskService.GetDbTasks();
            foreach (var task in tasks){
                AddTask(task);
                UpdateTask(task);
            }
        }
    }
}