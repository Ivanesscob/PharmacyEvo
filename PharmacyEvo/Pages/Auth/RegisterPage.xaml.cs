using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages.Auth
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterModel Model { get; set; } = new RegisterModel();
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void TextBlockLogin_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Global.GlobalClass.MainFrame.Navigate(new LoginPage());
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Model.Password) ||
            string.IsNullOrEmpty(Model.ConfirmPassword) ||
            string.IsNullOrEmpty(Model.Email) ||
            string.IsNullOrEmpty(Model.Name))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                if (ProcedureDB.CheckCustomerEmailExists(Model))
                {
                    return;
                }
                else
                {
                    ProcedureDB.AddCustomer(Model);
                    GlobalClass.MainFrame.Navigate(new LoginPage());
                }
            }
            

        }
    }
}


