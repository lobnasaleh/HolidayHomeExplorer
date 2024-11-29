using MagicVillaApI2.Models;

namespace MagicVillaApI2.Repositories.Interfaces
{
    public interface IVillaRepository
    {
        public List<Villa> GetAll();
        public Villa GetById(int id);
    }
}
