
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

            TaskService.LoadTasks(tasks =>
            {
                TasksStackLayout.Clear();
                foreach(var t in tasks)
                    AddTask(t);
            });
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
            TaskService.AddTask(th);
            AddTask(th);
            TaskTitleInput.Text = "";

            tasks.Add(th);
            await TaskService.SaveTasks(tasks);
        }

        async void HandleDeleteTask(TaskModel task)
        {
            TaskService.DeleteTask(task);
            TasksStackLayout.Remove(task.GetVisualModel());

            await TaskService.SaveTasks(tasks);
        }

        async void HandleCompleteTask(TaskModel task)
        {
            task.Done = !task.Done;
            TaskService.UpdateTask(task);

            await TaskService.SaveTasks(tasks);
        }
    }
}