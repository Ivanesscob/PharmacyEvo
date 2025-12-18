using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace PharmacyEvo.Models
{
    public class CartItemViewModel : INotifyPropertyChanged
    {
        private int _quantity;
        public Medicine Medicine { get; set; }
        public BitmapImage ImageSource { get; set; }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PriceText));
                OnPropertyChanged(nameof(TotalText));
            }
        }

        public string PriceText => $"{Medicine?.Price:F2} ₽";
        public string TotalText => $"{(Medicine?.Price ?? 0) * Quantity:F2} ₽";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


