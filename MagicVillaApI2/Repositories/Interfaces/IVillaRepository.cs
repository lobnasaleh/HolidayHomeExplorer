using MagicVillaApI2.Models;
using MagicVillaApI2.Models.DTO;
using System.Linq.Expressions;

namespace MagicVillaApI2.Repositories.Interfaces
{
    public interface IVillaRepository :IGenericRepository<Villa>
    {
        public Task Update(Villa v);


    }
}
