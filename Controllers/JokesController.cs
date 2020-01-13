using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JokesAPI.Models;
using Microsoft.Extensions.Logging;

namespace JokesAPI.Controllers
{
    [Route("api/jokes")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly JokesContext _jokesContext;
        private readonly ILogger _log;

        public JokesController(JokesContext context, ILogger<JokesController> logger)
        {
            _log = logger;

            _log.LogInformation("Jokes Controller CTOR");

            _jokesContext = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JokeItem>>> GetJokeItems()
        {
            if (_jokesContext.JokeItems  == null)
                return Ok("The Jokie Jar is empty. Sorry folks"); // 911 Should I return Ok? It's not a failure

            List<JokeItem> jokes = null;

            try
            {
                
                if (_jokesContext.JokeItems == null)
                {
                    _log.LogWarning("There are no JokeItems. Returning empty");

                    return NotFound("There are no jokes in the database"); // 911 Is NotFound appropriate?
                }

                _log.LogDebug("Building the list of jokes to return...");

                jokes = await _jokesContext.JokeItems.ToListAsync();

                if (jokes != null)
                    _log.LogInformation("Returning {NumJokes} jokes", jokes.Count<JokeItem>());
                else
                    return NotFound("No Jokes found");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception in GetJokeItems");

                return BadRequest("Exception occurred getting jokes due to exception: " + ex.Message);
            }

            return Ok(jokes);
        }


        // GET: api/JokeItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JokeItem>> GetJokeItem(long? id)
        {
            JokeItem joke = null;

            try
            {
                if (id == null)
                    return NotFound("Parameter 1: Id was not found or is missing. Please provide an id value");

                _log.LogInformation("GetJokeItem.Id = {JokeId}", id.ToString());

                joke = await _jokesContext.JokeItems.FindAsync(id);

                if (joke == null)
                {
                    _log.LogInformation("No Joke found with Id = {JokeId}", id.ToString());

                    return NotFound();
                }

                _log.LogInformation("returning Joke.Id = {JokeId}", id.ToString());
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception getting joke Id = {JokeId}", id.ToString());

                return BadRequest("Exception occurred getting joke id: " + id.ToString() + "\r\n" + ex.Message);
            }

            return Ok(joke);
        }

        // PUT: api/JokeItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJokeItem(long? id, JokeItem jokeItem)
        {
            if (id == null)
            {
                _log.LogWarning("PutJokeItem: user did not supply id value after the apiEndPoint");

                return NotFound("Parameter 1: Id was not found or is missing. Please provide an id value");
            }

            if (jokeItem == null)
            {
                _log.LogWarning("PutJokeItem: no JokeItem was found in the parameters");

                return NotFound("Parameter 2: JokeItem was not found or is missing. Please provide a JokeItem");
            }

            _log.LogInformation("PutJokeItem {JokeId}", id);

            if (id != jokeItem.Id)
            {
                 _log.LogWarning("The Id and JokeItem.Id must match.  id: {JokeId} != JokeItem.Id: {JokeItemId}", id.ToString(), jokeItem.Id.ToString());

                return BadRequest("The Id and JokeItem.Id must match");
            }

            _jokesContext.Entry(jokeItem).State = EntityState.Modified;

            try
            {
                _log.LogInformation("Saving new Joke Id = " + id.ToString());

                await _jokesContext.SaveChangesAsync();

                _log.LogInformation("Joke Id = {JokeId} saved successfully", id.ToString());
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                _log.LogError(dbEx, "DbUpdateConcurrencyException");

                if (!JokeItemExists(id ?? 0))
                {
                    return NotFound();
                }
                else
                {
                    _log.LogError("throwing from PutJokeItem for Id = {JokeId}", id.ToString());

                    return BadRequest("Updating Jokes DB faied due to this exception: " + dbEx.Message);
                }
            }

            return NoContent();
        }

        // POST: api/JokeItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<JokeItem>> PostJokeItem(JokeItem jokeItem)
        {
            try
            {
                //911 Do I need to test jokeItem for null. Let's test this..

                _log.LogInformation("Post JokeItem.Id = {JokeId}", jokeItem.Id.ToString());

                if (!_jokesContext.JokeItems.Contains<JokeItem>(jokeItem))
                    _jokesContext.JokeItems.Add(jokeItem);
                else
                    return Ok("That JokeItem already exists.  Use the PUT to update or choose a unique Id");

                await _jokesContext.SaveChangesAsync();

                _log.LogInformation("JokeItem.Id = {JokeId} posted successfully", jokeItem.Id.ToString());
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "PostJokeItem threw an exeption");

                return BadRequest("PostJokeItem failed due to exception: " + ex.Message);
            }

            return CreatedAtAction(nameof(GetJokeItem), new { id = jokeItem.Id }, jokeItem);
        }

        // DELETE: api/JokeItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JokeItem>> DeleteJokeItem(long? id)
        {
            JokeItem joke = null;

            try
            {
                if (id == null)
                {
                    _log.LogWarning("PutJokeItem: user did not supply id value after the apiEndPoint");

                    return NotFound("Parameter 1: Id was not found or is missing. Please provide an id value");
                }

                joke = await _jokesContext.JokeItems.FindAsync(id);

                if (joke == null)
                {
                    return NotFound("We could not find a joke to delete with Id = " + id.ToString());
                }

                _log.LogInformation("Removing Joke Id = {JokeId}", id.ToString());

                _jokesContext.JokeItems.Remove(joke);

                await _jokesContext.SaveChangesAsync();

                _log.LogInformation("Joke Id = {JokeId} deleted successfully", id.ToString());
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception deleting a joke.id = {JokeId}", id.ToString());

                return BadRequest("Unable to delete Joke Id = " + id.ToString() + "\r\n" + ex.Message);
            }

            return Ok(joke);
        }

        private bool JokeItemExists(long id)
        {
            if (_jokesContext.JokeItems == null) //911 If this table doesn't exist we have bigger problems. 
                return false;

            return _jokesContext.JokeItems.Any(e => e.Id == id);
        }

        [HttpGet("[action]")]
        public IActionResult PagingJoke(int? pageNumber, int? pageSize)
        {
            // 911 - Can JokeItems be null? Perhaps if the DB is empty????

            var jokes = _jokesContext.JokeItems;

            if (pageNumber == null)
                return NotFound("Parameter 1: pageNumber, was not found, mispelled, or missing a value");

            if (pageSize == null)
                return NotFound("pageSize parameter not found,mispelled, or missing a value");

            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 2;

            _log.LogInformation("Returning currentPageNumber = {currentPageNumber}, pageSize {currentPageSize}", currentPageNumber, currentPageSize);

            IQueryable<JokeItem> list = null;

            try
            {
                list = jokes.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unknown Exception inside PagingJoke {currentPageNumber}, {currentPageSize}", currentPageNumber, currentPageSize);

                return BadRequest("Unable to get page of Jokes due to exception: " + ex.Message);
            }

            _log.LogInformation("Returning {Count} Jokes", list.Count<JokeItem>());

            return Ok(list);
        }

        [HttpGet("[action]")]
        public IActionResult SearchJokes(string text)
        {
            //911 add logging and exception handling

            if (text == null)
                return NotFound("Parameter 1: text, was not found and is needed to perform a search");

            IQueryable<JokeItem> list = null;

            try
            {
                _log.LogInformation("Searching for Jokes containing {SearchText}", text);

                list = _jokesContext.JokeItems.Where(j => j.Joke.Contains(text));
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Exception searching for {SearchText} Jokes Database", text);

                return BadRequest("Unable to Search the Jokes DB due to this exception: " + ex.Message);
            }

            _log.LogInformation("{SearchText} was found {count} times", text, list.Count<JokeItem>());
            return Ok(list);
        }
    }
}
