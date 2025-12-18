using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using PharmacyEvo.Models;

namespace PharmacyEvo.Global
{
    public class GlobalClass : INotifyPropertyChanged
    {
        
        public static string ConnectionString { get; set; } = "Data Source=localhost;Initial Catalog=PharmacyDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";
        private static Frame _mainFrame = new Frame();

        public static User CurrentUser { get; set; }
        public static ObservableCollection<CartItem> Cart { get; set; } = new ObservableCollection<CartItem>();

        public static Frame MainFrame
        {
            get => _mainFrame;
            set
            {
                if (_mainFrame != value)
                {
                    _mainFrame = value;
                    OnStaticPropertyChanged(nameof(MainFrame));
                }
            }
        }

        public static event PropertyChangedEventHandler StaticPropertyChanged;

        private static void OnStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => StaticPropertyChanged += value;
            remove => StaticPropertyChanged -= value;
        }
    }
}
