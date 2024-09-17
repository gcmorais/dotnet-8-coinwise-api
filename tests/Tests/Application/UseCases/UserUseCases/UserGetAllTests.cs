using Application.UseCases.UserUseCases.Common;
using Application.UseCases.UserUseCases.GetAllUser;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.UserUseCases
{
    public class UserGetAllTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public UserGetAllTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllUsers_RepositoryReturnsUsers_ReturnsMappedUserResponses()
        {
            // Arrange
            var users = new List<User>
        {
            new User(
                name: "User1",
                email: "user1@example.com",
                hashPassword: new byte[10],
                saltPassword: new byte[5]
            ),

            new User(
                name: "User2",
                email: "user2@example.com",
                hashPassword: new byte[11],
                saltPassword: new byte[6]
            )
        };

            var userResponses = new List<UserResponse>
            {
                new UserResponse { Name = "User1", Email = "user1@example.com" },
                new UserResponse { Name = "User2", Email = "user2@example.com" }
            };

            var cancellationToken = new CancellationToken();

            // Configure o mock do repositório para retornar a lista de usuários
            _userRepositoryMock
                .Setup(repo => repo.GetAll(cancellationToken))
                .ReturnsAsync(users);

            // Configure o mock do mapper para retornar a lista de UserResponse
            _mapperMock
                .Setup(m => m.Map<List<UserResponse>>(users))
                .Returns(userResponses);

            var getAllUserHandler = new GetAllUserHandler(_userRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await getAllUserHandler.Handle(new GetAllUserRequest(), cancellationToken);

            // Assert
            response.ShouldBe(userResponses); // Verifica se a resposta está correta

            _userRepositoryMock.Verify(repo => repo.GetAll(cancellationToken), Times.Once); // Verifica se GetAll foi chamado uma vez
            _mapperMock.Verify(m => m.Map<List<UserResponse>>(users), Times.Once); // Verifica se o mapeamento foi chamado uma vez
        }
    }
}
