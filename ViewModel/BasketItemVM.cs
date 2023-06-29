

using System.ComponentModel.DataAnnotations;

namespace FinalArizon.ViewModel
{
    public class BasketItemVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double DiscountPrice { get; set; }
        [Required]

        public string ImagePath { get; set; }
        [Required]
        public int ProductCount { get; set; }

    
    }
}
