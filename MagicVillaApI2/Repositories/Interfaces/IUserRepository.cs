using MagicVillaApI2.Models;
using MagicVillaApI2.Models.DTO;

namespace MagicVillaApI2.Repositories.Interfaces
{
    public interface IUserRepository
    {
      Task<LoginResponseDTO> Login (LoginRequestDTO loginRequestDTO);
      Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);

      bool IsUniqueUser(string username);


    }
}
