using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hachi.Data;
using Hachi.Models;
using Hachi.Models.SubCategoryViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hachi.Controllers
{
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        [TempData]
        public string StatusMessage { get; set; }

        public SubCategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var subCategories = _db.SubCategory.Include(s => s.Category);
            return View(await subCategories.ToListAsync());
        }
        //GET Action For Create
        public IActionResult Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = _db.Category.ToList(),
                SubCategory = new SubCategory(),
                SubCategoryList = _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToList()
            };
            return View(model);
        }
        //Post Create
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExits = _db.SubCategory.Where(s => s.Name == model.SubCategory.Name).Count();
                var doesSubCatAndCatExits = _db.SubCategory.Where(s => s.Name == model.SubCategory.Name && s.CategoryId == model.SubCategory.CategoryId).Count();
                if (doesSubCategoryExits > 0 && model.isNew)
                {
                    //Erro
                    StatusMessage = "Error: Subcategory Name already Exist";
                }
                else
                {
                    if (doesSubCatAndCatExits == 0 && !model.isNew)
                    {
                        StatusMessage = "Error: Subcategory Name doesn't Exist";
                    }
                    else
                    {
                        if (doesSubCatAndCatExits > 0)
                        {
                            StatusMessage = "Error: Category and CategoryName combination existis";
                        }
                        else
                        {
                            _db.Add(model.SubCategory);
                            await _db.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = _db.Category.ToList(),
                SubCategory = model.SubCategory,
                SubCategoryList = _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToList(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }
    }
}