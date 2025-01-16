using MagicVillaApI2.Models;
using System.Linq.Expressions;

namespace MagicVillaApI2.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<List<T>> GetAllAsyncWithExpression(Expression<Func<T, bool>>? filter = null,string? includeProperties=null);//law madetsh expression return all
        public Task<T> GetWithExpressionAsync(Expression<Func<T, bool>> filter = null, bool tracked = true,string? includeProperties = null);//l2eny fel method bta3t get by id sa3ta bakoon 3yza 22ol asnotracking
        public void Remove(T v);
        public Task AddAsync(T villa);
        public Task saveAsync();
        public Task<bool> ExistsByNameAsync(string name);

        // public Task< List<Villa>> GetAllAsync();
        // public Task<Villa> GetByIdAsync(int id);
        // public Task<Villa> GetByIdWithNoTrackingAsync(int id);
    }
}
