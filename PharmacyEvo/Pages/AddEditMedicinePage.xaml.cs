using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditMedicinePage.xaml
    /// </summary>
    public partial class AddEditMedicinePage : Page
    {
        private Medicine _medicine;
        private bool _isEditMode;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Manufacturer> _manufacturers;

        public AddEditMedicinePage(Medicine medicine = null)
        {
            InitializeComponent();
            _medicine = medicine;
            _isEditMode = medicine != null;

            _categories = new ObservableCollection<Category>(ProcedureDB.GetCategories());
            CategoryComboBox.ItemsSource = _categories;

            _manufacturers = new ObservableCollection<Manufacturer>(ProcedureDB.GetManufacturers());
            ManufacturerComboBox.ItemsSource = _manufacturers;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать лекарство";
                NameTextBox.Text = _medicine.Name;
                CategoryComboBox.SelectedValue = _medicine.CategoryId;
                ManufacturerComboBox.SelectedValue = _medicine.ManufacturerId;
                PriceTextBox.Text = _medicine.Price.ToString(CultureInfo.InvariantCulture);
                IsPrescriptionCheckBox.IsChecked = _medicine.IsPrescription;
            }
            else
            {
                TitleTextBlock.Text = "Добавить лекарство";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new MedicinesPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                CategoryComboBox.SelectedValue == null ||
                ManufacturerComboBox.SelectedValue == null ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                !decimal.TryParse(PriceTextBox.Text, out _))
            {
                MessageBox.Show("Заполните все поля корректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var medicine = new Medicine
            {
                MedicineId = _isEditMode ? _medicine.MedicineId : 0,
                Name = NameTextBox.Text,
                CategoryId = (int)CategoryComboBox.SelectedValue,
                ManufacturerId = (int)ManufacturerComboBox.SelectedValue,
                Price = decimal.Parse(PriceTextBox.Text, CultureInfo.InvariantCulture),
                IsPrescription = IsPrescriptionCheckBox.IsChecked ?? false
            };

            ProcedureDB.UpsertMedicine(medicine);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
