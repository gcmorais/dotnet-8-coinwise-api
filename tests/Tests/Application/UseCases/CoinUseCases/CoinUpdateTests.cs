using System;
using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.CoinUseCases.UpdateCoin;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.CoinUseCases
{
    public class CoinUpdateTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICoinRepository> _coinRepositoryMock;

        public CoinUpdateTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _coinRepositoryMock = new Mock<ICoinRepository>();
        }

        [Fact]
        public async Task ValidCoin_UpdateIsCalled_ReturnValidCoinResponse()
        {
            // Arrange
            var updateCoinRequest = new Fixture().Create<UpdateCoinRequest>();

            var user = new User("Sample User", "user@example.com", new byte[0], new byte[0]);

            var existingCoin = new Coin(
                name: "Testing Coin",
                abbreviation: "TST",
                price: updateCoinRequest.Price,
                user: user
            )
            {
                Id = updateCoinRequest.Id
            };

            var updateCoin = new Coin(
                name: updateCoinRequest.Name,
                abbreviation: updateCoinRequest.Abbreviation,
                price: updateCoinRequest.Price,
                user: user
            )
            {
                Id = updateCoinRequest.Id
            };

            var cancellationToken = new CancellationToken();

            // Mocks
            _coinRepositoryMock
                .Setup(repo => repo.Get(updateCoinRequest.Id, cancellationToken))
                .ReturnsAsync(existingCoin);

            _coinRepositoryMock
               .Setup(repo => repo.Update(It.Is<Coin>(coin =>
                   coin.Id == updateCoinRequest.Id &&
                   coin.Name == updateCoinRequest.Name &&
                   coin.Abbreviation == updateCoinRequest.Abbreviation &&
                   coin.Price == updateCoinRequest.Price &&
                   coin.DateCreated == existingCoin.DateCreated)))
               .Callback<Coin>(coin =>
               {
                   coin.ShouldNotBeNull();
                   coin.Name.ShouldBe(updateCoinRequest.Name);
                   coin.Abbreviation.ShouldBe(updateCoinRequest.Abbreviation);
                   coin.Price.ShouldBe(updateCoinRequest.Price);
                   coin.DateCreated.ShouldBe(existingCoin.DateCreated);
               });

            _mapperMock.Setup(m => m.Map<CoinResponse>(It.IsAny<Coin>())).Returns(new CoinResponse
            {
                Name = updateCoinRequest.Name,
                Abbreviation = updateCoinRequest.Abbreviation,
                Price = updateCoinRequest.Price,
            });

            var coinUpdateData = new UpdateCoinHandler(_unitOfWorkMock.Object, _coinRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await coinUpdateData.Handle(updateCoinRequest, cancellationToken);

            // Assert
            response.Name.ShouldBe(updateCoinRequest.Name);
            response.Abbreviation.ShouldBe(updateCoinRequest.Abbreviation);
            response.Price.ShouldBe(updateCoinRequest.Price);

            _coinRepositoryMock.Verify(repo => repo.Update(It.Is<Coin>(c =>
                c.Id == updateCoinRequest.Id &&
                c.Name == updateCoinRequest.Name &&
                c.Abbreviation == updateCoinRequest.Abbreviation &&
                c.Price == updateCoinRequest.Price)), Times.Once);

            _unitOfWorkMock.Verify(c => c.Commit(cancellationToken), Times.Once);
        }
    }
}
