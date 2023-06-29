using FinalArizon.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FinalArizon.ViewModel
{
    public class ParentsMenuCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
