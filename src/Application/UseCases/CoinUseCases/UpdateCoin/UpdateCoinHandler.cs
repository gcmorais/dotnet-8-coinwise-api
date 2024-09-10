using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.UserUseCases.Common;
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

        public UpdateCoinHandler(IUnitOfWork unitOfWork, ICoinRepository coinRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _coinRepository = coinRepository;
            _mapper = mapper;
        }
        public async Task<CoinResponse> Handle(UpdateCoinRequest request, CancellationToken cancellationToken)
        {
            var coin = await _coinRepository.Get(request.Id, cancellationToken);

            if (coin is null) return default;

            coin.Name = request.Name;
            coin.Abbreviation = request.Abbreviation;
            coin.Price = request.Price;

            _coinRepository.Update(coin);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<CoinResponse>(coin);
        }
    }
}
