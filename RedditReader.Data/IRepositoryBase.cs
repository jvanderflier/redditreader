using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data;
public interface IRepositoryBase<T> where T : class
{
    T? GetById(int id);
    IEnumerable<T> GetAll(bool disableTracking = false);
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();

}
