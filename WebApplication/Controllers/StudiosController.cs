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
    public class StudiosController : Controller
    {
        private WEBApplicationContext db = new WEBApplicationContext();

        // GET: /Studios/
        public async Task<ActionResult> Index()
        {
            return View(await db.Studios.ToListAsync());
        }

        // GET: /Studios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Studios studios = await db.Studios.FindAsync(id);
            if (studios == null)
            {
                return HttpNotFound();
            }
            return View(studios);
        }

        // GET: /Studios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Studios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="StudioID,Nome,Cidade")] Studios studios)
        {
            if (ModelState.IsValid)
            {
                db.Studios.Add(studios);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(studios);
        }

        // GET: /Studios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Studios studios = await db.Studios.FindAsync(id);
            if (studios == null)
            {
                return HttpNotFound();
            }
            return View(studios);
        }

        // POST: /Studios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="StudioID,Nome,Cidade")] Studios studios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studios).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(studios);
        }

        // GET: /Studios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Studios studios = await db.Studios.FindAsync(id);
            if (studios == null)
            {
                return HttpNotFound();
            }
            return View(studios);
        }

        // POST: /Studios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Studios studios = await db.Studios.FindAsync(id);
            db.Studios.Remove(studios);
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
