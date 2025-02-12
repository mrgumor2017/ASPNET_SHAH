using ASPNET_SHAH.Data;
using ASPNET_SHAH.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPNET_SHAH.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _dbContext;
        public ProductController(AppDBContext context)
        {
            _dbContext = context;
        }
        public ActionResult Index()
        {
            var products = _dbContext.Products.AsEnumerable();

            return View(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dbContext.Categories, "Id", "Name");
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_dbContext.Categories, "Id", "Name");
                return View(model);
            }

            model.Id = Guid.NewGuid().ToString();
            _dbContext.Products.Add(model);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(string id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_dbContext.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_dbContext.Categories, "Id", "Name", model.CategoryId);
                return View(model);
            }

            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Amount = model.Amount;
            product.Image = model.Image;
            product.CategoryId = model.CategoryId;

            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // GET: ProductController/Delete/5
        public ActionResult Delete(string id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
