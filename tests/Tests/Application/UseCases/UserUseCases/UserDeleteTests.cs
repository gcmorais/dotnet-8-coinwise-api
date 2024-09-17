using Application.UseCases.UserUseCases.Common;
using Application.UseCases.UserUseCases.DeleteUser;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.UserUseCases
{
    public class UserDeleteTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserDeleteTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task UserExists_DeleteIsCalled_ReturnValidResponseUser()
        {
            // Arrange
            var deleteUserRequest = new Fixture().Create<DeleteUserRequest>();

            byte[] hashPassword = new byte[10];
            byte[] saltPassword = new byte[5];

            // Crie o usuário usando o construtor apropriado
            var user = new User(
                name: "User to Delete",
                email: "user-to-delete@example.com",
                hashPassword: hashPassword,
                saltPassword: saltPassword
            );

            var cancellationToken = new CancellationToken();

            // Setup mocks
            _userRepositoryMock
                .Setup(repo => repo.Get(deleteUserRequest.Id, cancellationToken))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(repo => repo.Delete(user))
                .Verifiable();

            _mapperMock.Setup(m => m.Map<UserResponse>(user)).Returns(new UserResponse
            {
                Name = user.Name,
                Email = user.Email
            });

            var userDeleteService = new DeleteUserHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await userDeleteService.Handle(deleteUserRequest, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Name.ShouldBe(user.Name);
            response.Email.ShouldBe(user.Email);

            _userRepositoryMock.Verify(repo => repo.Delete(user), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task UserDoesNotExist_DeleteIsCalled_ReturnDefault()
        {
            // Arrange
            var deleteUserRequest = new Fixture().Create<DeleteUserRequest>();

            var cancellationToken = new CancellationToken();

            _userRepositoryMock
                .Setup(repo => repo.Get(deleteUserRequest.Id, cancellationToken))
                .ReturnsAsync((User)null); // Simulate user not found

            var userDeleteService = new DeleteUserHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await userDeleteService.Handle(deleteUserRequest, cancellationToken);

            // Assert
            response.ShouldBeNull();

            _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }
    }
}
