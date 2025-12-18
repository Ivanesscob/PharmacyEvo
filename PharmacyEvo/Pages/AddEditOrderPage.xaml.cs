using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class AddEditOrderPage : Page
    {
        private Order _order;
        private bool _isEditMode;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Employee> _employees;

        public AddEditOrderPage(Order order = null)
        {
            InitializeComponent();
            _order = order;
            _isEditMode = order != null;

            _customers = new ObservableCollection<Customer>(ProcedureDB.GetCustomers());
            CustomerComboBox.ItemsSource = _customers;

            _employees = new ObservableCollection<Employee>(ProcedureDB.GetEmployees());
            EmployeeComboBox.ItemsSource = _employees;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать заказ";
                OrderDatePicker.SelectedDate = _order.OrderDate;
                CustomerComboBox.SelectedValue = _order.CustomerId;
                EmployeeComboBox.SelectedValue = _order.EmployeeId;
            }
            else
            {
                TitleTextBlock.Text = "Добавить заказ";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new OrdersPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatePicker.SelectedDate == null ||
                CustomerComboBox.SelectedValue == null ||
                EmployeeComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var order = new Order
            {
                OrderId = _isEditMode ? _order.OrderId : 0,
                OrderDate = OrderDatePicker.SelectedDate.Value,
                CustomerId = (int)CustomerComboBox.SelectedValue,
                EmployeeId = (int)EmployeeComboBox.SelectedValue
            };

            ProcedureDB.UpsertOrder(order);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
