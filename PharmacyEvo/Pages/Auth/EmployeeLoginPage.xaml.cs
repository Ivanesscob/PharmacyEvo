using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages.Auth
{
    /// <summary>
    /// Логика взаимодействия для EmployeeLoginPage.xaml
    /// </summary>
    public partial class EmployeeLoginPage : Page
    {
        public AuthModel AuthModel { get; set; } = new AuthModel();

        public EmployeeLoginPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}


