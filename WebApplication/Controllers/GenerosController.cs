﻿using System;
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
    public class GenerosController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Generos/
        public async Task<ActionResult> Index()
        {
            return View(await unitOfWork.GenerosRepository.GetAsync());
        }

        // GET: /Generos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generos generos = await unitOfWork.GenerosRepository.GetByIDAsync(id);
            if (generos == null)
            {
                return HttpNotFound();
            }
            return View(generos);
        }

        // GET: /Generos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Generos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="GeneroID,Descricao")] Generos generos)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.GenerosRepository.Insert(generos);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }

            return View(generos);
        }

        // GET: /Generos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generos generos = await unitOfWork.GenerosRepository.GetByIDAsync(id);
            if (generos == null)
            {
                return HttpNotFound();
            }
            return View(generos);
        }

        // POST: /Generos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="GeneroID,Descricao")] Generos generos)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.GenerosRepository.Update(generos);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(generos);
        }

        // GET: /Generos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generos generos = await unitOfWork.GenerosRepository.GetByIDAsync(id);
            if (generos == null)
            {
                return HttpNotFound();
            }
            return View(generos);
        }

        // POST: /Generos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            unitOfWork.GenerosRepository.Delete(id);
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
