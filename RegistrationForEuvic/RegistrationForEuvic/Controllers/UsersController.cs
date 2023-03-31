using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationForEuvic.Managers;
using RegistrationForEuvic.Models;
using RegistrationForEuvic.Models.DTOs;
using RegistrationForEuvic.Models.Mappers;

namespace RegistrationForEuvic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {

        private IConfiguration _config;
        private readonly UserDbContext _context;

        public UsersController( IConfiguration config ,UserDbContext context)
        {
            _config = config;
            _context = context;
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // POST: api/Users/Login
        [Authorize]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<User>> GetUser([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _context.Users.Where(x => x.Email.Equals(loginDto.Email)).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }
            //checking password
            string hashedPasswordFromDB = user.Password;
            string saltFromDB = user.Salt;

            PasswordManager passwordManager = new PasswordManager(loginDto.Password, saltFromDB);
            if (!passwordManager.Compare(hashedPasswordFromDB))
            {
                return Unauthorized("Incorrect password");
            }

            return user;
        }


        // POST: api/Users/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterDto _userDto)
        {
            //validation check            
            if (!ModelState.IsValid)
            {
                return BadRequest("Wrong input format");
            }
            //checking if its a new user--> checking if PESEL, email and phone number are unique 
            if (!UniqueEmail(_userDto.Email))
            {
                return BadRequest("Email is not unique");
            }
            if (!UniquePesel(_userDto.Pesel))
            {
                return BadRequest("PESEL is not unique");
            }
            if (!UniquePhoneNumber(_userDto.PhoneNumber))
            {
                return BadRequest("Phone number is not unique");
            }

            int newId = GetNewUserId();
            //password hashing
            PasswordManager passManager = new PasswordManager(_userDto.Password);
            _userDto.Password = passManager.ComputedHashedPassword;

            //phone numbers are stored in db WITHOUT seperators
            _userDto.PhoneNumber = UserInputManager.FormatToNoSepartorNumber(_userDto.PhoneNumber, ' ', '-');

            //get the age
            int computedAge = UserInputManager.GetAgeBasedOnPesel(_userDto.Pesel);

            //get formated power usage
            _userDto.PowerUsageAvg = UserInputManager.FormatPowerUsage(_userDto.PowerUsageAvg);

            if (_userDto.Age != computedAge)
            {
                _userDto.Age = computedAge;
            }

            User user = UserMapper.RegisterDtoToUser(ref _userDto, ref newId);

            //for future login
            user.Salt = passManager.Salt;
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            string key = _config["Jwt:Key"];
            string token = UserTokenManager.GenerateToken(ref user, ref key);

            return Ok(token);

        }


        /// <summary>
        /// Checks if user with passed id already exists in database
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>true if user exists</returns>
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        /// <summary>
        /// Calculates new, unique user id
        /// </summary>
        /// <returns>user id</returns>
        private int GetNewUserId()
        {
            if (!_context.Users.Any())
            {
                return 1;
            }

            return _context.Users.Max(x => x.UserId) + 1;
        }             

        /// <summary>
        /// Checks if passed email value already exists in database
        /// </summary>
        /// <param name="email">email value</param>
        /// <returns>true if email is unique</returns>
        private bool UniqueEmail(String email)
        {
            return !_context.Users.Any(x => x.Email == email);
        }

        /// <summary>
        /// Checks if passed PESEL number value already exists in database
        /// </summary>
        /// <param name="pesel">PESEL number value</param>
        /// <returns>true if passed value is unique</returns>
        private bool UniquePesel(String pesel)
        {
            return !_context.Users.Any(x => x.Pesel == pesel);
        }

        /// <summary>
        /// Checks if passed phone number value already exists in database
        /// </summary>
        /// <param name="phoneNumber">phone number value</param>
        /// <returns>true if passed value is unique</returns>
        private bool UniquePhoneNumber(String phoneNumber)
        {
            return !_context.Users.Any(x => x.PhoneNumber == phoneNumber);
        }


    }

}
