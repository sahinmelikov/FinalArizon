using FinalArizon.Models;
using System.ComponentModel.DataAnnotations;

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
        public double DiscountPrice { get; set; }
        public int ProductCode { get; set; }
        public string Delivery { get; set; } //////Çatdırılma
        public string Dimensions { get; set; }  ///////Ölçülər
        public int ProductQuantity { get; set; }   ///////BAzadaki MEhsullarin Sayi
        public int ModelId { get; set; }


        [Required]
        public IFormFile Photo { get; set; }
        public IFormFile ProductColorPhoto { get; set; }




    }
}
