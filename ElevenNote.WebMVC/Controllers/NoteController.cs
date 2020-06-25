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
    [Authorize]
    public class NoteController : Controller
    {
        //CREATE___________________________________________
        //GET => Note/Create
        public ActionResult Create()
        {
            var service = CreateCategoryService();

            ViewBag.CategoryId = new SelectList(service.GetCategories(), "CategoryId", "Name");

            return View();
        }
        //POST => Note/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            var categoryService = CreateCategoryService();

            if (this.ModelState.IsValid)
            {
                var service = CreateNoteService();

                if (service.CreateNote(model))
                {
                    this.TempData["SaveResult"] = "Your note was created.";
                    return RedirectToAction("Index");
                }
                else
                {
                    this.ModelState.AddModelError("", "Note could not be created");
                    ViewBag.CategoryId = new SelectList(categoryService.GetCategories(), "CategoryId", "Name");
                    return View(model);
                }
            }
            else
            {
                ViewBag.CategoryId = new SelectList(categoryService.GetCategories(), "CategoryId", "Name");
                return View(model);
            }
        }

        //READ_____________________________________________
        //GET => Note/Index
        public ActionResult Index()
        {
            var service = CreateNoteService();
            var model = service.GetNotes();

            return View(model);
        }
        //GET => Note/Detail/{id}
        public ActionResult Details(int id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);
            return View(model);
        }


        //UPDATE___________________________________________
        //GET => Note/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreateNoteService();
            var categoryService = CreateCategoryService();
            var detail = service.GetNoteById(id);

            var model =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content,
                    CategoryId = detail.CategoryId,
                };

            this.ViewBag.CategoryId = new SelectList(categoryService.GetCategories(), "CategoryId", "Name", detail.CategoryId);
            return View(model);
        }
        //POST => Note/Edit/{id}
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (this.ModelState.IsValid)
            {
                if (model.NoteId != id)
                {
                    this.ModelState.AddModelError("", "Id Mismatch");
                    return View(model);
                }
                else
                {
                    var service = CreateNoteService();

                    if (service.UpdateNote(model))
                    {
                        TempData["SaveResult"] = "Your note was updated.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "Your note could not be updated.");
                        return View(model);
                    }
                }
            }
            else
                return View(model);
        }

        //DELETE___________________________________________
        //GET => Note/Delete/{id}
        public ActionResult Delete(int id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);

            return View(model);
        }
        //POST => Note/Delete/{id}
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public ActionResult DeleteNote(int id)
        {
            var service = CreateNoteService();

            service.DeleteNote(id);
           
            TempData["SaveResult"] = "Your note was deleted.";
            return RedirectToAction("Index");
        }

        //SERVICE METHODS__________________________________
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(this.User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }
        private CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(this.User.Identity.GetUserId());
            var service = new CategoryService(userId);
            return service;
        }
    }
}