using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
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
            if (string.IsNullOrEmpty(AuthModel.Password) || string.IsNullOrEmpty(AuthModel.LoginOrEmail))
            {
                MessageBox.Show("Заполните все поля",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (ProcedureDB.LoginEmployee(AuthModel))
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new LoginPage());
        }
    }
}


