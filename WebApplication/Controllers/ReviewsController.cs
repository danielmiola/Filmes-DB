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
    public class ReviewsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Reviews/
        public async Task<ActionResult> Index()
        {
            return View(await unitOfWork.ReviewsRepository.GetAsync(includeProperties: "Filmes"));
        }

        // GET: /Reviews/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await unitOfWork.ReviewsRepository.GetByIDAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // GET: /Reviews/Create
        public ActionResult Create()
        {
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo");
            return View();
        }

        // POST: /Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ReviewID,FilmeID,UserID,Nota,Resenha")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ReviewsRepository.Insert(reviews);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo", reviews.FilmeID);
            return View(reviews);
        }

        // GET: /Reviews/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await unitOfWork.ReviewsRepository.GetByIDAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo", reviews.FilmeID);
            return View(reviews);
        }

        // POST: /Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ReviewID,FilmeID,UserID,Nota,Resenha")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.ReviewsRepository.Update(reviews);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FilmeID = new SelectList(unitOfWork.FilmesRepository.Get(), "FilmeID", "Titulo", reviews.FilmeID);
            return View(reviews);
        }

        // GET: /Reviews/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reviews reviews = await unitOfWork.ReviewsRepository.GetByIDAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // POST: /Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            unitOfWork.ReviewsRepository.Delete(id);
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
