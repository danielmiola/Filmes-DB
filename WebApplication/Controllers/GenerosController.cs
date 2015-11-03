using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.DAL;

namespace WebApplication.Controllers
{
    public class GenerosController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Generos/
        public async Task<ActionResult> Index(int? id = 1)
        {
            var genres = await unitOfWork.GenerosRepository.GetAsync();
            ViewBag.Count = genres.Count();

            id = (id < 1) ? 1 : id;
            int add = (genres.Count() % 5) > 0 ? 1 : 0;
            id = (id > (genres.Count() / 5)) ? (genres.Count() / 5) + add : id;

            ViewBag.Page = id;
            ViewBag.Max = (genres.Count() / 5) + add;
            ViewBag.Entity = "Generos";

            return View(genres.Skip((id.GetValueOrDefault() - 1) * 5).Take(5));
        }

        // GET: /Generos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generos generos = await unitOfWork.GenerosRepository.GetByIDAsync(id);
            if (generos == null)
            {
                return HttpNotFound();
            }
            return View(generos);
        }

        // GET: /Generos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Generos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Descricao")] Generos generos)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.GenerosRepository.Insert(generos);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(generos);
        }

        // GET: /Generos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generos generos = await unitOfWork.GenerosRepository.GetByIDAsync(id);
            if (generos == null)
            {
                return HttpNotFound();
            }
            return View(generos);
        }

        // POST: /Generos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var genreToUpdate = await unitOfWork.GenerosRepository.GetByIDAsync(id);

            if (TryUpdateModel(genreToUpdate, "", new string[] { "Descricao" }))
            {
                try
                {
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
           
            return View(genreToUpdate);
        }

        // GET: /Generos/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Falha ao apagar Gênero. Tente novamente, e se o problema persistir contate o administrador do sistema.";
            }

            Generos generos = await unitOfWork.GenerosRepository.GetByIDAsync(id);
            if (generos == null)
            {
                return HttpNotFound();
            }
            return View(generos);
        }

        // POST: /Generos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                unitOfWork.GenerosRepository.Delete(id);
                await unitOfWork.SaveAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
