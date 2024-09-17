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
            // Verifica se o email já está registrado
            var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);
            if (existingUser != null)
            {
                // Tratar o caso onde o usuário já existe (pode lançar uma exceção ou retornar um erro específico)
                throw new InvalidOperationException("User with the same email already exists.");
            }

            // Criação do hash da senha
            _createVerifyHash.CreateHashPassword(request.Password, out byte[] hashPassword, out byte[] saltPassword);

            // Criação da entidade User usando o construtor adequado
            var user = new User(
                request.Name,
                request.Email,
                hashPassword,
                saltPassword
            );

            // Adiciona o usuário ao repositório
            _userRepository.Create(user);

            // Salva as mudanças no banco de dados
            await _unitOfWork.Commit(cancellationToken);

            // Mapeia a entidade User para a resposta DTO
            return _mapper.Map<UserResponse>(user);
        }
    }
}
