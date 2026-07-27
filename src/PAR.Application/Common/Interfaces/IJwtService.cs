using PAR.Domain.Entities;

namespace PAR.Application.Ports;

public interface IJwtService
{
    string GenerateToken(User user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    int? ValidateTokenAndGetUserId(string token);
}
