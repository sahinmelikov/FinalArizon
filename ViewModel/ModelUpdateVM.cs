using FinalArizon.Models;

namespace FinalArizon.ViewModel
{
    public class ModelUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentsCategoryId { get;set; }
        public List<Product> Products { get; set; }
    }
}
