using MagicVillaApI2.Data;
using MagicVillaApI2.Models;
using MagicVillaApI2.Repositories.Interfaces;

namespace MagicVillaApI2.Repositories
{
    public class VillaNumberRepository : GenericRepository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _context;

        public VillaNumberRepository(ApplicationDbContext _context) : base(_context)
        {
            this._context = _context;
        }
        public async Task Update(VillaNumber vn)
        {
            vn.UpdatedDate = DateTime.Now;
            _context.Update(vn);
        }
    }
}
