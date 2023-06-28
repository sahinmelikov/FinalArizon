namespace FinalArizon.Models
{
    public class Cart
    {
        public List<Product> Products { get; set; }
        public int TotalItemCount => Products?.Count ?? 0;
    }
}
