using Villa_Web.Models.DTO;

namespace Villa_Web.Services.IServices
{
    public interface IVillaNumberService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id); 
        Task<T> CreateAsync<T>(VillaNumberCreateDTO entity);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO entity);
        Task<T> DeleteAsync<T>(int id);

    }
}
