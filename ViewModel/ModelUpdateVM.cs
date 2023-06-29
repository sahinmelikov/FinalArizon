using FinalArizon.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalArizon.ViewModel
{
    public class ModelUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ParentsCategoryId { get;set; }
        //public List<Product> Products { get; set; }
    }
}
