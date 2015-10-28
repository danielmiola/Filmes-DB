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
            var atores = await unitOfWork.AtoresRepository.GetByIDAsync(id);
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
        public async Task<ActionResult> Create([Bind(Include="Nome,DataNascimento")] Atores atores)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.AtoresRepository.Insert(atores);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
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
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var actorToUpdate = await unitOfWork.AtoresRepository.GetByIDAsync(id);
            
            if (TryUpdateModel(actorToUpdate, "", new string[] { "Nome","DataNascimento" }))
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

            return View(actorToUpdate);
        }

        // GET: /Atores/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Falha ao apagar ator. Tente novamente, e se o problema persistir contate o administrador do sistema.";
            }

            Atores atores = await unitOfWork.AtoresRepository.GetByIDAsync(id);
            if (atores == null)
            {
                return HttpNotFound();
            }
            return View(atores);
        }

        // POST: /Atores/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                unitOfWork.AtoresRepository.Delete(id);
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
