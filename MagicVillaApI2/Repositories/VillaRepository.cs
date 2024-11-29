using MagicVillaApI2.Data;
using MagicVillaApI2.Models;
using MagicVillaApI2.Repositories.Interfaces;

namespace MagicVillaApI2.Repositories
{
    public class VillaRepository : IVillaRepository
    {

        readonly ApplicationDbContext _context;
        public VillaRepository(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public List<Villa> GetAll()
        {
            return _context.Villas.ToList();
        }

        public Villa GetById(int id)
        {
            return _context.Villas.FirstOrDefault(v => v.Id == id);
        }
    }
}
