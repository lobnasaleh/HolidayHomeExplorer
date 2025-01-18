using MagicVillaApI2.Data;
using MagicVillaApI2.Models;
using MagicVillaApI2.Models.DTO;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVillaApI2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretkey;

        public UserRepository(ApplicationDbContext _db,IConfiguration configuration)
        {
            this._db = _db;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
           var user= _db.LocalUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
           var user=_db.LocalUsers.FirstOrDefault(u=>u.UserName.ToLower()==loginRequestDTO.UserName.ToLower()&& u.Password==loginRequestDTO.Password);
            if (user == null) {
               return new LoginResponseDTO()
               {
                   Token="",
                   User=null
               };
            }
            //generate jwt Token

            var tokenHandler=new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(secretkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                  new Claim (ClaimTypes.Name,user.Id.ToString())    ,
                  new Claim (ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token =tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token=tokenHandler.WriteToken(token),//ha ahtag a3melha serialize 3shan tethat fe string
                User=user
            };

        return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            LocalUser user = new LocalUser()
            {
                UserName = registerationRequestDTO.UserName,
                Name = registerationRequestDTO.Name,
                Password = registerationRequestDTO.Password,
                Role = registerationRequestDTO.Role
            };
          await  _db.LocalUsers.AddAsync(user);
          await  _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
