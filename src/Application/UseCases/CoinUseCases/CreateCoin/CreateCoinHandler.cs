using Application.UseCases.CoinUseCases.Common;
using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;
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
            //var user = _userRepository.GetById(request.UserId.Id, cancellationToken);

            //if (user == null) return null;

            var coin = _mapper.Map<Coin>(request);

            //var coinResponse = new Coin()
            //{
            //    Id = coin.Id,
            //    Abbreviation = coin.Abbreviation,
            //    DateCreated = coin.DateCreated,
            //    DateDeleted = coin.DateDeleted,
            //    DateUpdated = coin.DateUpdated,
            //    Name = coin.Name,
            //    Price = coin.Price,
            //    UserId = user.Result,
            //}; 

            _coinRepository.Create(coin);

            await _unitOfWork.Commit(cancellationToken);
            return _mapper.Map<CoinResponse>(coin);
        }
    }
}
