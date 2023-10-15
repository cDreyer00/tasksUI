using Microsoft.Maui.Controls.Shapes;

namespace tasksUI
{
    public class TaskHolder
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }

        public event Action onDelete;
        public event Action onComplete;

        View _taskModel = null;

        public TaskHolder(string title)
        {
            Title = title;
        }

        public View GetModel()
        {
            if (_taskModel != null)
                return _taskModel;

            var grid = new Grid
            {
                ColumnSpacing = 10
            };

            // Column Definitions
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });

            // Button 1
            var deleteButton = new Button();
            deleteButton.Clicked += (s, e) => { onDelete?.Invoke(); };

            var completeButton = new Button();
            Grid.SetColumn(completeButton, 2);
            completeButton.Clicked += (s, e) => { onComplete?.Invoke(); };

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

            // Adding elements to the Grid
            grid.Children.Add(deleteButton);
            grid.Children.Add(border);
            grid.Children.Add(completeButton);

            _taskModel = grid;
            return _taskModel;
        }
    }
}