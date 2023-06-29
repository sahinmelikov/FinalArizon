using System.ComponentModel.DataAnnotations;

namespace FinalArizon.ViewModel
{
    public class ProductUpdateVM
    {

        public int Id { get; set; }
        [Required, MaxLength(30)]
        public string? Name { get; set; }
        [Required, MaxLength(150)]
        public string? Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double DiscountPrice { get; set; }
        [Required]
        public int ProductCode { get; set; }
        [Required]
        public string Delivery { get; set; } //////Çatdırılma
        public string Dimensions { get; set; }  ///////Ölçülər
        public int ProductQuantity { get; set; }   ///////BAzadaki MEhsullarin Sayi
        public int ModelId { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public IFormFile ProductColorPhoto { get; set; }
    }
}
