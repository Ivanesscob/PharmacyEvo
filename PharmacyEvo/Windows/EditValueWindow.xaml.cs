using System.Windows;

namespace PharmacyEvo.Windows
{
    public partial class EditValueWindow : Window
    {
        public string Caption { get; set; } = "Изменить";
        public string Hint { get; set; } = "";
        public string Value { get; set; } = "";

        public EditValueWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                MessageBox.Show("Поле не может быть пустым!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}


