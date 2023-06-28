namespace FinalArizon.Models
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ParentsCategory ParentsCategory { get; set; }
        public int ParentsCategoryId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
