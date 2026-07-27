using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Cliente.DTOs;

namespace PAR.Application.Features.Cliente.Commands;

public record UpdateClienteCommand(
    int Id,
    string CNombres, string CApellidos, string CDni,
    string? CCorreo, int? CEdad, string? CDireccion, int? UserId)
    : IRequest<Result<ClienteDto>>;
