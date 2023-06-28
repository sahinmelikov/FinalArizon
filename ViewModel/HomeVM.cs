using FinalArizon.Models;

namespace FinalArizon.ViewModel
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
       
        public List<ParentsCategory> ParentsCategories { get; set; }
        public List<Slider> Sliders { get; set; }
        public Product SelectedProduct { get; set; }
        public List<Product> SameProductCodeProducts { get; set; }
        public List<Feature> Features { get; set; }
        public List<Model> Models { get; set; } = new List<Model>();
        public bool ShowOnlyBigDiscounts { get; set; }
    }
}
