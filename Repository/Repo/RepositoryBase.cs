using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ThienASPMVC08032023.Database;
using ThienASPMVC08032023.Repository.InterfaceRepo;

namespace ThienASPMVC08032023.Repository.Repo
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private AppDbContext _context { get; set; }
        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }


        public IQueryable<T> FindAll()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);

        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

    }
}
