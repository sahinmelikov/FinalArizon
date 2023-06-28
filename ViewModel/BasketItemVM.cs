namespace FinalArizon.ViewModel
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Price { get; set; }
        public double DiscountPrice { get; set; }
       
        public string ImagePath { get; set; }
        public int ProductCount { get; set; }

    
    }
}
