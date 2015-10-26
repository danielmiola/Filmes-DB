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
        private WEBApplicationContext db = new WEBApplicationContext();

        // GET: /Papeis/
        public async Task<ActionResult> Index()
        {
            var papeis = db.Papeis.Include(p => p.Atores).Include(p => p.Filmes);
            return View(await papeis.ToListAsync());
        }

        // GET: /Papeis/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Papeis papeis = await db.Papeis.FindAsync(id);
            if (papeis == null)
            {
                return HttpNotFound();
            }
            return View(papeis);
        }

        // GET: /Papeis/Create
        public ActionResult Create()
        {
            ViewBag.AtorID = new SelectList(db.Atores, "AtoresID", "Nome");
            ViewBag.FilmeID = new SelectList(db.Filmes, "FilmeID", "Titulo");
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
                db.Papeis.Add(papeis);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AtorID = new SelectList(db.Atores, "AtoresID", "Nome", papeis.AtorID);
            ViewBag.FilmeID = new SelectList(db.Filmes, "FilmeID", "Titulo", papeis.FilmeID);
            return View(papeis);
        }

        // GET: /Papeis/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Papeis papeis = await db.Papeis.FindAsync(id);
            if (papeis == null)
            {
                return HttpNotFound();
            }
            ViewBag.AtorID = new SelectList(db.Atores, "AtoresID", "Nome", papeis.AtorID);
            ViewBag.FilmeID = new SelectList(db.Filmes, "FilmeID", "Titulo", papeis.FilmeID);
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
                db.Entry(papeis).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AtorID = new SelectList(db.Atores, "AtoresID", "Nome", papeis.AtorID);
            ViewBag.FilmeID = new SelectList(db.Filmes, "FilmeID", "Titulo", papeis.FilmeID);
            return View(papeis);
        }

        // GET: /Papeis/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Papeis papeis = await db.Papeis.FindAsync(id);
            if (papeis == null)
            {
                return HttpNotFound();
            }
            return View(papeis);
        }

        // POST: /Papeis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Papeis papeis = await db.Papeis.FindAsync(id);
            db.Papeis.Remove(papeis);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
