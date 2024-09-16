using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data;
public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly RedditDbContext _context;
    public RepositoryBase(RedditDbContext context)
    {
        _context = context;
    }

    public void Add(T entity)
     => _context.Set<T>().Add(entity);

    public void Delete(T entity)
     => _context.Set<T>().Remove(entity);
    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        =>  _context.Set<T>().Where(expression);

    public IEnumerable<T> GetAll()
        => _context.Set<T>();

    public T GetById(int id)
        => _context.Set<T>().Find(id);

    public void Update(T entity)
     => _context.Set<T>().Update(entity);

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}