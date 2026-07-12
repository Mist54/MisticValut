using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MisticVault.Api.Controllers;
using MisticVault.Application.Todo.DTOs.Todo;
using MisticVault.Application.Todo.Interfaces;
using Moq;
using Xunit;

namespace MisticVault.Tests
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoService> _mockService;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockService = new Mock<ITodoService>();
            _controller = new TodoController(_mockService.Object);
        }

        // ==========================================
        // 1. GET ALL TESTS
        // ==========================================
        [Fact]
        public async Task GetAll_ReturnsOk_WithListOfItems()
        {
            // Arrange
            var mockList = new List<TodoResponseDTO>
            {
                new TodoResponseDTO { Id = Guid.NewGuid(), Title = "Task 1" },
                new TodoResponseDTO { Id = Guid.NewGuid(), Title = "Task 2" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnItems = Assert.IsType<List<TodoResponseDTO>>(okResult.Value);
            Assert.Equal(2, returnItems.Count);
        }

        // ==========================================
        // 2. GET BY ID TESTS
        // ==========================================
        [Fact]
        public async Task GetById_ReturnsOk_WhenItemExists()
        {
            var todoId = Guid.NewGuid();
            var expectedResponse = new TodoResponseDTO { Id = todoId, Title = "Unit Test Task" };
            _mockService.Setup(s => s.GetByIdAsync(todoId)).ReturnsAsync(expectedResponse);

            var result = await _controller.GetById(todoId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TodoResponseDTO>(okResult.Value);
            Assert.Equal("Unit Test Task", returnValue.Title);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            var todoId = Guid.NewGuid();
            _mockService.Setup(s => s.GetByIdAsync(todoId)).ReturnsAsync((TodoResponseDTO)null!);

            var result = await _controller.GetById(todoId);

            Assert.IsType<NotFoundResult>(result);
        }

        // ==========================================
        // 3. POST (CREATE) TESTS
        // ==========================================
        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithCreatedItem()
        {
            // Arrange
            var request = new CreateTodoRequestDTO { Title = "New Task" };
            var createdResponse = new TodoResponseDTO { Id = Guid.NewGuid(), Title = "New Task" };

            _mockService.Setup(s => s.CreateAsync(request)).ReturnsAsync(createdResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), createdResult.ActionName);
            Assert.Equal(createdResponse.Id, ((dynamic)createdResult.RouteValues!["id"]));

            var returnValue = Assert.IsType<TodoResponseDTO>(createdResult.Value);
            Assert.Equal("New Task", returnValue.Title);
        }

        // ==========================================
        // 4. PUT (UPDATE) TESTS
        // ==========================================
        [Fact]
        public async Task Update_ReturnsOk_WhenUpdateSucceeds()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var request = new UpdateTodoRequestDTO { Title = "Updated Title" };
            var updatedResponse = new TodoResponseDTO { Id = todoId, Title = "Updated Title" };

            _mockService.Setup(s => s.UpdateAsync(todoId, request)).ReturnsAsync(updatedResponse);

            // Act
            var result = await _controller.Update(todoId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TodoResponseDTO>(okResult.Value);
            Assert.Equal("Updated Title", returnValue.Title);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenItemToUpdateDoesNotExist()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            var request = new UpdateTodoRequestDTO { Title = "Updated Title" };

            _mockService.Setup(s => s.UpdateAsync(todoId, request)).ReturnsAsync((TodoResponseDTO)null!);

            // Act
            var result = await _controller.Update(todoId, request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // ==========================================
        // 5. DELETE TESTS
        // ==========================================
        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleteSucceeds()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(todoId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(todoId);

            // Assert
            Assert.IsType<NoContentResult>(result); // 204 NoContent
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenItemToDeleteDoesNotExist()
        {
            // Arrange
            var todoId = Guid.NewGuid();
            _mockService.Setup(s => s.DeleteAsync(todoId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(todoId);

            // Assert
            Assert.IsType<NotFoundResult>(result); // 404 NotFound
        }
    }
}