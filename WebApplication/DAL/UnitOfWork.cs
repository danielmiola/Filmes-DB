using System;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.DAL
{
    public class UnitOfWork : IDisposable
    {
        private WEBApplicationContext context = new WEBApplicationContext();
        private GenericRepository<Atores> atoresRepository;
        private GenericRepository<Filmes> filmesRepository;
        private GenericRepository<Generos> generosRepository;
        private GenericRepository<Papeis> papeisRepository;
        private GenericRepository<Reviews> reviewsRepository;
        private GenericRepository<Studios> studiosRepository;

        public GenericRepository<Atores> AtoresRepository
        {
            get
            {

                if (this.atoresRepository == null)
                {
                    this.atoresRepository = new GenericRepository<Atores>(context);
                }
                return atoresRepository;
            }
        }

        public GenericRepository<Filmes> FilmesRepository
        {
            get
            {

                if (this.filmesRepository == null)
                {
                    this.filmesRepository = new GenericRepository<Filmes>(context);
                }
                return filmesRepository;
            }
        }

        public GenericRepository<Generos> GenerosRepository
        {
            get
            {

                if (this.generosRepository == null)
                {
                    this.generosRepository = new GenericRepository<Generos>(context);
                }
                return generosRepository;
            }
        }

        public GenericRepository<Papeis> PapeisRepository
        {
            get
            {

                if (this.papeisRepository == null)
                {
                    this.papeisRepository = new GenericRepository<Papeis>(context);
                }
                return papeisRepository;
            }
        }

        public GenericRepository<Reviews> ReviewsRepository
        {
            get
            {

                if (this.reviewsRepository == null)
                {
                    this.reviewsRepository = new GenericRepository<Reviews>(context);
                }
                return reviewsRepository;
            }
        }

        public GenericRepository<Studios> StudiosRepository
        {
            get
            {

                if (this.studiosRepository == null)
                {
                    this.studiosRepository = new GenericRepository<Studios>(context);
                }
                return studiosRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AlterReviewsRate(int dif)
        {
            context.AlterReviewsRate(dif);
        }
    }
}