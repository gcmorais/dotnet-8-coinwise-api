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
        private readonly ICreateVerifyHash _createVerifyHash;

        public CreateUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper, ICreateVerifyHash createVerifyHash)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
            _createVerifyHash = createVerifyHash;
        }
        public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            _createVerifyHash.CreateHashPassword(request.Password, out byte[] hashPassword, out byte[] saltPassword);

            var userResponse = new User()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                DateDeleted = user.DateDeleted,
                DateUpdated = user.DateUpdated,
                DateCreated = user.DateCreated,
                HashPassword = hashPassword,
                SaltPassword = saltPassword
            };

            _userRepository.Create(userResponse);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UserResponse>(userResponse);
        }
    }
}
