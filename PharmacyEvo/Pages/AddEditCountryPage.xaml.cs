using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditCountryPage.xaml
    /// </summary>
    public partial class AddEditCountryPage : Page
    {
        private Country _country;
        private bool _isEditMode;

        public AddEditCountryPage(Country country = null)
        {
            InitializeComponent();
            _country = country;
            _isEditMode = country != null;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать страну";
                CountryNameTextBox.Text = _country.CountryName;
            }
            else
            {
                TitleTextBlock.Text = "Добавить страну";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new CountriesPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CountryNameTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var country = new Country
            {
                CountryId = _isEditMode ? _country.CountryId : 0,
                CountryName = CountryNameTextBox.Text
            };

            ProcedureDB.UpsertCountry(country);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
