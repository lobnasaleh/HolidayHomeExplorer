using MagicVillaApI2.Data;
using MagicVillaApI2.Models;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVillaApI2.Repositories
{
    public class VillaRepository : GenericRepository<Villa>,IVillaRepository
    {

        private readonly ApplicationDbContext _context;

        public VillaRepository(ApplicationDbContext _context):base(_context)
        {
            this._context = _context;
           
        }


        //sebtha henna msh generic 3shan el update bykoon specifi lel entity

        public async Task Update(Villa v)
        {
            v.UpdatedDate = DateTime.Now;

           _context.Update(v);
           await _context.SaveChangesAsync();
        }
      
    }
}
/* public async Task<Villa> GetByIdAsync(int id)
       {
           return await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
       }

       public Task<Villa> GetByIdWithNoTrackingAsync(int id)
       {
           return _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

       }
       public async Task<List<Villa>> GetAllAsync()
       {
           List<Villa> Villas = await _context.Villas.ToListAsync();

           return Villas;
       }*/

