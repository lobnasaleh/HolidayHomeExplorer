using MagicVillaApI2.Data;
using MagicVillaApI2.Models;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVillaApI2.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public GenericRepository(ApplicationDbContext _context)
        {
            this._context = _context;
            this.dbSet=_context.Set<T>();//badal kol _context.villas hastakhdem dbset variable
        }


        public async Task AddAsync(T villa)
        {
            await dbSet.AddAsync(villa);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            bool res = await _context.Villas.AnyAsync(v => v.Name.ToLower() == name.ToLower());
            return res;
        }

        public async Task<List<T>> GetAllAsyncWithExpression(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet ;
            if (filter != null)
            {

                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }
        public async Task<T> GetWithExpressionAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {

                query = query.Where(filter);
            }

            if (!tracked)
            {

                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T v)
        {
            _context.Remove(v);
        }

        public async Task saveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
