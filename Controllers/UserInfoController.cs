using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.ApiErrors;
using JokesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JokesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly JokesContext _context;
        private readonly ILogger _log;

        public UserInfoController(JokesContext context, ILogger<JokesController> logger)
        {
            _context = context;
            _log = logger;
        }

        /// <summary>
        /// Gets all users:
        /// WARNING: Also returns the user's passwords in this first pass.
        /// </summary>
        /// <returns>A list of all users in the system</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUserInfo()
        {
            _log.LogInformation("GetUserInfo API called");

            return await _context.UserInfo.ToListAsync();
        }

        /// <summary>
        /// Gets a specific user by Id
        /// </summary>
        /// <param name="id">Id of an exising user</param>
        /// <returns>Information for the user specified</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserInfo(long id)
        {
            _log.LogInformation("GetUserInfo by Id = {Id} API called", id);

            var userInfo = await _context.UserInfo.FindAsync(id);

            if (userInfo == null)
            {
                return NotFound(new NotFoundError("A User with Id = " + id + " does not exist"));
            }

            return userInfo;
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
            if (userInfo == null)
                return BadRequest(new BadRequestError("Please provide UserInfo to Post. No UserInfo was found."));

            _log.LogInformation("PostUserInfo API called");
            
            _log.LogInformation("Creating new User");

            _context.UserInfo.Add(userInfo);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInfo", new { id = userInfo.Id }, userInfo);
        }

        /// <summary>
        /// Deletes the specified user
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        /// <returns>That user's info if successful</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInfo>> DeleteUserInfo(long id)
        {
            _log.LogInformation("DeleteUserInfo API called");

            var userInfo = await _context.UserInfo.FindAsync(id);
            if (userInfo == null)
            {
                _log.LogInformation("Unable to Delete: User.Id = {Id} does not exist", id);

                return NotFound(new NotFoundError("User.Id = " + id.ToString() + " does not exist"));
            }

            _log.LogInformation("Deleting User.Id = {Id}", id);

            _context.UserInfo.Remove(userInfo);

            await _context.SaveChangesAsync();

            _log.LogInformation("User.Id = {Id} successfully deleted", id);

            return userInfo;
        }

        private bool UserInfoExists(long id)
        {
            return _context.UserInfo.Any(e => e.Id == id);
        }
    }
}
