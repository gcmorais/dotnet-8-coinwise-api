using Application.UseCases.UserUseCases.Common;
using Application.UseCases.UserUseCases.UpdateUser;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.UserUseCases
{
    public class UserUpdateTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserUpdateTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task ValidUser_UpdateIsCalled_ReturnValidResponseUser()
        {
            // Arrange
            var updateUserRequest = new Fixture().Create<UpdateUserRequest>();

            var existingUser = new User(
                name: "Existing Name",
                email: "existing-email@example.com",
                hashPassword: new byte[5],
                saltPassword: new byte[10]
            )
            {
                Id = updateUserRequest.Id,
                DateCreated = DateTimeOffset.UtcNow // A data deve ser configurada corretamente
            };

            var updatedUser = new User(
                name: updateUserRequest.Name,
                email: updateUserRequest.Email,
                hashPassword: existingUser.HashPassword,
                saltPassword: existingUser.SaltPassword
            )
            {
                Id = updateUserRequest.Id,
                DateCreated = existingUser.DateCreated,
                DateUpdated = DateTimeOffset.UtcNow
            };

            var cancellationToken = new CancellationToken();

            // Setup mocks
            _userRepositoryMock
                .Setup(repo => repo.Get(updateUserRequest.Id, cancellationToken))
                .ReturnsAsync(existingUser);

            _userRepositoryMock
                .Setup(repo => repo.Update(It.IsAny<User>()))
                .Callback<User>(user =>
                {
                    // Verifique se as propriedades do usuário são atualizadas corretamente
                    user.ShouldNotBeNull();
                    user.Id.ShouldBe(updateUserRequest.Id);
                    user.Name.ShouldBe(updateUserRequest.Name);
                    user.Email.ShouldBe(updateUserRequest.Email);
                    user.DateCreated.ShouldBe(existingUser.DateCreated);
                });

            _mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Name = updateUserRequest.Name,
                Email = updateUserRequest.Email
            });

            var userUpdateService = new UpdateUserHandler(_unitOfWorkMock.Object, _userRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await userUpdateService.Handle(updateUserRequest, cancellationToken);

            // Assert
            response.Name.ShouldBe(updateUserRequest.Name);
            response.Email.ShouldBe(updateUserRequest.Email);

            _userRepositoryMock.Verify(repo => repo.Update(It.Is<User>(u =>
                u.Id == updateUserRequest.Id &&
                u.Name == updateUserRequest.Name &&
                u.Email == updateUserRequest.Email)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }
    }
}
