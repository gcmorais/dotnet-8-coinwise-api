using Application.UseCases.UserUseCases.Common;
using Application.UseCases.UserUseCases.UpdateUser;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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

            string dateString = @"20/05/2012";

            var existingUser = new User
            {
                Id = updateUserRequest.Id,
                Name = "Existing Name",
                Email = "existing-email@example.com",
                DateCreated = Convert.ToDateTime(dateString)
            };

            var updatedUser = new User
            {
                Id = updateUserRequest.Id,
                Name = updateUserRequest.Name,
                Email = updateUserRequest.Email,
                DateCreated = existingUser.DateCreated,
                DateUpdated = DateTime.UtcNow
            };

            var cancellationToken = new CancellationToken();

            // Setup mocks
            _userRepositoryMock
                .Setup(repo => repo.Get(updateUserRequest.Id, cancellationToken))
                .ReturnsAsync(existingUser);

            _userRepositoryMock
                .Setup(repo => repo.Update(It.Is<User>(user =>
                    user.Id == updateUserRequest.Id &&
                    user.Name == updateUserRequest.Name &&
                    user.Email == updateUserRequest.Email &&
                    user.DateCreated == existingUser.DateCreated)))
                .Callback<User>(user =>
                {
                    // Verify user properties in the callback
                    user.ShouldNotBeNull();
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
