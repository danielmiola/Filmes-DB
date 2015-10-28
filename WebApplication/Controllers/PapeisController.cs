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
    public class PapeisController : Controller
    {   
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Papeis/
        public async Task<ActionResult> Index()
        {
            return View(await unitOfWork.PapeisRepository.GetAsync(includeProperties: "Atores,Filmes"));
        }

        // GET: /Papeis/Details/5
        public async Task<ActionResult> Details(int? FilmeID, int? AtorID)
        {
            if (FilmeID == null || AtorID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Papeis papeis = await unitOfWork.PapeisRepository.GetByIDAsync(FilmeID, AtorID);
            if (papeis == null)
            {
                return HttpNotFound();
            }
            return View(papeis);
        }

        // GET: /Papeis/Create
        public ActionResult Create()
        {
            ViewBag.AtorID = new SelectList(unitOfWork.AtoresRepository.Get(), "AtoresID", "Nome");
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo");
            return View();
        }

        // POST: /Papeis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="FilmeID,AtorID,NomePersonagem")] Papeis papeis)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.PapeisRepository.Insert(papeis);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            ViewBag.AtorID = new SelectList(unitOfWork.AtoresRepository.Get(), "AtoresID", "Nome", papeis.AtorID);
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo", papeis.FilmeID);
            return View(papeis);
        }

        // GET: /Papeis/Edit/5
        public async Task<ActionResult> Edit(int? FilmeID, int? AtorID)
        {
            if (FilmeID == null || AtorID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Papeis papeis = await unitOfWork.PapeisRepository.GetByIDAsync(FilmeID, AtorID);
            if (papeis == null)
            {
                return HttpNotFound();
            }
            ViewBag.AtorID = new SelectList(unitOfWork.AtoresRepository.Get(), "AtoresID", "Nome", papeis.AtorID);
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo", papeis.FilmeID);
            return View(papeis);
        }

        // POST: /Papeis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? FilmeID, int? AtorID)
        {
            if (FilmeID == null || AtorID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var papelToUpdate = await unitOfWork.PapeisRepository.GetByIDAsync(FilmeID, AtorID);

            if (TryUpdateModel(papelToUpdate, "", new string[] { "NomePersonagem" }))
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

            ViewBag.AtorID = new SelectList(unitOfWork.AtoresRepository.Get(), "AtoresID", "Nome", papelToUpdate.AtorID);
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo", papelToUpdate.FilmeID);
            return View(papelToUpdate);
        }

        // GET: /Papeis/Delete/5
        public async Task<ActionResult> Delete(int? FilmeID, int? AtorID, bool? saveChangesError = false)
        {
            if (FilmeID == null || AtorID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Falha ao apagar personagem. Tente novamente, e se o problema persistir contate o administrador do sistema.";
            }

            Papeis papeis = await unitOfWork.PapeisRepository.GetByIDAsync(FilmeID, AtorID);
            if (papeis == null)
            {
                return HttpNotFound();
            }
            return View(papeis);
        }

        // POST: /Papeis/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int FilmeID, int AtorID)
        {
            try
            {
                unitOfWork.PapeisRepository.Delete(FilmeID, AtorID);
                await unitOfWork.SaveAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { FilmeID = FilmeID, AtorID = AtorID, saveChangesError = true });
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
