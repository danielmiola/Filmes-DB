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
            if (ModelState.IsValid)
            {
                unitOfWork.PapeisRepository.Insert(papeis);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="FilmeID,AtorID,NomePersonagem")] Papeis papeis)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.PapeisRepository.Update(papeis);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AtorID = new SelectList(unitOfWork.AtoresRepository.Get(), "AtoresID", "Nome", papeis.AtorID);
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo", papeis.FilmeID);
            return View(papeis);
        }

        // GET: /Papeis/Delete/5
        public async Task<ActionResult> Delete(int? FilmeID, int? AtorID)
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

        // POST: /Papeis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int FilmeID, int AtorID)
        {
            unitOfWork.PapeisRepository.Delete(FilmeID, AtorID);
            await unitOfWork.SaveAsync();
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
