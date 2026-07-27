using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Auth.DTOs;
using PAR.Application.Ports;
using PAR.Domain.Entities;
using PAR.Domain.Ports;

namespace PAR.Application.Features.Auth.Commands;

public class LoginCommandHandler(
    IUserRepository userRepository,
    ILoginAttemptRepository loginAttemptRepository,
    IRoleRepository roleRepository,
    IPasswordService passwordService,
    IJwtService jwtService) : IRequestHandler<LoginCommand, Result<LoginResponseDto>>
{
    private const int MaxIpAttempts = 100; // 10
    private const int MaxUserAttempts = 500; // 5
    private const int WindowMinutes = 15; // 5

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Brute force check by IP
        var ipAttempts = await loginAttemptRepository.GetRecentFailedAttemptsAsync(request.IpAddress, WindowMinutes, cancellationToken);
        if (ipAttempts >= MaxIpAttempts)
        {
            await RecordAttempt(null, request.Username, request.IpAddress, false, "IP blocked due to brute force", cancellationToken);
            return Result<LoginResponseDto>.Failure("Too many failed attempts from this IP. Please try again later.", 429);
        }

        // Brute force check by username
        var userAttempts = await loginAttemptRepository.GetRecentFailedAttemptsByUserAsync(request.Username, WindowMinutes, cancellationToken);
        if (userAttempts >= MaxUserAttempts)
        {
            await RecordAttempt(null, request.Username, request.IpAddress, false, "User account brute force detected", cancellationToken);
            return Result<LoginResponseDto>.Failure("Too many failed attempts for this account. Please try again later.", 429);
        }

        var user = await userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (user == null)
        {
            await RecordAttempt(null, request.Username, request.IpAddress, false, "User not found", cancellationToken);
            return Result<LoginResponseDto>.Failure("Invalid credentials.", 401);
        }

        if (user.IsLocked)
        {
            await RecordAttempt(user.Id, request.Username, request.IpAddress, false, "Account locked", cancellationToken);
            return Result<LoginResponseDto>.Failure("Account is locked. Please contact an administrator.", 403);
        }

        if (!user.IsActive)
        {
            await RecordAttempt(user.Id, request.Username, request.IpAddress, false, "Account inactive", cancellationToken);
            return Result<LoginResponseDto>.Failure("Account is inactive.", 403);
        }

        if (!passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            await RecordAttempt(user.Id, request.Username, request.IpAddress, false, "Invalid password", cancellationToken);
            return Result<LoginResponseDto>.Failure("Invalid credentials.", 401);
        }

        // Clear failed attempts on success
        await loginAttemptRepository.ClearAttemptsAsync(request.IpAddress, cancellationToken);
        await RecordAttempt(user.Id, request.Username, request.IpAddress, true, null, cancellationToken);

        var rolePermNames   = await roleRepository.GetUserPermissionsAsync(user.Id, cancellationToken);
        var directPermIds   = await userRepository.GetUserDirectPermissionIdsAsync(user.Id, cancellationToken);
        IEnumerable<string> allPermissions;
        if (directPermIds.Any())
        {
            var allPerms        = await roleRepository.GetAllPermissionsAsync(cancellationToken);
            var directPermNames = allPerms.Where(p => directPermIds.Contains(p.Id)).Select(p => p.Name);
            allPermissions      = rolePermNames.Union(directPermNames).Distinct();
        }
        else
        {
            allPermissions = rolePermNames;
        }

        var token        = jwtService.GenerateToken(user, allPermissions);
        var refreshToken = jwtService.GenerateRefreshToken();

        return Result<LoginResponseDto>.Success(new LoginResponseDto
        {
            Token        = token,
            RefreshToken = refreshToken,
            Username     = user.Username,
            Email        = user.Email,
            Permissions  = allPermissions.ToList()
        });
    }

    private async Task RecordAttempt(int? userId, string username, string ip, bool success, string? reason, CancellationToken ct)
    {
        await loginAttemptRepository.AddAttemptAsync(new LoginAttempt
        {
            UserId = userId,
            Username = username,
            IpAddress = ip,
            Success = success,
            FailureReason = reason,
            AttemptedAt = DateTime.UtcNow
        }, ct);
    }
}
