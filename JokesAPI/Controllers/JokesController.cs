using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.ApiErrors;
using JokesAPI.Models;
using JokesAPI.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using Serilog;

namespace JokesAPI.Controllers
{

    [Route("api/jokes")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly IJokesRepository _repository;
        private readonly Microsoft.Extensions.Logging.ILogger _log;

        public JokesController(IJokesRepository repository, ILogger<JokesController> logger)
        {
            logger.LogInformation("Jokes Controller CTOR"); //TODO: Learn ASP.Net Core's Resource/Localization

            _log = logger;

            _repository = repository;
        }


        /// <summary>
        /// GetAllJokesAsync returns all jokes in the database.
        /// </summary>
        /// <remarks>This returns all jokes. This is fine while the database is small.
        /// Consider using PageJokes as the data grow to maximize client responsiveness for the user</remarks>
        /// <returns>A list of Jokes</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllJokesAsync()
        {
            IEnumerable<JokeItem> jokes = null;
           
            try
            {
                _log.LogDebug("Building the list of jokes to return...");

                jokes = await _repository.GetAllAsync();
              

                if (jokes != null)
                    _log.LogInformation("Returning {NumJokes} jokes", jokes.Count());
                else
                    return NotFound(new NotFoundError("No Jokes found"));
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception in GetAllJokesAsync");

                return BadRequest(new BadRequestError("Exception occurred getting jokes due to exception: " + ex.Message));
            }

            return Ok(jokes);
        }


        /// <summary>
        /// GetJokeItem returns a specific joke associated with the supplied id
        /// </summary>
        /// <param name="id">Id of a joke in the Jokes database</param>
        /// <returns>StatusCodes.Status200OK if the joke is found, NotFound if the Joke does not exist, BadRequest on exception</returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<JokeItem>> GetJokeItem(long? id)
        {
            JokeItem joke = null;

            try
            {
                if (id == null)
                    return NotFound(new NotFoundError("Parameter 1: Id was not found or is missing. Please provide an id value"));

                _log.LogInformation("GetJokeItem.Id = {JokeId}", id.ToString());

                joke = await _repository.GetItemAsync((long)id);

                if (joke == null)
                {
                    _log.LogInformation("No Joke found with Id = {JokeId}", id.ToString());

                    return NotFound(new NotFoundError("No Joke found with Id = " + id.ToString()));
                }

                _log.LogInformation("returning Joke.Id = {JokeId}", id.ToString());
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception getting joke Id = {JokeId}", id.ToString());

                return BadRequest(new BadRequestError("Exception occurred getting joke id: " + id.ToString() + "\r\n" + ex.Message));
            }

            return Ok(joke);
        }

        /// <summary>
        /// PutJokeItem allows the client to modify an existing Joke
        /// </summary>
        /// <remarks>This API is restricted.  Users must be authenticated via the Login API
        /// 
        /// Status != Success returns a standard ApiError object
        /// 
        ///      {
        ///         "statusCode": 404,
        ///         "statusDescription": "NotFound",
        ///         "message": "No Joke found with Id = 123"
        ///      }
        /// </remarks>
        /// <param name="id">Id of the joke in the Jokes table</param>
        /// <param name="jokeItem">A JokeItem object</param>
        /// <response code="200">Success: Joke successfully updated</response>
        /// <response code="404">NotFound: Unabled to modify the Joke due to validation error</response>
        /// <response code="400">BadRequest: Exception occured inside the API method</response>
        /// <returns>StatusCodes.Status200OK (success), BadRequest (bad data or exception), or NotFound (if the Id does not exist)</returns>
        [Authorize]
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> PutJokeItem(long? id, JokeItem jokeItem)
        {
            if (id == null)
            {
                _log.LogWarning("PutJokeItem: user did not supply id value after the apiEndPoint");

                return NotFound(new NotFoundError("Parameter 1: Id was not found or is missing. Please provide an id value"));
            }

            if (jokeItem == null)
            {
                _log.LogWarning("PutJokeItem: no JokeItem was found in the parameters");

                return NotFound(new NotFoundError("Parameter 2: JokeItem was not found or is missing. Please provide a JokeItem"));
            }

            _log.LogInformation("PutJokeItem {JokeId}", id);
            

            if (id != jokeItem.Id)
            {
                _log.LogWarning("The Id and JokeItem.Id must match.  id: {JokeId} != JokeItem.Id: {JokeItemId}", id.ToString(), jokeItem.Id.ToString());

                return BadRequest(new BadRequestError("The Id and JokeItem.Id must match. Provide the id in the url and the body"));
            }

            try
            {
                _log.LogInformation("Saving new Joke Id = " + id.ToString());

                _repository.SetState(jokeItem, EntityState.Modified);
                await _repository.Commit();

                _log.LogInformation("Joke Id = {JokeId} saved successfully", id.ToString());
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                _log.LogError(dbEx, "DbUpdateConcurrencyException");

                if (!JokeItemExists(id ?? 0))
                {
                    return NotFound(new NotFoundError("Id = " + id + " does not exist or is missing"));
                }
                else
                {
                    _log.LogError("throwing from PutJokeItem for Id = {JokeId}", id.ToString());

                    return BadRequest(new BadRequestError("Updating Jokes DB faied due to this exception: " + dbEx.Message));
                }
            }
            
            return Ok("Joke.Id = " + id.ToString() + " updated successfully");
        }

        /// <summary>
        /// PostJokeItem allows the user to add jokes to the Jokes Database
        /// </summary>
        /// <remarks>This API is restricted.  Users must be authenticated via the Login API</remarks>
        /// <param name="jokeItem">JSON Representing a JokeItem Model</param>
        /// <returns>Success = StatusCodes.Status200OK or BadRequest if unsuccessful</returns>
        // POST: api/JokeItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<JokeItem>> PostJokeItem(JokeItem jokeItem)
        {
            try
            {
                if (jokeItem == null)
                    return BadRequest(new BadRequestError("Please provide a joke.  No joke was found."));

                _log.LogInformation("Post JokeItem.Id = {JokeId}", jokeItem.Id);

                RepositoryStatus retVal = _repository.Add(jokeItem);

                if (retVal.Success == false)
                {
                    _log.LogWarning("Post JokeItem.Id = {JokeId} already exists. New Joke not created.", jokeItem.Id);

                    return BadRequest(new BadRequestError(retVal.Message ?? "A joke already exists with that Id"));
                }

                await _repository.Commit();

                _log.LogInformation("JokeItem.Id = {JokeId} posted successfully", jokeItem.Id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "PostJokeItem threw an exeption");

                return BadRequest(new BadRequestError("PostJokeItem failed due to exception: " + ex.Message));
            }

            return CreatedAtAction(nameof(GetJokeItem), new { id = jokeItem.Id }, jokeItem);
        }

         /// <summary>
        /// DeleteJokeItem provides the ability to delete a joke from the datbase.
        /// </summary>
        /// <param name="id">Id of the Joke in the Jokes table</param>
        /// <remarks>This API is restricted.  Users must be authenticated via the Login API</remarks>
        /// <returns>If successfully deleted this method returns StatusCodes.Status200OK. 
        /// Otherwise it returns a BadRequest status code.</returns>
        [Authorize]
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<JokeItem>> DeleteJokeItem(long? id)
        {
            JokeItem joke = null;

            try
            {
                if (id == null)
                {
                    _log.LogWarning("PutJokeItem: user did not supply id value after the apiEndPoint");

                    return NotFound(new NotFoundError("Parameter 1: Id was not found or is missing. Please provide an id value"));
                }

                joke = await _repository.GetItemAsync((long)id);

                if (joke == null)
                {
                    return NotFound(new NotFoundError("We could not find a joke to delete with Id = " + id.ToString()));
                }

                _log.LogInformation("Removing Joke Id = {JokeId}", id.ToString());

                _repository.Remove(joke);

                await _repository.Commit();

                _log.LogInformation("Joke Id = {JokeId} deleted successfully", id.ToString());
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception deleting a joke.id = {JokeId}", id.ToString());

                return BadRequest(new BadRequestError("Unable to delete Joke Id = " + id.ToString() + "\r\n" + ex.Message));
            }

            return Ok("Joke.Id = " + joke.Id + " was successfully deleted");
        }

        private bool JokeItemExists(long id)
        {
            //TODO: Move this into the Repository class
            IEnumerable<JokeItem>item = _repository.Find(e => e.Id == id);

            if (item == null)
                return false;

            return item.Any();
        }

        /// <summary>
        /// PagingJoke provides a mechanism for paging through the span of jokes.
        /// </summary>
        /// <param name="pageNumber">Defines wich page number as the start</param>
        /// <param name="pageSize">Defines how many items appear on a page</param>
        /// <returns>A specified number of jokes on the requested page</returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<JokeItem>> PagingJoke(int? pageNumber, int? pageSize)
        {

            if (pageNumber == null)
                return NotFound(new NotFoundError("Parameter 1: pageNumber, was not found, mispelled, or missing a value"));

            if (pageSize == null)
                return NotFound(new NotFoundError("pageSize parameter not found,mispelled, or missing a value"));

            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 2;

            _log.LogInformation("Returning currentPageNumber = {currentPageNumber}, pageSize {currentPageSize}", currentPageNumber, currentPageSize);

            IEnumerable<JokeItem> list = null;

            try
            {
                list = await _repository.GetJokesPage(currentPageNumber, currentPageSize);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unknown Exception inside PagingJoke {currentPageNumber}, {currentPageSize}", currentPageNumber, currentPageSize);

                return BadRequest(new BadRequestError("Unable to get page of Jokes due to exception: " + ex.Message));
            }

            _log.LogInformation("Returning {Count} Jokes", (list != null ? list.Count().ToString() : "0"));

            return Ok(list);
        }

        /// <summary>
        /// SearchJokes provides the ability to search for jokes that 'contain' the search text.
        /// Future Plans: Extend this to look for 'StartsWith', 'Contains', or 'EndsWidth'.
        /// </summary>
        /// <param name="text">The text you are searching for in the jokes database.</param>
        /// <returns>Any joke that 'Contains" the search text value</returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<JokeItem>> SearchJokes(string text)
        {
            if (text == null)
                return NotFound(new NotFoundError("Parameter 1: text, was not found and is needed to perform a search"));

            try
            {
                _log.LogInformation("Searching for Jokes containing {SearchText}", text);

                IEnumerable<JokeItem> jokes = await _repository.Contains(text);

                if (jokes.Any())
                {
                    _log.LogInformation("{SearchText} was found {count} times", text, jokes.Count().ToString());

                    return Ok(jokes);
                }

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception searching for {SearchText} Jokes Database", text);

                return BadRequest(new BadRequestError("Unable to Search the Jokes DB due to this exception: " + ex.Message));
            }

            return BadRequest();
        }

        /// <summary>
        /// The Random API returns a random joke from the Jokes Database.
        /// </summary>
        /// <returns>A single joke from the jokes database</returns>
        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<JokeItem>> Random()
        {
            ActionResult<JokeItem> joke = null;

            int id = -1;

            try
            {
                int max = _repository.Count;

                if (max == 0)
                {
                    _log.LogWarning("No Jokes are in the Jokes DB.  Random joke generator cannot return a joke");

                    return NoContent();
                }

                Random rand = new Random();

                bool done = false;
                int retryAttemptsAllowed = 10;
                int retryAttempt = 1;

                //
                // C.Carter 1/13/2020
                //
                // DESIGN DECISION:
                //   This loop is not deterministic if we have items in the DB.
                //   We could return without a joke if we continue selecting non-existant ids in the table (user deleted them?)
                //   Better would be to create a list of the top N jokes in a List<T> collection
                //   and create a randome index into that.
                //
                // KNOWLEDGE GAP:
                //   How to select top 100 jokes using async methods
                //
                // FOR NOW:
                //   I have a simple While !done loop with a retry gate to make sure we at least get a product
                //   into the market.  The likely hood of wholes in our table is low and days off market
                //   potentially mean loss of marketshare.
                //

                while (!done)
                {
                    id = rand.Next(1, max);

                    joke = await _repository.GetItemAsync(id);

                    if (joke.Value == null)
                    {
                        if (retryAttempt > 10)
                        {
                            // Tried N times and failed to find a joke.  The Ids must not exist. 
                            // 911: Design Decision: Perhaps select the Top 100, put those in a list, and index the List randomly?

                            done = true;

                            _log.LogInformation("We were unable to find a random joke after {RetryAttempts} attempts", retryAttemptsAllowed);

                            return NotFound(new NotFoundError("we were unabel to find a random joke after = " + retryAttemptsAllowed + " attempts. Please try again."));
                        }
                        else
                        {
                            _log.LogInformation("The Random Id of = {JokeId} was not found DB. Retrying {Attempt} of {MaxAttempts} ", id, retryAttempt, retryAttemptsAllowed);

                            retryAttempt++;
                        }
                    }
                    else
                        done = true; // joke was found
                }

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception retrieving a random joke");

                return BadRequest(new BadRequestError("Unable to retrieve a random Joke: " + ex.Message));
            }

            _log.LogInformation("Random Joke.Id = {JokeId} is being returned", id);

            return Ok(joke);
        }
    }
}
