using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseService.Domain.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T?> GetByIdAsync(object id);
        IQueryable<T> GetQueryable();
    }
}
