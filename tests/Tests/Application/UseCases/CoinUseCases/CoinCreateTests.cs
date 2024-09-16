using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.CoinUseCases.CreateCoin;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.CoinUseCases
{
    public class CoinCreateTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICoinRepository> _coinRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public CoinCreateTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _coinRepositoryMock = new Mock<ICoinRepository>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task ValidCoin_CreateIsCalled_ReturnValidResponseCoin()
        {
            // Arrange
            var createCoinRequest = new Fixture().Create<CreateCoinRequest>();

            var user = new User
            {
                Id = createCoinRequest.UserData.Id,
                Name = "User Name",
                Email = "user@example.com"
            };

            var coin = new Coin
            {
                Id = Guid.NewGuid(),
                Abbreviation = createCoinRequest.Abbreviation,
                Name = createCoinRequest.Name,
                Price = createCoinRequest.Price,
                DateCreated = DateTime.UtcNow
            };

            var coinResponse = new CoinResponse
            {
                Abbreviation = createCoinRequest.Abbreviation,
                Name = createCoinRequest.Name,
                Price = createCoinRequest.Price
            };

            var cancellationToken = new CancellationToken();

            // Setup mocks
            _userRepositoryMock
                .Setup(repo => repo.GetById(createCoinRequest.UserData.Id, cancellationToken))
                .ReturnsAsync(user);

            _mapperMock
                .Setup(m => m.Map<Coin>(createCoinRequest))
                .Returns(coin);

            _mapperMock
                .Setup(m => m.Map<CoinResponse>(It.IsAny<Coin>()))
                .Returns(coinResponse);

            _coinRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<Coin>()))
                .Callback<Coin>(c =>
                {
                    c.Abbreviation.ShouldBe(coin.Abbreviation);
                    c.Name.ShouldBe(coin.Name);
                    c.Price.ShouldBe(coin.Price);
                });

            var createCoinHandler = new CreateCoinHandler(
                _unitOfWorkMock.Object,
                _coinRepositoryMock.Object,
                _mapperMock.Object,
                _userRepositoryMock.Object
            );

            // Act
            var response = await createCoinHandler.Handle(createCoinRequest, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Abbreviation.ShouldBe(createCoinRequest.Abbreviation);
            response.Name.ShouldBe(createCoinRequest.Name);
            response.Price.ShouldBe(createCoinRequest.Price);

            _userRepositoryMock.Verify(repo => repo.GetById(createCoinRequest.UserData.Id, cancellationToken), Times.Once);
            _coinRepositoryMock.Verify(repo => repo.Create(It.IsAny<Coin>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }
    }
}
