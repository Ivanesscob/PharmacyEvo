using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public bool IsCustomer { get; set; }
        public bool IsManeger { get; set; }
        public bool IsAdmin { get; set; }
        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
            IsCustomer = GlobalClass.CurrentUser.IsCustomer;
            IsAdmin = GlobalClass.CurrentUser.IsAdmin;
            IsManeger = GlobalClass.CurrentUser.IsManeger;
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


