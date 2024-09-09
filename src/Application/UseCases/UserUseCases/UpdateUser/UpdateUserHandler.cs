﻿using Application.UseCases.UserUseCases.Common;
using AutoMapper;
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
            var user = await _userRepository.Get(request.Id, cancellationToken);

            if (user is null) return default;

            user.Name = request.Name;
            user.Email = request.Email;

            _userRepository.Update(user);

            await _unitOfWork.Commit(cancellationToken);

            return _mapper.Map<UserResponse>(user);
        }
    }
}
