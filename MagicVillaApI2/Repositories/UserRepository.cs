using AutoMapper;
using MagicVillaApI2.Data;
using MagicVillaApI2.Models;
using MagicVillaApI2.Models.DTO;
using MagicVillaApI2.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVillaApI2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IMapper _mapper;
        private string secretkey;

        public UserRepository(ApplicationDbContext _db,IConfiguration configuration, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IMapper _mapper)
        {
            this._db = _db;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            this._mapper = _mapper;
            secretkey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
           var user= _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
           var user=_db.ApplicationUsers.FirstOrDefault(u=>u.UserName.ToLower()==loginRequestDTO.UserName.ToLower());
           
           bool isvalid=await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            
            
            if (user == null || !isvalid ) {
               return new LoginResponseDTO()
               {
                   Token="",
                   User=null
               };
            }
            //to get the roles of the user
            var roles=await _userManager.GetRolesAsync(user);


            //generate jwt Token

            var tokenHandler=new JwtSecurityTokenHandler();
            var key=Encoding.ASCII.GetBytes(secretkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                  new Claim (ClaimTypes.Name,user.Id.ToString())    ,
                  new Claim (ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token =tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token=tokenHandler.WriteToken(token),//ha ahtag a3melha serialize 3shan tethat fe string
                User=_mapper.Map<UserDTO>(user),
               // Role=roles.FirstOrDefault()
            };

        return loginResponseDTO;
        }

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.Name,
                NormalizedEmail=registerationRequestDTO.Name.ToUpper(),
                Name = registerationRequestDTO.Name
               
            };
            try { 
                //password
                var result=await _userManager.CreateAsync(user,registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                    //create role

                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("User"));
                    }
                    //assign role
                    await _userManager.AddToRoleAsync(user, "User");

                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registerationRequestDTO.UserName);

                    return _mapper.Map<UserDTO>(userToReturn);
                }
              
            }catch(Exception ex) { 

            }
           
            return new UserDTO();
        }
    }
}
