using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditEmployeePage.xaml
    /// </summary>
    public partial class AddEditEmployeePage : Page
    {
        private Employee _employee;
        private bool _isEditMode;
        private ObservableCollection<Role> _roles;

        public AddEditEmployeePage(Employee employee = null)
        {
            InitializeComponent();
            _employee = employee;
            _isEditMode = employee != null;

            _roles = new ObservableCollection<Role>(ProcedureDB.GetRoles());
            RoleComboBox.ItemsSource = _roles;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать сотрудника";
                FullNameTextBox.Text = _employee.FullName;
                EmailTextBox.Text = _employee.Email;
                PhoneTextBox.Text = _employee.Phone;
                PasswordTextBox.Text = _employee.Password;
                RoleComboBox.SelectedValue = _employee.RoleId;
                HireDatePicker.SelectedDate = _employee.HireDate;
                IsActiveCheckBox.IsChecked = _employee.IsActive;
            }
            else
            {
                TitleTextBlock.Text = "Добавить сотрудника";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new EmployeesPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
                RoleComboBox.SelectedValue == null ||
                HireDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var employee = new Employee
            {
                EmployeeId = _isEditMode ? _employee.EmployeeId : 0,
                FullName = FullNameTextBox.Text,
                Email = EmailTextBox.Text,
                Phone = PhoneTextBox.Text,
                Password = PasswordTextBox.Text,
                RoleId = (int)RoleComboBox.SelectedValue,
                HireDate = HireDatePicker.SelectedDate.Value,
                IsActive = IsActiveCheckBox.IsChecked ?? false
            };

            ProcedureDB.UpsertEmployee(employee);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
