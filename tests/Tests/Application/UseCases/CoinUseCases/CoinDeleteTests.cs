using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.CoinUseCases.DeleteCoin;
using Application.UseCases.UserUseCases.DeleteUser;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.CoinUseCases
{
    public class CoinDeleteTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICoinRepository> _coinRepositoryMock;

        public CoinDeleteTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _coinRepositoryMock = new Mock<ICoinRepository>();
        }

        [Fact]
        public async Task CoinExists_DeleteIsCalled_ReturnValidCoinResponse()
        {
            // Arrange
            var deleteCoinRequest = new Fixture().Create<DeleteCoinRequest>();

            var coin = new Coin
            {
                Id = deleteCoinRequest.Id,
                Abbreviation = "DEL",
                Price = 800,
                DateCreated = DateTime.UtcNow
            };

            var cancellationToken = new CancellationToken();

            // Setup mocks
            _coinRepositoryMock
                .Setup(repo => repo.Get(deleteCoinRequest.Id, cancellationToken))
                .ReturnsAsync(coin);

            _coinRepositoryMock
                .Setup(repo => repo.Delete(coin))
                .Verifiable();

            _mapperMock.Setup(m => m.Map<CoinResponse>(coin)).Returns(new CoinResponse
            {
                Name = coin.Name,
                Abbreviation = coin.Abbreviation,
                Price = coin.Price
            });

            var coinDeleteData = new DeleteCoinHandler(_unitOfWorkMock.Object, _coinRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await coinDeleteData.Handle(deleteCoinRequest, cancellationToken);

            // Assert
            response.ShouldNotBeNull();
            response.Name.ShouldBe(coin.Name);
            response.Abbreviation.ShouldBe(coin.Abbreviation);
            response.Price.ShouldBe(coin.Price);

            _coinRepositoryMock.Verify(repo => repo.Delete(coin), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task CoinDoesNotExist_DeleteIsCalled_ReturnDefault()
        {
            // Arrange
            var deleteCoinRequest = new Fixture().Create<DeleteCoinRequest>();

            var cancellationToken = new CancellationToken();

            _coinRepositoryMock
                .Setup(repo => repo.Get(deleteCoinRequest.Id, cancellationToken))
                .ReturnsAsync((Coin)null);

            var coinDeleteData = new DeleteCoinHandler(_unitOfWorkMock.Object, _coinRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await coinDeleteData.Handle(deleteCoinRequest, cancellationToken);

            // Assert
            response.ShouldBeNull();

            _coinRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Coin>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Never);
        }
    }
}
