using ASPNET_SHAH.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPNET_SHAH.ViewModels
{
    public class CreateProductVM
    {
        public Product Product { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
        public IFormFile? File { get; set; }
    }
}
