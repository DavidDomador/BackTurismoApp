using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Ports;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Users.Commands;

public class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordService passwordService) : IRequestHandler<ChangePasswordCommand, Result>
{
    private const int PasswordHistoryLimit = 3;

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            return Result.Failure("User not found.", 404);

        if (!passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            return Result.Failure("Current password is incorrect.", 400);

        // Check last 3 passwords
        var history = await userRepository.GetPasswordHistoryAsync(user.Id, PasswordHistoryLimit, cancellationToken);
        foreach (var pastPassword in history)
        {
            if (passwordService.VerifyPassword(request.NewPassword, pastPassword.PasswordHash))
                return Result.Failure($"New password cannot be the same as the last {PasswordHistoryLimit} passwords.", 400);
        }

        var newHash = passwordService.HashPassword(request.NewPassword);
        user.ChangePassword(newHash);

        await userRepository.UpdateAsync(user, cancellationToken);
        await userRepository.AddPasswordHistoryAsync(new UserPasswordHistory
        {
            UserId = user.Id,
            PasswordHash = newHash,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return Result.Success();
    }
}
