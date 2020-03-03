using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.ApiErrors;
using JokesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;

namespace JokesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger _log;
        private readonly IMapper _mapper;

        public UserInfoController(AppDbContext context, ILogger<JokesController> logger, IMapper mapper)
        {
            _context = context;
            _log = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all users and their non-proteced information
        /// </summary>
        /// <returns>A list of all users in the system</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoDTO>>> GetUsers()
        {
            List<UserInfoDTO> target;

            try
            {
                _log.LogInformation("GetUsers API called");

                List<UserInfo> source = await _context.UserInfo.ToListAsync();

                target = _mapper.Map<List<UserInfoDTO>>(source);
            }
            catch (Exception ex)
            {
                _log.LogError("GetUsers.Exception: " + ex.Message);

                return BadRequest(new BadRequestError("Exception: " + ex.Message));
            }

            return Ok(target);
        }

        /// <summary>
        /// Returns all users including protected data such as passwords.
        /// Must be logged in and provide a JSON Web Token
        /// </summary>
        /// <returns>List of UserInfo objects</returns>
        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsersSecure()
        {
            List<UserInfo> source;

            try
            {
                _log.LogInformation("GetUsersSecure API called");

                source = await _context.UserInfo.ToListAsync();
            }
            catch (Exception ex)
            {
                _log.LogError("GetUsersSecure.Exception: " + ex.Message);

                return BadRequest(new BadRequestError("Exception: " + ex.Message));
            }

            return Ok(source);
        }

        /// <summary>
        /// Gets a specific user by Id.  Does not return protected information such as Password
        /// </summary>
        /// <param name="id">Id of an exising user</param>
        /// <returns>UserInfoDTO</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoDTO>> GetUserInfo(long id)
        {
            UserInfoDTO dto;

            try
            {
                _log.LogInformation("GetUserInfo by Id = {Id} API called", id);

                var userInfo = await _context.UserInfo.FindAsync(id);

                if (userInfo == null)
                {
                    return NotFound(new NotFoundError("A User with Id = " + id + " does not exist"));
                }

                dto = _mapper.Map<UserInfoDTO>(userInfo);

            }
            catch (Exception ex)
            {
                _log.LogError("GetUserInfo.Exception: " + ex.Message);

                return BadRequest(new BadRequestError("Exception: " + ex.Message));
            }

            return dto;
        }

        /// <summary>
        /// Updates the specified user
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="userInfo">UserInfo Model</param>
        /// <returns>Ok if successful</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserInfo(long id, UserInfo userInfo)
        {
            try
            {
                _log.LogInformation("PutUserInfo API called, {Id}", id);

                if (userInfo == null)
                    return BadRequest(new BadRequestError("Please provide the UserInfo you wish to modify"));

                if (id != userInfo.Id)
                {
                    return BadRequest(new BadRequestError("The Ids in the request did not match"));
                }

                _context.Entry(userInfo).State = EntityState.Modified;

                try
                {
                    _log.LogInformation("PutUserInfo Saving Changes for Id={Id}", id);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _log.LogError(ex, "Exception in PutUserInfo");

                    if (!UserInfoExists(id))
                    {
                        return NotFound(new NotFoundError("The Id = {Id} does not exist" + id));
                    }
                    else
                    {
                        return BadRequest(new BadRequestError("Exception occurred in PutUserInfo: " + ex.Message));
                    }
                }

                _log.LogInformation("UserInfo for Id={Id} saved", id);
            }
            catch (Exception ex)
            {
                _log.LogError("PutUserInfo.Exception: " + ex.Message);

                return BadRequest(new BadRequestError("Exception: " + ex.Message));
            }

            return Ok(userInfo);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="userInfo">UserInfo Model</param>
        /// <returns>The new user's information if successful</returns>
        [HttpPost]
        public async Task<ActionResult<UserInfo>> PostUserInfo(UserInfo userInfo)
        {
            CreatedAtActionResult result;

            try
            {

                if (userInfo == null)
                    return BadRequest(new BadRequestError("Please provide UserInfo to Post. No UserInfo was found."));

                _log.LogInformation("PostUserInfo API called");

                _log.LogInformation("Creating new User");

                _context.UserInfo.Add(userInfo);

                await _context.SaveChangesAsync();

                result = CreatedAtAction("GetUserInfo", new { id = userInfo.Id }, userInfo);
            }
            catch (Exception ex)
            {
                _log.LogError("PutUserInfo.Exception: " + ex.Message);

                return BadRequest(new BadRequestError("Exception: " + ex.Message));
            }

            return result;
        }

        /// <summary>
        /// Deletes the specified user
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        /// <returns>That user's info if successful</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInfo>> DeleteUserInfo(long id)
        {
            UserInfo userInfo;

            try
            {
                _log.LogInformation("DeleteUserInfo API called");

                userInfo = await _context.UserInfo.FindAsync(id);

                if (userInfo == null)
                {
                    _log.LogInformation("Unable to Delete: User.Id = {Id} does not exist", id);

                    return NotFound(new NotFoundError("User.Id = " + id.ToString() + " does not exist"));
                }

                _log.LogInformation("Deleting User.Id = {Id}", id);

                _context.UserInfo.Remove(userInfo);

                await _context.SaveChangesAsync();

                _log.LogInformation("User.Id = {Id} successfully deleted", id);
            }
            catch (Exception ex)
            {
                _log.LogError("DeleteUserInfo.Exception: " + ex.Message);

                return BadRequest(new BadRequestError("Exception: " + ex.Message));
            }

            return userInfo;
        }

        private bool UserInfoExists(long id)
        {
            return _context.UserInfo.Any(e => e.Id == id);
        }
    }
}
