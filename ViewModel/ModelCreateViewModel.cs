using FinalArizon.Models;

namespace FinalArizon.ViewModel
{
    public class ModelCreateViewModel
    {
        public string Name { get; set; }
        public int ParentsCategoryId { get; set; }
        public List<Product> Products { get; set; }
    }
}
