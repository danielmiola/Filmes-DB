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
        public async Task<ActionResult> Index(int? id = 1)
        {
            var reviews = await unitOfWork.ReviewsRepository.GetAsync(includeProperties: "Filmes");
            ViewBag.Count = reviews.Count();

            id = (id < 1) ? 1 : id;
            int add = (reviews.Count() % 5) > 0 ? 1 : 0;
            id = (id > (reviews.Count() / 5)) ? (reviews.Count() / 5) + add : id;

            ViewBag.Page = id;
            ViewBag.Max = (reviews.Count() / 5) + add;
            ViewBag.Entity = "Reviews";

            return View(reviews.Skip((id.GetValueOrDefault() - 1) * 5).Take(5));
        }

        // GET: /Reviews/Filme/5
        public async Task<ActionResult> Filme(int id)
        {
            var reviews = await unitOfWork.ReviewsRepository.GetAsync(filter: f => f.FilmeID == id, includeProperties: "Filmes");
            return View("Index", reviews);
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
        public async Task<ActionResult> Create([Bind(Include="FilmeID,Nota,Resenha")] Reviews reviews)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.ReviewsRepository.Insert(reviews);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            
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
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Reviews reviewToUpdate = await unitOfWork.ReviewsRepository.GetByIDAsync(id);

            if (TryUpdateModel(reviewToUpdate, "", new string[] { "FilmeID", "Nota", "Resenha" }))
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

            return View(reviewToUpdate);
        }

        // GET: /Reviews/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Falha ao apagar review. Tente novamente, e se o problema persistir contate o administrador do sistema.";
            }

            Reviews reviews = await unitOfWork.ReviewsRepository.GetByIDAsync(id);
            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // POST: /Reviews/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                unitOfWork.ReviewsRepository.Delete(id);
                await unitOfWork.SaveAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        // GET: /Reviews/AlteRate/5
        public ActionResult AlterRate(int? id = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            unitOfWork.AlterReviewsRate(id.GetValueOrDefault());
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
