using MagicVillaApI2.Models;

namespace MagicVillaApI2.Repositories.Interfaces
{
    public interface IVillaNumberRepository:IGenericRepository<VillaNumber>
    {

        public Task Update(VillaNumber villaNumber);    
    }
}
