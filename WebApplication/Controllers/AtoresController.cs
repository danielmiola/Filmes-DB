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
    public class AtoresController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Atores/
        public async Task<ActionResult> Index()
        {
            return View(await unitOfWork.AtoresRepository.GetAsync());
        }

        // GET: /Atores/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atores atores = await unitOfWork.AtoresRepository.GetByIDAsync(id);
            if (atores == null)
            {
                return HttpNotFound();
            }
            return View(atores);
        }

        // GET: /Atores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Atores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="AtoresID,Nome,DataNascimento")] Atores atores)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AtoresRepository.Insert(atores);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }

            return View(atores);
        }

        // GET: /Atores/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atores atores = await unitOfWork.AtoresRepository.GetByIDAsync(id);
            if (atores == null)
            {
                return HttpNotFound();
            }
            return View(atores);
        }

        // POST: /Atores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="AtoresID,Nome,DataNascimento")] Atores atores)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.AtoresRepository.Update(atores);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(atores);
        }

        // GET: /Atores/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atores atores = await unitOfWork.AtoresRepository.GetByIDAsync(id);
            if (atores == null)
            {
                return HttpNotFound();
            }
            return View(atores);
        }

        // POST: /Atores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            unitOfWork.AtoresRepository.Delete(id);
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
