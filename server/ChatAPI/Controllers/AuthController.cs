﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Core;
using Chat.Core.Interfaces;
using Chat.Core.Resourses;
using Chat.Data.Entities;
using ChatAPI.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authRepository;
        private readonly IHubContext<BroadcastHub> _hubContext;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _config;
        public AuthController(IAuthService authRepository, IConfiguration config, IHubContext<BroadcastHub> hubContext,IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _config = config;
            _hubContext = hubContext;
			_unitOfWork = unitOfWork;
		}
        [Authorize]
        [Produces("application/json")]
        [HttpGet("all/{id}")]
        public async Task<IEnumerable<User>> GetUsers(int id)
        {
            var users = await _authRepository.GetAllUser(id);
            return users;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authRepository.GetUserById(id);
            var userFromDto = Mapping.Mapper.Map<UserForReturnDto>(user);
            return Ok(userFromDto);
        }

        [HttpPost("isUserExists")]
        public async Task<IActionResult> IsUserAlreadyExists([FromBody] string email)
        {

           var user = await _authRepository.Login(email.ToLower()); ;
           return Ok(user.Email);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationDto dto)
        {
            var userToCreate =Mapping.Mapper.Map<User>(dto);
            var createUser = await _authRepository.Register(userToCreate);
            _unitOfWork.Complete();
            await _hubContext.Clients.All.SendAsync("refreshUsers", createUser);
            return Ok(createUser);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var userFromRepo = await _authRepository.Login(dto.Email.ToLower());
            if (userFromRepo == null)
            {
                return Unauthorized("In valid credential");
            }
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:secretKey").Value));
            var credentiatl = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentiatl
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user = userFromRepo
            });
        }
    }
}
