using MediatR;
using PAR.Application.Common.Models;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Commands;

public class LockUserCommandHandler(IUserRepository userRepository) : IRequestHandler<LockUserCommand, Result>
{
    public async Task<Result> Handle(LockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return Result.Failure("User not found.", 404);

        user.Lock();
        await userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}

public class UnlockUserCommandHandler(IUserRepository userRepository) : IRequestHandler<UnlockUserCommand, Result>
{
    public async Task<Result> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return Result.Failure("User not found.", 404);

        user.Unlock();
        await userRepository.UpdateAsync(user, cancellationToken);
        return Result.Success();
    }
}
