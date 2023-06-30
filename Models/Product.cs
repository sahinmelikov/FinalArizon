namespace FinalArizon.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public double Price { get; set; }
        public double DiscountPrice { get; set; }
        public string ImagePath { get; set; }
        public int ViewCount { get; set; }  ////////BAxish Sayi
        public int ProductCode{get;set; }  ///////Mehsulun Kodu
        public string Delivery { get; set; } //////Çatdırılma
        public string Dimensions { get; set; }  ///////Ölçülər
        public int ProductQuantity { get; set; }   ///////BAzadaki MEhsullarin Sayi
       
        public string ProductİmageColor { get;set; }   ////////Producttun Rengi
        public DateTime DateTimeValue { get; set; }
        public List<Feature> Features { get; set; }
        public Model Model { get; set; }
        public int ModelId { get; set; }
       
    }
}
