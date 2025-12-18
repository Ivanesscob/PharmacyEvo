using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для SuppliesPage.xaml
    /// </summary>
    public partial class SuppliesPage : Page
    {
        public SuppliesPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new HomePage());
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
