using System;
using System.IO;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using PagedList;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class FilmController : Controller
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? page)
        {
            ViewBag.FilmsList = _unitOfWork.GetEntitys<Film>().ToPagedList(page ?? 1, 3);
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

                    model.AuthorId = User.Identity.GetUserId();

                    if (!_unitOfWork.SaveEntity(model)) throw new Exception("Error save entity");

                    TempData["infoMessage"] = "Film added";
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    TempData["infoMessage"] = $"An error has occurred: {exception}";
                    return View(model);
                }
            }
            return View(model);
        }


        public ActionResult Edit(Guid id)
        {
            try
            {
                var entity = _unitOfWork.GetEntity<Film>(id);

                if (entity == null)
                {
                    throw new Exception("Film not exist");
                }

                if (entity.AuthorId == User.Identity.GetUserId())
                {
                    Film.ExistedFile = entity.ImagePath;
                    return View(entity);
                }

                TempData["infoMessage"] = "Only  the author can remove/delete";

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["infoMessage"] = $"An error has occurred: {exception.Message}";
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
                    if (!_unitOfWork.SaveEntity(model)) throw new Exception("Error save entity");

                    TempData["infoMessage"] = "Film update";
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    TempData["infoMessage"] = $"Произошла ошибка: {exception.Message}";
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult Delete(Guid id)
        {
            try
            {
                var entity = _unitOfWork.GetEntity<Film>(id);

                if (entity == null)
                {
                    throw new Exception("Film not exist");
                }

                if (entity.AuthorId == User.Identity.GetUserId()) return View(entity);
                TempData["infoMessage"] = "Only  the author can remove/delete";
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["infoMessage"] = $"An error has occurred: {exception.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Film model)
        {
            try
            {
                if (!_unitOfWork.DeleteEntity<Film>(model.Id)) throw new Exception("Error save entity");
                TempData["infoMessage"] = "Film delete";
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["infoMessage"] = $"An error has occurred: {exception.Message}";
                return View(model);
            }
        }

        private void ValidateImage(ref Film model)
        {
            if (string.IsNullOrEmpty(Film.ExistedFile))
                Film.ExistedFile = model.File?.FileName;

            model.ImagePath = Film.ExistedFile;

            if (!string.IsNullOrEmpty(model.ImagePath))
            {
                ModelState.Remove("ImagePath");
            }
        }

        private void SaveImage(ref Film model)
        {
            model.File = model.File ?? Request.Files[0];

            if (!string.IsNullOrEmpty(model.File?.FileName))
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "images/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                model.File.SaveAs(Path.Combine(path, Path.GetFileName(model.File.FileName)));

                model.ImagePath = model.File.FileName;
            }
        }
    }
}