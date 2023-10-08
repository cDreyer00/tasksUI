namespace tasksUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Label.Text = e.NewTextValue;
        }
    }
}