using FinalArizon.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FinalArizon.ViewModel
{
    public class ProductCreateVM
    {

        [Required, MaxLength(30)]
        public string Name { get; set; }
        [Required, MaxLength(150)]

        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double DiscountPrice { get; set; }
        [Required]
        public int ProductCode { get; set; }
        [Required]
        public string Delivery { get; set; } //////Çatdırılma
        [Required]
        public string Dimensions { get; set; }  ///////Ölçülər
        [Required]
        public int ProductQuantity { get; set; }   ///////BAzadaki MEhsullarin Sayi
        [Required]
        public int ModelId { get; set; }
        public DateTime DateTimeValue { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public IFormFile ProductColorPhoto { get; set; }




    }
}
