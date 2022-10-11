using AutoMapper;
using JwtAuthWebAPiProject.Abstractions;
using JwtAuthWebAPiProject.DTOs;
using JwtAuthWebAPiProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthWebAPiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            //create user output model to view
            var response = new UserResponse();
            var userOutput = users.Select(u => _mapper.Map(u, response)).ToList();
            _mapper.Map(users, userOutput);
            return Ok(userOutput);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest($"User with this email already exists ({request.Email}) ");
            }

            User user = new User();
            _mapper.Map(request, user);

            //output model for created user
            var createdUser = await _userRepository.CreateAsync(user);
            var userInputModel = new UserResponse();
            _mapper.Map(createdUser, userInputModel);
            return Ok(userInputModel);


        }

    }
}
