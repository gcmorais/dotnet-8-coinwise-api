using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.CoinUseCases.CreateCoin
{
    public class CreateCoinHandler : IRequestHandler<CreateCoinRequest, CoinResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoinRepository _coinRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateCoinHandler(IUnitOfWork unitOfWork, ICoinRepository coinRepository, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _coinRepository = coinRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<CoinResponse> Handle(CreateCoinRequest request, CancellationToken cancellationToken)
        {
            // Verifica se o usuário existe
            var user = await _userRepository.GetById(request.UserData.Id, cancellationToken);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            // Mapeia a solicitação para a entidade Coin
            var coin = _mapper.Map<Coin>(request);

            // Associa o usuário à moeda
            coin.AssignUser(user);

            // Adiciona a moeda ao repositório
            _coinRepository.Create(coin);

            // Salva as alterações no banco de dados
            await _unitOfWork.Commit(cancellationToken);

            // Mapeia a entidade Coin para a resposta
            return _mapper.Map<CoinResponse>(coin);
        }
    }
}
