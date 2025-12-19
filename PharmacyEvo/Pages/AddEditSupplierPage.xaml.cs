using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class AddEditSupplierPage : Page
    {
        private Supplier _supplier;
        private bool _isEditMode;
        private ObservableCollection<Country> _countries;

        public AddEditSupplierPage(Supplier supplier = null)
        {
            InitializeComponent();
            _supplier = supplier;
            _isEditMode = supplier != null;

            _countries = new ObservableCollection<Country>(ProcedureDB.GetCountries());
            CountryComboBox.ItemsSource = _countries;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать поставщика";
                NameTextBox.Text = _supplier.Name;
                PhoneTextBox.Text = _supplier.Phone;
                AddressTextBox.Text = _supplier.Address;
                CountryComboBox.SelectedValue = _supplier.CountryId;
            }
            else
            {
                TitleTextBlock.Text = "Добавить поставщика";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new SuppliersPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || 
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
                CountryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var supplier = new Supplier
            {
                SupplierId = _isEditMode ? _supplier.SupplierId : 0,
                Name = NameTextBox.Text.Trim(),
                Phone = PhoneTextBox.Text.Trim(),
                Address = AddressTextBox.Text.Trim(),
                CountryId = (int)CountryComboBox.SelectedValue
            };

            ProcedureDB.UpsertSupplier(supplier);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
