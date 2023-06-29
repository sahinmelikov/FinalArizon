using System.ComponentModel.DataAnnotations;

namespace FinalArizon.ViewModel
{
    public class ParentsMenuUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Photo { get; set; }

    }
}
