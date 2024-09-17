using Application.UseCases.UserUseCases.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.UserUseCases.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            // Recupera o usuário do repositório
            var user = await _userRepository.Get(request.Id, cancellationToken);

            if (user == null)
            {
                // Retorna um erro ou uma resposta padrão quando o usuário não for encontrado
                return default;
            }

            // Atualiza as propriedades do usuário usando os métodos apropriados
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                user.UpdateName(request.Name);
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                // Assumindo que a validação do email é feita em outro lugar, por exemplo, no repositório ou serviço
                user.UpdateEmail(request.Email);
            }

            // Atualiza o usuário no repositório
            _userRepository.Update(user);

            // Salva as mudanças no banco de dados
            await _unitOfWork.Commit(cancellationToken);

            // Mapeia a entidade User para a resposta DTO
            return _mapper.Map<UserResponse>(user);
        }
    }
}
