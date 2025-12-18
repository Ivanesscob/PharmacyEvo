using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages.Auth
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public AuthModel AuthModel { get; set; } = new AuthModel();
        public LoginPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void TextBlockRegister_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Global.GlobalClass.MainFrame.Navigate(new RegisterPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AuthModel.Password) || string.IsNullOrEmpty(AuthModel.LoginOrEmail))
            {
                MessageBox.Show("Заполните все поля",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (ProcedureDB.GetCustomer(AuthModel))
                {
                    GlobalClass.MainFrame.Navigate(new HomePage());
                }
                else
                {
                    MessageBox.Show("Пароль или логин не правильные",
                                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SystemButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new EmployeeLoginPage());
        }
    }
}


