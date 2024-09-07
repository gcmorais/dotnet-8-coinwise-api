using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.UserUseCases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            _userRepository.Create(user);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UserResponse>(user);
        }
    }
}
