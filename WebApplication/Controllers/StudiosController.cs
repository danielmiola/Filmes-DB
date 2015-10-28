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
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Studios/
        public async Task<ActionResult> Index()
        {
            return View(await unitOfWork.StudiosRepository.GetAsync());
        }

        // GET: /Studios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Studios studios = await unitOfWork.StudiosRepository.GetByIDAsync(id);
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
        public async Task<ActionResult> Create([Bind(Include="Nome,Cidade")] Studios studios)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.StudiosRepository.Insert(studios);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
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
            Studios studios = await unitOfWork.StudiosRepository.GetByIDAsync(id);
            if (studios == null)
            {
                return HttpNotFound();
            }
            return View(studios);
        }

        // POST: /Studios/Edit/5
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

            var studioToEdit = await unitOfWork.StudiosRepository.GetByIDAsync(id);

            if (TryUpdateModel(studioToEdit, "", new string[] { "Nome","Cidade" }))
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
            
            return View(studioToEdit);
        }

        // GET: /Studios/Delete/5
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

            Studios studios = await unitOfWork.StudiosRepository.GetByIDAsync(id);
            if (studios == null)
            {
                return HttpNotFound();
            }
            return View(studios);
        }

        // POST: /Studios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                unitOfWork.StudiosRepository.Delete(id);
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
