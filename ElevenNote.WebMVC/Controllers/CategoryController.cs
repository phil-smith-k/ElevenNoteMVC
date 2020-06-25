using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    public class CategoryController : Controller
    {
        //CREATE___________________________________________
        //GET => Note/Create
        public ActionResult Create()
        {
            return View();
        }
        //POST => Note/Create
        [HttpPost, ActionName("Create")]
        public ActionResult Create (CategoryCreate model)
        {
            if (this.ModelState.IsValid)
            {
                var service = CreateCategoryService();

                if (service.CreateCategory(model))
                {
                    this.TempData["SaveResult"] = "Your category was created.";
                    return RedirectToAction("Index");
                }
                else
                {
                    this.ModelState.AddModelError("", "Category could not be created.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
        //READ_____________________________________________
        //GET => Note/Index
        public ActionResult Index()
        {
            var service = CreateCategoryService();
            var categories = service.GetCategories();

            return View(categories);
        }
        //GET => Note/Detail/{id}
        public ActionResult Details(int id)
        {
            var service = CreateCategoryService();
            var category = service.GetCategoryById(id);

            return View(category);
        }
        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(this.User.Identity.GetUserId());
            return new CategoryService(userId);
        }
    }
}