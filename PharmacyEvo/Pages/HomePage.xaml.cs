using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class HomePage : Page
    {
        public bool IsCustomer { get; set; }
        public bool IsManeger { get; set; }
        public bool IsAdmin { get; set; }
        private ObservableCollection<MedicineViewModel> _medicines;
        private ObservableCollection<MedicineViewModel> _allMedicines;
        private ObservableCollection<Medicine> _allMedicinesData;
        private ObservableCollection<Category> _categories;

        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
            IsCustomer = GlobalClass.CurrentUser.IsCustomer;
            IsAdmin = GlobalClass.CurrentUser.IsAdmin;
            IsManeger = GlobalClass.CurrentUser.IsManeger;
            _medicines = new ObservableCollection<MedicineViewModel>();
            _allMedicines = new ObservableCollection<MedicineViewModel>();
            LoadCategories();
            LoadMedicines();
        }

        private void LoadCategories()
        {
            var allCategories = new ObservableCollection<Category>();
            allCategories.Add(new Category { CategoryId = 0, CategoryName = "Все категории" });
            foreach (var category in ProcedureDB.GetCategories())
            {
                allCategories.Add(category);
            }
            _categories = allCategories;
            CategoryFilterComboBox.ItemsSource = _categories;
            CategoryFilterComboBox.SelectedIndex = 0;
        }

        private void LoadMedicines()
        {
            _medicines = new ObservableCollection<MedicineViewModel>();
            _allMedicines = new ObservableCollection<MedicineViewModel>();
            _allMedicinesData = new ObservableCollection<Medicine>(ProcedureDB.GetMedicines());
            
            foreach (var medicine in _allMedicinesData)
            {
                var viewModel = new MedicineViewModel
                {
                    MedicineId = medicine.MedicineId,
                    Name = medicine.Name,
                    Price = medicine.Price,
                    PriceText = $"{medicine.Price:F2} ₽",
                    ImagePath = medicine.ImagePath,
                    CategoryId = medicine.CategoryId
                };
                
                if (!string.IsNullOrEmpty(medicine.ImagePath))
                {
                    try
                    {
                        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, medicine.ImagePath);
                        if (File.Exists(fullPath))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.EndInit();
                            viewModel.ImageSource = bitmap;
                        }
                    }
                    catch
                    {
                    }
                }
                
                _allMedicines.Add(viewModel);
            }
            
            ApplyFilters();
            MedicinesItemsControl.ItemsSource = _medicines;
        }

        private void ApplyFilters()
        {
            if (_allMedicines == null || _medicines == null)
                return;

            var filtered = _allMedicines.ToList();

            var searchText = SearchTextBox?.Text?.ToLower() ?? "";
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filtered = filtered.Where(m => m.Name?.ToLower().Contains(searchText) == true).ToList();
            }

            if (CategoryFilterComboBox?.SelectedItem is Category selectedCategory)
            {
                if (selectedCategory.CategoryId != 0)
                {
                    filtered = filtered.Where(m => m.CategoryId == selectedCategory.CategoryId).ToList();
                }
            }

            var sortIndex = SortComboBox?.SelectedIndex ?? 0;
            switch (sortIndex)
            {
                case 0:
                    filtered = filtered.OrderBy(m => m.Name).ToList();
                    break;
                case 1:
                    filtered = filtered.OrderByDescending(m => m.Name).ToList();
                    break;
                case 2:
                    filtered = filtered.OrderBy(m => m.Price).ToList();
                    break;
                case 3:
                    filtered = filtered.OrderByDescending(m => m.Price).ToList();
                    break;
            }

            _medicines.Clear();
            foreach (var item in filtered)
            {
                _medicines.Add(item);
            }
            
            MedicinesItemsControl.ItemsSource = _medicines;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            SortComboBox.SelectedIndex = 0;
            CategoryFilterComboBox.SelectedIndex = 0;
            ApplyFilters();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame?.Navigate(new CartPage());
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is MedicineViewModel viewModel)
            {
                var medicine = _allMedicinesData.FirstOrDefault(m => m.MedicineId == viewModel.MedicineId);
                if (medicine != null)
                {
                    var existingItem = GlobalClass.Cart.FirstOrDefault(item => item.Medicine.MedicineId == medicine.MedicineId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity++;
                    }
                    else
                    {
                        GlobalClass.Cart.Add(new CartItem
                        {
                            Medicine = medicine,
                            Quantity = 1
                        });
                    }
                    
                    MessageBox.Show("Товар добавлен в корзину", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new ProfilePage());
        }

        private void RolesButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new RolesPage());
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new ClientsPage());
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new EmployeesPage());
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new SuppliersPage());
        }

        private void ManufacturersButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new ManufacturersPage());
        }

        private void CountriesButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new CountriesPage());
        }

        private void CategoriesButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new CategoriesPage());
        }

        private void MedicinesButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new MedicinesPage());
        }

        private void MedicineBatchesButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new MedicineBatchesPage());
        }

        private void SuppliesButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new SuppliesPage());
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new OrdersPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.CurrentUser = null;
            GlobalClass.MainFrame.Navigate(new Auth.LoginPage());
        }
    }
}


