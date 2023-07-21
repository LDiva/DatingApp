using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context,ITokenService tokenService)
        {
            _tokenService = tokenService;

            _context = context;

        }
        
          [HttpPost("register")] //post: /api/account/register?username=sam&password=pwd

        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
           if(await UserExists(registerDto.UserName)) return BadRequest("Username is taken");
       
           using var hmac = new HMACSHA256();

           var user = new AppUser
          {
            UserName = registerDto.UserName.ToLower(),
            Passwordhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            Passwordsalt = hmac.Key
          };

          _context.Users.Add(user);
          await _context.SaveChangesAsync();

          return new UserDto
          {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
          };
          
    
        }

        [HttpPost("login")]
         public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
          {
            
            var user= await _context.Users.SingleOrDefaultAsync(d => 
            d.UserName == loginDto.Username);

            if(user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA256(user.Passwordsalt);

            var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            
            for(int i =0; i<ComputeHash.Length; i++)
            {
              if (ComputeHash[i]!=user.Passwordhash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
              Username = user.UserName,
              Token = _tokenService.CreateToken(user)
            };

          }
        public async Task<bool> UserExists(string username)
         {
          return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
         }

    }
}