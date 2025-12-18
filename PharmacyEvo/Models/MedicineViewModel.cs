using System.Windows.Media.Imaging;

namespace PharmacyEvo.Models
{
    public class MedicineViewModel
    {
        public int MedicineId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PriceText { get; set; }
        public string ImagePath { get; set; }
        public BitmapImage ImageSource { get; set; }
        public int CategoryId { get; set; }
    }
}


