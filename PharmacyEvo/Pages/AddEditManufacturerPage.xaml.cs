using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class AddEditManufacturerPage : Page
    {
        private Manufacturer _manufacturer;
        private bool _isEditMode;
        private ObservableCollection<Country> _countries;

        public AddEditManufacturerPage(Manufacturer manufacturer = null)
        {
            InitializeComponent();
            _manufacturer = manufacturer;
            _isEditMode = manufacturer != null;

            _countries = new ObservableCollection<Country>(ProcedureDB.GetCountries());
            CountryComboBox.ItemsSource = _countries;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать производителя";
                NameTextBox.Text = _manufacturer.Name;
                CountryComboBox.SelectedValue = _manufacturer.CountryId;
            }
            else
            {
                TitleTextBlock.Text = "Добавить производителя";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new ManufacturersPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || CountryComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var manufacturer = new Manufacturer
            {
                ManufacturerId = _isEditMode ? _manufacturer.ManufacturerId : 0,
                Name = NameTextBox.Text.Trim(),
                CountryId = (int)CountryComboBox.SelectedValue
            };

            ProcedureDB.UpsertManufacturer(manufacturer);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
