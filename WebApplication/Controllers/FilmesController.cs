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
using WebApplication.ViewModels;

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
            PopulateAssigned();
            return View();
        }

        // POST: /Filmes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Titulo,Duracao,AnoLancamento")] Filmes filmes, string[] selectedStudios, string[] selectedGeneros)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UpdateRelatedStudios(selectedStudios, filmes);
                    UpdateRelatedGenres(selectedGeneros, filmes);
                    unitOfWork.FilmesRepository.Insert(filmes);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
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
            var filmes = await unitOfWork.FilmesRepository.GetAsync( filter: f => f.FilmeID == id, includeProperties: "Studios,Generos" );
            Filmes filme = filmes.FirstOrDefault();
            PopulateAssigned(filme);
            if (filmes == null)
            {
                return HttpNotFound();
            }
            return View(filme);
        }

        // POST: /Filmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id, string[] selectedStudios, string[] selectedGeneros)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var filmes = await unitOfWork.FilmesRepository.GetAsync(filter: f => f.FilmeID == id, includeProperties: "Studios,Generos");
            var filmToUpdate = filmes.FirstOrDefault();

            if (TryUpdateModel(filmToUpdate, "", new string[] { "Titulo","Duracao","AnoLancamento" }))
            {
                try
                {
                    UpdateRelatedStudios(selectedStudios, filmToUpdate);
                    UpdateRelatedGenres(selectedGeneros, filmToUpdate);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            PopulateAssigned(filmToUpdate);
            return View(filmToUpdate);
        }

        // GET: /Filmes/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Falha ao apagar filme. Tente novamente, e se o problema persistir contate o administrador do sistema.";
            }

            Filmes filmes = await unitOfWork.FilmesRepository.GetByIDAsync(id);
            if (filmes == null)
            {
                return HttpNotFound();
            }
            return View(filmes);
        }

        // POST: /Filmes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                unitOfWork.FilmesRepository.Delete(id);
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

        private void PopulateAssigned(Filmes filme = null)
        {
            if (filme == null)
            {
                filme = new Filmes();
            }
            var allGenres = unitOfWork.GenerosRepository.Get();
            var allStudios = unitOfWork.StudiosRepository.Get();
            var filmGenres = new HashSet<int>(filme.Generos.Select(g => g.GeneroID));
            var filmStudios = new HashSet<int>(filme.Studios.Select(s => s.StudioID));
            var viewModelGenres = new List<AssignedData>();
            var viewModelStudios = new List<AssignedData>();
            
            foreach (var genre in allGenres)
            {
                viewModelGenres.Add(new AssignedData
                {
                    ID = genre.GeneroID,
                    Description = genre.Descricao,
                    Assigned = filmGenres.Contains(genre.GeneroID)
                });
            }

            foreach (var studio in allStudios)
            {
                viewModelStudios.Add(new AssignedData
                {
                    ID = studio.StudioID,
                    Description = studio.Nome,
                    Assigned = filmStudios.Contains(studio.StudioID)
                });
            }
            
            ViewBag.Generos = viewModelGenres;
            ViewBag.Studios = viewModelStudios;
        }

        private void UpdateRelatedStudios(string[] selectedStudios, Filmes filmeToUpdate)
        {
            if (selectedStudios == null)
            {
                filmeToUpdate.Studios = new List<Studios>();
                return;
            }

            var selectedStudiosHS = new HashSet<string>(selectedStudios);
            var filmeStudios = new HashSet<int>(filmeToUpdate.Studios.Select(s => s.StudioID));

            foreach (var studio in unitOfWork.StudiosRepository.Get())
            {
                if (selectedStudiosHS.Contains(studio.StudioID.ToString()))
                {
                    if (!filmeStudios.Contains(studio.StudioID))
                    {
                        filmeToUpdate.Studios.Add(studio);
                    }
                }
                else
                {
                    if (filmeStudios.Contains(studio.StudioID))
                    {
                        filmeToUpdate.Studios.Remove(studio);
                    }
                }
            }
        }

        private void UpdateRelatedGenres(string[] selectedGenres, Filmes filmeToUpdate)
        {
            if (selectedGenres == null)
            {
                filmeToUpdate.Generos = new List<Generos>();
            }
            else
            {
                var selectedGenresHS = new HashSet<string>(selectedGenres);
                var filmeGenres = new HashSet<int>(filmeToUpdate.Studios.Select(s => s.StudioID));

                foreach (var genre in unitOfWork.GenerosRepository.Get())
                {
                    if (selectedGenresHS.Contains(genre.GeneroID.ToString()))
                    {
                        if (!filmeGenres.Contains(genre.GeneroID))
                        {
                            filmeToUpdate.Generos.Add(genre);
                        }
                    }
                    else
                    {
                        if (filmeGenres.Contains(genre.GeneroID))
                        {
                            filmeToUpdate.Generos.Remove(genre);
                        }
                    }
                }
            }
        }
    }
}
