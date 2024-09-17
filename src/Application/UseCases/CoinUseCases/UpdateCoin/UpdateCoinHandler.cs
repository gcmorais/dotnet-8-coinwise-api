using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.CoinUseCases.UpdateCoin
{
    public class UpdateCoinHandler : IRequestHandler<UpdateCoinRequest, CoinResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoinRepository _coinRepository;
        private readonly IMapper _mapper;

        public UpdateCoinHandler(
            IUnitOfWork unitOfWork,
            ICoinRepository coinRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _coinRepository = coinRepository;
            _mapper = mapper;
        }

        public async Task<CoinResponse> Handle(UpdateCoinRequest request, CancellationToken cancellationToken)
        {
            // Obtém a moeda do repositório
            var coin = await _coinRepository.Get(request.Id, cancellationToken);

            if (coin == null)
            {
                // Lança uma exceção com uma mensagem mais informativa
                throw new InvalidOperationException($"Coin with ID {request.Id} not found.");
            }

            // Atualiza as propriedades da moeda usando métodos da entidade
            coin.UpdateName(request.Name);
            coin.UpdateAbbreviation(request.Abbreviation);
            coin.UpdatePrice(request.Price);

            // Atualiza a moeda no repositório
            _coinRepository.Update(coin);

            // Salva as alterações no banco de dados
            await _unitOfWork.Commit(cancellationToken);

            // Mapeia a entidade Coin para a resposta
            return _mapper.Map<CoinResponse>(coin);
        }
    }
}
