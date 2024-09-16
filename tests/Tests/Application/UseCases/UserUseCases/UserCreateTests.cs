using Application.UseCases.UserUseCases.Common;
using Application.UseCases.UserUseCases.CreateUser;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.UserUseCases
{
    public class UserCreateTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICreateVerifyHash> _createVerifyHashMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;


        public UserCreateTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _createVerifyHashMock = new Mock<ICreateVerifyHash>();
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task ValidUser_CreateIsCalled_ReturnValidResponseUser()
        {
            // Arrange
            var createUserRequest = new Fixture().Create<CreateUserRequest>();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = createUserRequest.Name,
                Email = createUserRequest.Email
            };

            _mapperMock.Setup(m => m.Map<User>(createUserRequest)).Returns(user);
            _mapperMock.Setup(m => m.Map<UserResponse>(It.IsAny<User>())).Returns(new UserResponse
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email
            });

            var userCreateService = new CreateUserHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object, _createVerifyHashMock.Object);

            var cancellationToken = new CancellationToken();

            // Act
            var response = await userCreateService.Handle(createUserRequest, cancellationToken);

            // Assert
            response.Name.ShouldBe(createUserRequest.Name);
            response.Email.ShouldBe(createUserRequest.Email);

            _userRepositoryMock.Verify(er => er.Create(It.Is<User>(u => u.Email == createUserRequest.Email)), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }
    }
}