using EventGoAPI.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventGoAPI.Application.Features.Command.User.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommandRequest, DeleteUserCommandResponse>
    {
        private IUserWriteRepository _userWriteRepository;
        private IUserReadRepository _userReadRepository;
        public DeleteUserCommandHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
        }
        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetEntityByIdAsync(request.Id);

            if (user == null)
            {
                return new DeleteUserCommandResponse()
                {
                    Success = false,
                    ResponseType = Enums.ResponseType.NotFound,
                    Message = "User Cannot Be Found!"
                };
            }
            await _userWriteRepository.DeleteAsync(request.Id);
            await _userWriteRepository.SaveChangesAsync();

            return new DeleteUserCommandResponse()
            {
                Success = true,
                ResponseType = Enums.ResponseType.Success,
                Message = "Successfully Deleted"
            };
        }
    }
}
