using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.UserUseCases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICreateVerifyHash _createVerifyHash;

        public CreateUserHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICreateVerifyHash createVerifyHash)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
            _createVerifyHash = createVerifyHash;
        }

        public async Task<UserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            // Mapeamento do DTO para a entidade User
            var user = _mapper.Map<User>(request);

            // Criação do hash da senha
            _createVerifyHash.CreateHashPassword(request.Password, out byte[] hashPassword, out byte[] saltPassword);

            // Configuração da resposta do usuário
            var userResponse = new User
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

            // Persistência do usuário no repositório
            _userRepository.Create(userResponse);

            // Commit das mudanças
            await _unitOfWork.Commit(cancellationToken);

            // Mapeamento da entidade User para a resposta DTO
            return _mapper.Map<UserResponse>(userResponse);
        }
    }
}
