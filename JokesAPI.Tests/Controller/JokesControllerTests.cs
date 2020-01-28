using JokesAPI.Models;
using JokesAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using JokesAPI.Contracts;
using JokesAPI.Persistence;
using Serilog;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JokesAPI.Tests.Controller
{

    public class JokesControllerTests
    {
        private readonly Mock<JokeItemRepository> _mockRepo;
        private readonly JokesController _controller;

        public JokesControllerTests()
        {
            _mockRepo = new Mock<JokeItemRepository>();

            _controller = new JokesController(_mockRepo.Object);
        }

        [Fact]
        public void GetAll_ActionExecutes_ReturnsAllJokeItems()
        {
            // TODO: Figure out how to use Mock with Async

        }
    }
}
