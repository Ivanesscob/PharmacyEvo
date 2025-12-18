using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditCustomerPage.xaml
    /// </summary>
    public partial class AddEditCustomerPage : Page
    {
        private Customer _customer;
        private bool _isEditMode;
        private ObservableCollection<Role> _roles;

        public AddEditCustomerPage(Customer customer = null)
        {
            InitializeComponent();
            _customer = customer;
            _isEditMode = customer != null;

            _roles = new ObservableCollection<Role>(ProcedureDB.GetRoles());
            RoleComboBox.ItemsSource = _roles;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать клиента";
                FullNameTextBox.Text = _customer.FullName;
                EmailTextBox.Text = _customer.Email;
                PhoneTextBox.Text = _customer.Phone;
                PasswordTextBox.Text = _customer.Password;
                RoleComboBox.SelectedValue = _customer.RoleId;
                RegistrationDatePicker.SelectedDate = _customer.RegistrationDate;
                IsActiveCheckBox.IsChecked = _customer.IsActive;
            }
            else
            {
                TitleTextBlock.Text = "Добавить клиента";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new ClientsPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                RoleComboBox.SelectedValue == null ||
                RegistrationDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var customer = new Customer
            {
                CustomerId = _isEditMode ? _customer.CustomerId : 0,
                FullName = FullNameTextBox.Text,
                Email = EmailTextBox.Text,
                Phone = PhoneTextBox.Text,
                Password = PasswordTextBox.Text,
                RoleId = (int)RoleComboBox.SelectedValue,
                RegistrationDate = RegistrationDatePicker.SelectedDate.Value,
                IsActive = IsActiveCheckBox.IsChecked ?? false
            };

            ProcedureDB.UpsertCustomer(customer);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
