using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages.Auth
{
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

            if (!IsValidEmail(Model.Email))
            {
                MessageBox.Show("Введите корректный email адрес!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Model.Password != Model.ConfirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ProcedureDB.CheckCustomerEmailExists(Model))
            {
                return;
            }

            ProcedureDB.AddCustomer(Model);
            GlobalClass.MainFrame.Navigate(new LoginPage());
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
                return emailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}


