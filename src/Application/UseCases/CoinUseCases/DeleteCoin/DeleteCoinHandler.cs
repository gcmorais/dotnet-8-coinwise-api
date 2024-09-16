using Application.UseCases.CoinUseCases.Common;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.CoinUseCases.DeleteCoin
{
    public class DeleteCoinHandler : IRequestHandler<DeleteCoinRequest, CoinResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICoinRepository _coinRepository;
        private readonly IMapper _mapper;

        public DeleteCoinHandler(IUnitOfWork unitOfWork, ICoinRepository coinRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _coinRepository = coinRepository;
            _mapper = mapper;
        }
        public async Task<CoinResponse> Handle(DeleteCoinRequest request, CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.Get(request.Id, cancellationToken);

            if (coin == null) return default;

            _coinRepository.Delete(coin);
            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<CoinResponse>(coin);
        }
    }
}
