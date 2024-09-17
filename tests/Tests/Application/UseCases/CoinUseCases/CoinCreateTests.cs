using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.CoinUseCases.CreateCoin;
using Application.UseCases.UserUseCases.Common;
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
            var fixture = new Fixture();
            var createCoinRequest = fixture.Create<CreateCoinRequest>();

            var userId = Guid.NewGuid(); // Cria um novo Guid para o usuário

            var user = new User(
                name: "User Name",
                email: "user@example.com",
                hashPassword: new byte[] { 1, 2, 3 },
                saltPassword: new byte[] { 4, 5, 6 }
            );

            // Simulando a atribuição do ID manualmente (precisa ser ajustado conforme o construtor da entidade)
            var userWithId = new User(
                name: "User Name",
                email: "user@example.com",
                hashPassword: new byte[] { 1, 2, 3 },
                saltPassword: new byte[] { 4, 5, 6 }
            );
            typeof(User).GetProperty("Id")?.SetValue(userWithId, userId);

            var coin = new Coin(
                name: createCoinRequest.Name,
                abbreviation: createCoinRequest.Abbreviation,
                price: createCoinRequest.Price,
                user: userWithId
            );

            var coinResponse = new CoinResponse
            {
                Id = coin.Id, // Inclui o Id na resposta
                Abbreviation = createCoinRequest.Abbreviation,
                Name = createCoinRequest.Name,
                Price = createCoinRequest.Price,
                UserData = new UserShortResponse // Ajusta UserShortResponse conforme necessário
                {
                    Id = userWithId.Id,
                    Name = userWithId.Name
                }
            };

            var cancellationToken = new CancellationToken();

            // Setup mocks
            _userRepositoryMock
                .Setup(repo => repo.GetById(createCoinRequest.UserData.Id, cancellationToken))
                .ReturnsAsync(userWithId);

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
                    c.User.ShouldBe(userWithId); // Verifica se o usuário está associado corretamente
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
            response.UserData.ShouldNotBeNull();
            response.UserData.Id.ShouldBe(userWithId.Id);
            response.UserData.Name.ShouldBe(userWithId.Name);

            _userRepositoryMock.Verify(repo => repo.GetById(createCoinRequest.UserData.Id, cancellationToken), Times.Once);
            _coinRepositoryMock.Verify(repo => repo.Create(It.IsAny<Coin>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Commit(cancellationToken), Times.Once);
        }
    }
}
