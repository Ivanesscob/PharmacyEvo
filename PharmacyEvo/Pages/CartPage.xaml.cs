using System;
using System.Collections.Generic;
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
    public partial class CartPage : Page
    {
        private ObservableCollection<CartItemViewModel> _cartItems;

        public CartPage()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            _cartItems = new ObservableCollection<CartItemViewModel>();
            
            foreach (var cartItem in GlobalClass.Cart)
            {
                var viewModel = new CartItemViewModel
                {
                    Medicine = cartItem.Medicine,
                    Quantity = cartItem.Quantity
                };

                if (!string.IsNullOrEmpty(cartItem.Medicine.ImagePath))
                {
                    try
                    {
                        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cartItem.Medicine.ImagePath);
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

                _cartItems.Add(viewModel);
            }

            CartItemsControl.ItemsSource = _cartItems;
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            var total = _cartItems.Sum(item => item.Medicine.Price * item.Quantity);
            TotalPriceTextBlock.Text = $"{total:F2} ₽";
        }

        private void IncreaseQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is CartItemViewModel viewModel)
            {
                viewModel.Quantity++;
                var cartItem = GlobalClass.Cart.FirstOrDefault(item => item.Medicine.MedicineId == viewModel.Medicine.MedicineId);
                if (cartItem != null)
                {
                    cartItem.Quantity = viewModel.Quantity;
                }
                UpdateTotal();
            }
        }

        private void DecreaseQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is CartItemViewModel viewModel)
            {
                if (viewModel.Quantity > 1)
                {
                    viewModel.Quantity--;
                    var cartItem = GlobalClass.Cart.FirstOrDefault(item => item.Medicine.MedicineId == viewModel.Medicine.MedicineId);
                    if (cartItem != null)
                    {
                        cartItem.Quantity = viewModel.Quantity;
                    }
                }
                else
                {
                    RemoveItem(viewModel);
                }
                UpdateTotal();
            }
        }

        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is CartItemViewModel viewModel)
            {
                RemoveItem(viewModel);
                UpdateTotal();
            }
        }

        private void RemoveItem(CartItemViewModel viewModel)
        {
            _cartItems.Remove(viewModel);
            var cartItem = GlobalClass.Cart.FirstOrDefault(item => item.Medicine.MedicineId == viewModel.Medicine.MedicineId);
            if (cartItem != null)
            {
                GlobalClass.Cart.Remove(cartItem);
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Корзина пуста", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (GlobalClass.CurrentUser == null || !GlobalClass.CurrentUser.IsCustomer)
            {
                MessageBox.Show("Для оформления заказа необходимо войти в систему как клиент.", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Оформить заказ на сумму {TotalPriceTextBlock.Text}?", 
                "Подтверждение заказа", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                var orderItems = new List<OrderItem>();
                foreach (var cartItem in _cartItems)
                {
                    orderItems.Add(new OrderItem
                    {
                        MedicineId = cartItem.Medicine.MedicineId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Medicine.Price
                    });
                }

                int employeeId = 1;

                ProcedureDB.AddOrderWithItems(
                    GlobalClass.CurrentUser.UserId,
                    employeeId,
                    DateTime.Now,
                    orderItems
                );

                GlobalClass.Cart.Clear();
                _cartItems.Clear();
                CartItemsControl.ItemsSource = null;
                UpdateTotal();

                MessageBox.Show("Заказ успешно оформлен!", "Успех", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                BackButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оформлении заказа: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new HomePage());
        }
    }
}



