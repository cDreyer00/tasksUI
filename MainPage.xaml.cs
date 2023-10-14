using Newtonsoft.Json;

namespace tasksUI
{
    public partial class MainPage : ContentPage
    {
        List<string> tasks = new();

        public MainPage()
        {
            InitializeComponent();
        }

        void HandleAddTask(object sender, EventArgs e)
        {
            tasks.Add(TaskTitleInput.Text);
            UpdateTasksStack();
        }

        void UpdateTasksStack()
        {
            TasksStackLayout.Children.Clear();

            foreach (var task in tasks)
            {
                Label tModel = TaskModel(task);
                TasksStackLayout.Add(tModel);
            }

        }

        Label TaskModel(string taskTitle) 
        {
            Label taskModel = new Label()
            {
                Text = taskTitle,
                Margin = 15,
                BackgroundColor = new Color(255, 255, 255),
                TextColor= new Color(0, 0, 0)
            };

            return taskModel;
        }
    }
}