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
    public class FilmesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Filmes/
        public async Task<ActionResult> Index()
        {
            return View(await unitOfWork.FilmesRepository.GetAsync());
        }

        // GET: /Filmes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filmes filmes = await unitOfWork.FilmesRepository.GetByIDAsync(id);
            if (filmes == null)
            {
                return HttpNotFound();
            }
            return View(filmes);
        }

        // GET: /Filmes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Filmes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="FilmeID,Titulo,Duracao,AnoLancamento")] Filmes filmes)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.FilmesRepository.Insert(filmes);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }

            return View(filmes);
        }

        // GET: /Filmes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filmes filmes = await unitOfWork.FilmesRepository.GetByIDAsync(id);
            if (filmes == null)
            {
                return HttpNotFound();
            }
            return View(filmes);
        }

        // POST: /Filmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="FilmeID,Titulo,Duracao,AnoLancamento")] Filmes filmes)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.FilmesRepository.Update(filmes);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(filmes);
        }

        // GET: /Filmes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filmes filmes = await unitOfWork.FilmesRepository.GetByIDAsync(id);
            if (filmes == null)
            {
                return HttpNotFound();
            }
            return View(filmes);
        }

        // POST: /Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            unitOfWork.FilmesRepository.Delete(id);
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
