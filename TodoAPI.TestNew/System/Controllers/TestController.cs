using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.Services;
using TodoAPI.TestNew.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TodoAPI.TestNew.System.Controllers
{
    public class TestController
    {
        [Fact]
        public async Task GetAllAsync_Test()
        {
            //Arrange
            var todoservice = new Mock<IToDoService>();
            todoservice.Setup(x => x.GetAll()).Returns(TodoMockData.GetTodos());
            var sut = new TodoItemsController(todoservice.Object);

            //Act
            var result = (OkObjectResult) await sut.GetAllAsync();

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllAsync_EmptyTest()
        {
            /// Arrange
            var todoService = new Mock<IToDoService>();
            todoService.Setup(_ => _.GetAllAsync()).ReturnsAsync(TodoMockData.GetEmptyTodo());
            var sut = new TodoItemsController(todoService.Object);

            /// Act
            var result = (NoContentResult)await sut.GetAllAsync();

            /// Assert
            result.StatusCode.Should().Be(204);
            todoService.Verify(_ => _.GetAllAsync(), Times.Exactly(1));
        }

        [Fact]
        public async Task SaveAsync_PostTest()
        {
            //Arrange
            var todoService = new Mock<IToDoService>();
            
            var newTodo = TodoMockData.NewTodo();

            var sut = new TodoItemsController(todoService.Object);

            //Act
            var result = await sut.SaveAsync(newTodo);

            //Assert
            todoService.Verify(x => x.Add(newTodo), Times.Exactly(1));
        }
    }
}
