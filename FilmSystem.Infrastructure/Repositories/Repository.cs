using System.Linq.Expressions;
using FilmSystem.Domain.Repositories;
using FilmSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmSystem.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly FilmSystemContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(FilmSystemContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll() => _dbSet.ToList();

        public virtual T? GetById(int id) => _dbSet.Find(id);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) =>
            _dbSet.Where(predicate).ToList();

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);
    }
}
