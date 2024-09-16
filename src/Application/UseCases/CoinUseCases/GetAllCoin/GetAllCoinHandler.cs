using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.CoinUseCases.GetAllCoin
{
    public sealed class GetAllCoinHandler : IRequestHandler<GetAllCoinRequest, List<CoinResponse>>
    {
        private readonly ICoinRepository _coinRepository;
        private readonly IMapper _mapper;
        public GetAllCoinHandler(ICoinRepository coinRepository, IMapper mapper)
        {
            _coinRepository = coinRepository;
            _mapper = mapper;
        }
        public async Task<List<CoinResponse>> Handle(GetAllCoinRequest request, CancellationToken cancellationToken)
        {
            var coins = await _coinRepository.GetAllCoins(cancellationToken);
            return _mapper.Map<List<CoinResponse>>(coins);
        }
    }
}
