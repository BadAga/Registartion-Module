using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationForEuvic.Models;
using RegistrationForEuvic.Models.DTOs;
using RegistrationForEuvic.Models.Mappers;
using RegistrationForEuvic.Models.Password_Manager;

namespace RegistrationForEuvic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UsersController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // POST: api/Users/Login
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

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users/Register
        [HttpPost]
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
            _userDto.PhoneNumber = TransformToNoSepartorNumber(_userDto.PhoneNumber, ' ', '-');

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

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private int GetNewUserId()
        {
            if (!_context.Users.Any())
            {
                return 1;
            }

            return _context.Users.Max(x => x.UserId) + 1;
        }

        /// <summary>
        /// Deletes all separators from phone number.
        /// Assumption: each number have ONLYONE type of separator.
        /// </summary>
        /// <param name="phoneNumber">phone number with no separators</param>
        /// <param name="separtors">array of separtors</param>
        /// <returns></returns>
        private string TransformToNoSepartorNumber(string phoneNumber, params char[] separtors)
        {
            string noSparators = String.Empty;
            char? existingSeparator = null;

            foreach (char separator in separtors)
            {
                if (phoneNumber.Any(x => x == separator))
                {
                    existingSeparator = separator;
                    break;
                }
            }
            if (existingSeparator == null)
            {
                return phoneNumber;
            }

            string[] subNumbers = phoneNumber.Split((char)existingSeparator);
            noSparators = String.Join("", subNumbers);

            return noSparators;
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
