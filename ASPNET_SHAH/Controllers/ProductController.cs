using ASPNET_SHAH.Data;
using ASPNET_SHAH.Models;
using ASPNET_SHAH.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace ASPNET_SHAH.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsEnumerable();

            return View(products);
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.AsEnumerable();

            var viewModel = new CreateProductVM
            {
                Categories = categories.Select(c =>
                new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] CreateProductVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _context.Categories
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id });
                return View(viewModel);
            }

            string? fileName = viewModel.File != null ? SaveImage(viewModel.File) : null;

            viewModel.Product.Image = fileName;
            viewModel.Product.Id = Guid.NewGuid().ToString();

            _context.Products.Add(viewModel.Product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product model)
        {
            if (model.Image != null)
            {
                string imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, Settings.PRODUCTS_PATH);
                string imagePath = Path.Combine(imagesPath, model.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Products.Remove(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string? SaveImage(IFormFile file)
        {
            try
            {
                var types = file.ContentType.Split("/");

                if (types[0] != "image")
                {
                    return null;
                }

                string imageName = $"{Guid.NewGuid()}.{types[1]}";
                string imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, Settings.PRODUCTS_PATH);
                string imagePath = Path.Combine(imagesPath, imageName);

                using (var fileStream = System.IO.File.Create(imagePath))
                {
                    using (var stream = file.OpenReadStream())
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IActionResult Edit(string id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new CreateProductVM
            {
                Product = product,
                Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreateProductVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _context.Categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                });
                return View(viewModel);
            }

            var existingProduct = _context.Products.Find(viewModel.Product.Id);
            existingProduct.Name = viewModel.Product.Name;
            existingProduct.Description = viewModel.Product.Description;
            existingProduct.Price = viewModel.Product.Price;
            existingProduct.Amount = viewModel.Product.Amount;
            existingProduct.CategoryId = viewModel.Product.CategoryId;

            if (viewModel.File != null)
            {
                existingProduct.Image = SaveImage(viewModel.File);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
