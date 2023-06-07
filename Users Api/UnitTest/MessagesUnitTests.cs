using Microsoft.AspNetCore.Mvc;
using Moq;
using Users_Server.Controllers;
using Users_Server.Models;
using Users_Server.Repositories;

namespace UnitTest
{
    public class MessagesUnitTests
    {
        [Fact]
        public async Task GetAllMessages_Returns_OkResult_With_Messages()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = 1, FirstName = "John", LastName = "Doe", UserName = "johndoe" },
                new User { Id = 2, FirstName = "Jane", LastName = "Smith", UserName = "janesmith" }
            };

                    var expectedMessages = new List<Message>
            {
                new Message { Id = 1, Content = "Hey", UserId = 1, User = expectedUsers[0] },
                new Message { Id = 2, Content = "Bye", UserId = 1, User = expectedUsers[1] }
            };

            var messageRepositoryMock = new Mock<IMessageRepository>();
            messageRepositoryMock.Setup(repo => repo.GetAllMessages()).ReturnsAsync(expectedMessages);

            var controller = new MessageController(messageRepositoryMock.Object, null!);

            // Act
            var result = await controller.GetAllMessages();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;

            Assert.NotNull(okResult);

            // Assert content
            var actualMessages = (List<Message>)okResult.Value!;

            Assert.Equal(expectedMessages.Count, actualMessages!.Count);

            for (int i = 0; i < expectedMessages.Count; i++)
            {
                Assert.Equal(expectedMessages[i].Id, actualMessages[i].Id);
                Assert.Equal(expectedMessages[i].Content, actualMessages[i].Content);
                Assert.Equal(expectedMessages[i].UserId, actualMessages[i].UserId);
                Assert.Equal(expectedMessages[i].User, actualMessages[i].User);
            }
        }

        [Fact]
        public async Task DeleteAllMessages_ReturnsOkResult()
        {
            // Arrange
            var messageRepositoryMock = new Mock<IMessageRepository>();
            messageRepositoryMock.Setup(repo => repo.DeleteAllMessages()).Returns(Task.CompletedTask);


            var controller = new MessageController(messageRepositoryMock.Object, null!);

            // Act
            var result = await controller.DeleteAllMessages();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal("All Messages are deleted", okResult.Value!.GetType().GetProperty("message")?.GetValue(okResult.Value));

            messageRepositoryMock.Verify(repo => repo.DeleteAllMessages(), Times.Once);
        }


        [Fact]
        public async Task AddMessage_WithValidData_ReturnsOk()
        {
            var userId = 1;
            var messageId = 1;
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe"
            };

            // Arrange
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock.Setup(repo => repo.GetUserById(userId))
                .Returns(Task.FromResult(user));

            var messageDto = new MessageDto
            {
                Content = "Hey"
            };

            var message = new Message
            {
                Id = messageId,
                UserId = userId,
                Content = "Hey",
                User = user
            };

            messageRepositoryMock.Setup(repo => repo.AddMessage(It.IsAny<Message>()))
                .Returns(Task.FromResult(message));

            var controller = new MessageController(messageRepositoryMock.Object, userRepositoryMock.Object!);

            // Act
            var result = await controller.AddMessage(messageDto, messageId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Message>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            messageRepositoryMock.Verify(repo => repo.AddMessage(It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public async Task AddMessage_WithNonExistentUser_ReturnsUnauthorized()
        {
            var userId = 1;
            var messageId = 1;

            // Arrange
            var messageRepositoryMock = new Mock<IMessageRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock.Setup(repo => repo.GetUserById(userId))
                .Returns(Task.FromResult<User>(null!));

            var messageDto = new MessageDto
            {
                Content = "Hey"
            };

            var controller = new MessageController(messageRepositoryMock.Object, userRepositoryMock.Object!);

            // Act
            var result = await controller.AddMessage(messageDto, messageId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Message>>(result);
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
            Assert.Equal("Not Authorized", unauthorizedResult.Value);

            messageRepositoryMock.Verify(repo => repo.AddMessage(It.IsAny<Message>()), Times.Never);
        }

    }
}
