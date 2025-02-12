using ASPNET_SHAH.Data;
using ASPNET_SHAH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNET_SHAH.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDBContext _dbContext;
        public CategoryController(AppDBContext context) 
        {
            _dbContext = context;
        }
        public IActionResult Index()
        {
            var categories = _dbContext.Categories.AsEnumerable();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            model.Id = Guid.NewGuid().ToString();
            _dbContext.Categories.Add(model);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
