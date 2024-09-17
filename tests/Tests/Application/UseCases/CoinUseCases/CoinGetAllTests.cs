using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.CoinUseCases.GetAllCoin;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Application.UseCases.CoinUseCases
{
    public class CoinGetAllTests
    {
        private readonly Mock<ICoinRepository> _coinRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public CoinGetAllTests()
        {
            _coinRepositoryMock = new Mock<ICoinRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllCoins_RepositoryReturnsCoins_ReturnsCoinResponses()
        {
            var user = new User("Sample User", "user@example.com", new byte[0], new byte[0]);

            // Arrange
            var coins = new List<Coin>
            {
                new Coin(name: "Coin1", abbreviation: "CON1", price: 100, user: user),
                new Coin(name: "Coin1", abbreviation: "CON1", price: 100, user: user)
            };

            var coinResponses = new List<CoinResponse>
            {
                new CoinResponse { Name = "Coin1", Abbreviation = "CON1", Price = 100 },
                new CoinResponse { Name = "Coin2", Abbreviation = "CON2", Price = 200 }
            };

            var cancellationToken = new CancellationToken();

            // Configure o mock do repositório para retornar a lista de moedas
            _coinRepositoryMock
                .Setup(repo => repo.GetAllCoins(cancellationToken))
                .ReturnsAsync(coins);

            // Configure o mock do mapper para retornar a lista de CoinResponse
            _mapperMock
                .Setup(m => m.Map<List<CoinResponse>>(coins))
                .Returns(coinResponses);

            var getAllCoinHandler = new GetAllCoinHandler(_coinRepositoryMock.Object, _mapperMock.Object);

            // Act
            var response = await getAllCoinHandler.Handle(new GetAllCoinRequest(), cancellationToken);

            // Assert
            response.ShouldBe(coinResponses); // Verifica se a resposta está correta

            _coinRepositoryMock.Verify(repo => repo.GetAllCoins(cancellationToken), Times.Once); // Verifica se GetAllCoins foi chamado uma vez
            _mapperMock.Verify(m => m.Map<List<CoinResponse>>(coins), Times.Once); // Verifica se o mapeamento foi chamado uma vez
        }
    }
}
