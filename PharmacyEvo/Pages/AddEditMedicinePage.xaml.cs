using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class AddEditMedicinePage : Page
    {
        private Medicine _medicine;
        private bool _isEditMode;
        private ObservableCollection<Category> _categories;
        private ObservableCollection<Manufacturer> _manufacturers;
        private string _selectedImagePath;

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
                
                if (!string.IsNullOrEmpty(_medicine.ImagePath))
                {
                    _selectedImagePath = _medicine.ImagePath;
                    LoadImagePreview(_medicine.ImagePath);
                }
            }
            else
            {
                TitleTextBlock.Text = "Добавить лекарство";
            }
        }

        private void LoadImagePreview(string imagePath)
        {
            try
            {
                var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);
                if (File.Exists(fullPath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    ImagePreview.Source = bitmap;
                    ImagePathTextBlock.Text = imagePath;
                }
            }
            catch
            {
                
            }
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Все файлы (*.*)|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var selectedFile = openFileDialog.FileName;
                    var fileName = Path.GetFileName(selectedFile);
                    var picsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Pics");
                    
                    // Создаем папку Pics, если её нет
                    if (!Directory.Exists(picsFolder))
                    {
                        Directory.CreateDirectory(picsFolder);
                    }

                    var destinationPath = Path.Combine(picsFolder, fileName);
                    
                    // Копируем файл
                    File.Copy(selectedFile, destinationPath, true);
                    
                    // Сохраняем путь для базы данных
                    _selectedImagePath = $"Pics/{fileName}";
                    
                    // Загружаем превью
                    LoadImagePreview(_selectedImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копировании изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                IsPrescription = IsPrescriptionCheckBox.IsChecked ?? false,
                ImagePath = _selectedImagePath ?? (_isEditMode ? _medicine.ImagePath : null)
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
