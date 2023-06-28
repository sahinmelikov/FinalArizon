using FinalArizon.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FinalArizon.ViewModel
{
    public class ParentsMenuCreateVM
    {
        public string Name { get; set; }
        public IFormFile Photo { get; set; }
    }
}
