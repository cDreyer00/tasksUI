using Microsoft.Maui.Controls.Shapes;
using Newtonsoft.Json;
using Microsoft.Maui.Controls;


namespace tasksUI
{
    public partial class MainPage : ContentPage
    {
        List<TaskHolder> tasks = new();

        public MainPage()
        {
            InitializeComponent();
        }

        void HandleAddTask(object sender, EventArgs e)
        {
            TaskHolder th = new(TaskTitleInput.Text);
            tasks.Add(th);
            TasksStackLayout.Add(th.TaskModel);
            TaskTitleInput.Text = "";
        }
    }

    public class TaskHolder
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }

        public View TaskModel { get; set; }

        public TaskHolder(string title)
        {
            Title = title;
            TaskModel = GetModel;
        }

        View GetModel
        {
            get
            {
                // Grid
                var grid = new Grid
                {
                    ColumnSpacing = 10
                };

                // Column Definitions
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });

                // Button 1
                var button1 = new Button();
                button1.Clicked += (s, e) => { }; /*delete me*/

                // Border
                var border = new Border
                {
                    Stroke = Color.FromHex("#3987E1"),
                    StrokeThickness = 4,
                    Background = Color.FromHex("#2B0B98"),
                    StrokeShape = new RoundRectangle { CornerRadius = 10 }
                };
                Grid.SetColumn(border, 1);

                // Label
                var label = new Label
                {
                    TextColor = new Color(255, 255, 255),
                    FontSize = 14,
                    VerticalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(10, 5),
                    Text = Title,                    
                };

                border.Content = label;

                // Button 2
                var button2 = new Button();
                Grid.SetColumn(button2, 2);
                button2.Clicked += (s, e) =>
                {
                    Done = true;                    
                    TaskModel.BackgroundColor = new Color(100, 255, 40);
                };

                // Adding elements to the Grid
                grid.Children.Add(button1);
                grid.Children.Add(border);
                grid.Children.Add(button2);

                return grid;
            }
        }
    }
}