using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JokesAPI.Controllers;
using JokesAPI.Models;
using JokesAPI.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace JokesAPI.Tests.Controller
{

    public class JokesControllerTests
    {
        private readonly Mock<IJokesRepository> _mockRepo;
        private readonly JokesController _controller;
        private readonly Mock<ILogger<JokesController>> _mockLogger;

        const int NUMBER_OF_JOKES_TO_CREATE = 10;

        public JokesControllerTests()
        {
            // Setup Global Mock object for use by some of the test methods to speed up testing

            _mockRepo = new Mock<IJokesRepository>();
            _mockRepo.Setup(repo => repo.GetAllAsync()).Returns(GetTestJokesAsync(NUMBER_OF_JOKES_TO_CREATE));
            _mockLogger = new Mock<ILogger<JokesController>>();

            _controller = new JokesController(_mockRepo.Object, _mockLogger.Object);
        }

        #region Test GetAllJokes API


        [Fact]
        public async Task GetAllJokesAsync_Returns_Success()
        {
            // Arrange
            // .. _mockRepo instantiated in Constructor

            //Act
            var result = await _controller.GetAllJokesAsync() as OkObjectResult;
            List<JokeItem> actualResult = result.Value as List<JokeItem>;

            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(result.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status200OK);
            Assert.True(actualResult.Count == NUMBER_OF_JOKES_TO_CREATE);
        }

        [Fact]
        public async Task GetAllJokesAsync_Exception_Returns_BadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IJokesRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync()).Throws<Exception>();

            var controller = new JokesController(mockRepo.Object, _mockLogger.Object);

            //Act
            var result = await controller.GetAllJokesAsync();

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetAllJokesAsync_NoData_Returns_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IJokesRepository>();

            mockRepo.Setup(repo => repo.GetAllAsync()).Returns(Task.FromResult((IEnumerable<JokeItem>)null));

            var controller = new JokesController(mockRepo.Object, _mockLogger.Object);

            //Act
            var result = await controller.GetAllJokesAsync();

            //Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion Test GetAllJokes API

        #region Test GetJokeItem API

        [Fact]
        public async Task GetJokeItem_Valid_Id_Returns_Success()
        {
            int id = 1;
            // Arrange
            // .. _mockRepo instantiated in Constructor
            _mockRepo.Setup(repo => repo.GetItemAsync(id)).Returns(CreateJoke(id));

            //Act
            var result = await _controller.GetJokeItem((long)id);
            JokeItem item = ((OkObjectResult)result.Result).Value as JokeItem;
            //Assert
            Assert.IsType<ActionResult<JokeItem>>(result);
            Assert.Equal(id, item.Id);
        }

        [Fact]
        public async Task GetJokeItem_Null_Id_Returns_Not_Found()
        {
            // Arrange

            //Act
            var result = await _controller.GetJokeItem(null);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetJokeItem_Non_Existent_Id__Returns_Not_Found()
        {
            // Arrange

            //Act
            var result = await _controller.GetJokeItem(99);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetJokeItem_Exception_Returns_BadRequest()
        {

            // Arrange
            var mockRepo = new Mock<IJokesRepository>();
            mockRepo.Setup(repo => repo.GetItemAsync(1)).Throws<Exception>();

            var controller = new JokesController(mockRepo.Object, _mockLogger.Object);

            //Act
            var result = await controller.GetJokeItem(1);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        #endregion Test GetJokeItem API

        #region Test PostJokeItem API

        [Fact]
        public async Task PostJokeItem_Null_Parameter_Returns_BadRequest()
        {
            //Arrange

            // Act
            var result = await _controller.PostJokeItem(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostJokeItem_Add_Joke_Failed_Returns_BadRequest()
        {
            JokeItem item = new JokeItem { Id = 1, Joke = "Bad Joke" };

            //Arrange
            _mockRepo.Setup(repo => repo.Add(item)).Returns(new RepositoryStatus(false, "Failed to add new joke"));
            // Act
            var result = await _controller.PostJokeItem(item);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostJokeItem_Add_New_Joke_Returns_Success()
        {
            JokeItem item = new JokeItem { Id = 1, Joke = "Bad Joke" };

            //Arrange
            _mockRepo.Setup(repo => repo.Add(item)).Returns(new RepositoryStatus(true, "Added Successfully"));

            // Act
            var result = await _controller.PostJokeItem(item);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        #endregion Test PostJokeItem API

        #region Test PutJokeItem API

        [Fact]
        public async Task PutJokeItem_Null_Id_Returns_NotFound()
        {
            //Arrange
            JokeItem item = new JokeItem { Id = 1, Joke = "Testing" };
            // Act
            var result = await _controller.PutJokeItem(null, item);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PutJokeItem_JokeItem_Is_Null_Returns_NotFound()
        {
            //Arrange
            JokeItem item = null;
            // Act
            var result = await _controller.PutJokeItem(1, item);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PutJokeItem_Id_In_QueryString_Different_From_JokeItem_Returns_BadRequest()
        {
            //Arrange
            JokeItem item = new JokeItem { Id = 99, Joke = "Different Joke" };
            // Act
            var result = await _controller.PutJokeItem(1, item);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutJokeItem_Valid_Data_Returns_Success()
        {
            //Arrange
            JokeItem item = new JokeItem { Id = 1, Joke = "Test Joke" };
            // Act
            var result = await _controller.PutJokeItem(1, item);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        #endregion Test PutJokeItem API

        #region Test PagingJoke API

        [Fact]
        public async Task PagingJoke_Valid_Range_Returns_Success()
        {
            int pageIndex = 1;
            int pageSize = (int)(NUMBER_OF_JOKES_TO_CREATE / 2); ;

            // Arrange
            _mockRepo.Setup(repo => repo.GetJokesPage(pageIndex, pageSize)).Returns(GetTestJokesAsync(pageSize));

            //Act
            var result = await _controller.PagingJoke(pageIndex, pageSize);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task PagingJoke_Null_Page_Number_Returns_NotFound()
        {
            int pageSize = (int)(NUMBER_OF_JOKES_TO_CREATE / 2); ;

            // Arrange

            //Act
            var result = await _controller.PagingJoke(null, pageSize);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task PagingJoke_Null_Page_Size_Returns_NotFound()
        {
            int pageIndex = 1;

            // Arrange

            //Act
            var result = await _controller.PagingJoke(pageIndex, null);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task PagingJoke_Exception_Returns_BadRequest()
        {
            int pageIndex = 1;
            int pageSize = 5;

            // Arrange
            _mockRepo.Setup(repo => repo.GetJokesPage(pageIndex, pageSize)).Throws<Exception>();

            //Act
            var result = await _controller.PagingJoke(pageIndex, pageSize);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        #endregion Test PagingJoke API

        #region Test SearchJokes API

        [Fact]
        public async Task SearchJokes_Valid_Text_Returns_Success()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Contains("Joke")).Returns(GetTestJokesAsync(1));
            //Act
            var result = await _controller.SearchJokes("Joke");

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task SearchJokes_Null_Text_Returns_NotFound()
        {
            // Arrange

            //Act
            var result = await _controller.SearchJokes(null);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        #endregion Test SearchJokes API

        #region Test Random Joke API
        
        [Fact]
        public async Task RandomJokes_Count_Equals_Zero_Returns_NoContent()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.Count).Returns(0);

            //Act
            var result = await _controller.Random();

            //Assert
            Assert.IsType<NoContentResult>(result.Result);

        }

        [Fact]
        public async Task RandomJoke_Returns_Success()
        {
            //Arrange

            _mockRepo.Setup(repo => repo.Count).Returns(1);
            _mockRepo.Setup(repo => repo.GetItemAsync(1)).Returns(CreateJoke(1));

            //Act
            var result = await _controller.Random();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);

        }

        [Fact]
        public async Task Random_Max_Retry_Reached_Returns_NotFound()
        {
            //Arrange

            _mockRepo.Setup(repo => repo.Count).Returns(1);
            _mockRepo.Setup(repo => repo.GetItemAsync(1)).Returns(Task.FromResult((JokeItem)null));

            //Act
            var result = await _controller.Random();

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);

        }

        #endregion Test Random Joke API

        #region Utility Methods


        public async Task<JokeItem> CreateJoke(int id, string text = "Testing")
        {
            return await Task.Run(() =>
            {
                JokeItem joke = new JokeItem() { Id = id, Joke = text };

                return joke;
            });
        }

        private async Task<IEnumerable<JokeItem>> GetTestJokesAsync(int numberOfJokes)
        {
            return await Task.Run(() =>
            {
                List<JokeItem> list = new List<JokeItem>();

                for (int x = 1; x <= numberOfJokes; x++)
                {
                    list.Add(new JokeItem() { Id = x, Joke = $"Joke #{x}" });
                }

                return list;
            });
        }
        #endregion Utility Methods
    }
}
