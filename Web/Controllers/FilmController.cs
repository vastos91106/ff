using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using PagedList;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class FilmController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            var model = _dbContext.Set<Film>().ToArray();
            ViewBag.FilmsList = model.ToPagedList(page ?? 1, 3);

            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Film());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Film model)
        {
            ValidateImage(ref model);

            if (ModelState.IsValid)
            {
                try
                {
                    SaveImage(ref model);

                    model.IdGuid = Guid.NewGuid();
                    model.AuthorId = User.Identity.GetUserId();
                    model.ImagePath = Request.Files[0].FileName;

                    _dbContext.Set<Film>().Add(model);
                    _dbContext.SaveChanges();

                    TempData["infoMessage"] = "Film added";
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    TempData["infoMessage"] = $"Произошла ошибка: {exception}";
                    return View(model);
                }

            }
            return View(model);
        }


        public ActionResult Edit(Guid id)
        {
            try
            {
                var entity = _dbContext.Set<Film>().Find(id);

                if (entity.AuthorId == User.Identity.GetUserId()) return View(entity);
                TempData["infoMessage"] = "Only  the author can remove/delete";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["infoMessage"] = "Film not exist";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Film model)
        {
            ValidateImage(ref model);

            if (ModelState.IsValid)
            {
                try
                {
                    SaveImage(ref model);

                    model.AuthorId = User.Identity.GetUserId();

                    _dbContext.Set<Film>().Attach(model);
                    _dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    _dbContext.SaveChanges();

                    TempData["infoMessage"] = "Film update";
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    TempData["infoMessage"] = $"Произошла ошибка: {exception}";
                    return View(model);
                }
            }

            return View(model);
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                var entity = _dbContext.Set<Film>().Find(id);

                if (entity.AuthorId == User.Identity.GetUserId()) return View(entity);
                TempData["infoMessage"] = "Only  the author can remove/delete";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["infoMessage"] = "Film not exist";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Film model)
        {
            try
            {
                _dbContext.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                _dbContext.Set<Film>().Remove(model);
                _dbContext.SaveChanges();

                TempData["infoMessage"] = "Film delete";
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["infoMessage"] = $"Произошла ошибка: {exception}";
                return View(model);
            }
        }

        private void ValidateImage(ref Film model)
        {
            if (string.IsNullOrEmpty(model.ImagePath) && !string.IsNullOrEmpty(Request.Form.Get("existImage")))
            {
                model.ImagePath = Request.Form.Get("existImage");
                ModelState.Remove("ImagePath");
            }
            else
            {
                model.ImagePath = Request.Files[0].FileName;
            }
        }

        private void SaveImage(ref Film model)
        {
            if (!string.IsNullOrEmpty(Request.Files[0].FileName))
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "images/";
                Request.Files[0].SaveAs(Path.Combine(path, Path.GetFileName(Request.Files[0].FileName)));

                model.ImagePath = Request.Files[0].FileName;
            }
        }
    }
}