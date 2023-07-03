

using System.Security.Cryptography;
using System.Text;
using ApplicationAPI.Data;
using ApplicationAPI.DTOs;
using ApplicationAPI.Entities;
using ApplicationAPI.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        public readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost("register")] // Path: api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
                return BadRequest("Username exists!");
            var user = _mapper.Map<AppUser>(registerDto);

            
            
            user.UserName = registerDto.Username.ToLower();
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded)
                return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => 
                    x.UserName == loginDto.Username);
            if (user == null)
                return Unauthorized("invalid username");
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!result)
                return Unauthorized();
            
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}