using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Empresa.DTOs;

namespace PAR.Application.Features.Empresa.Commands;

public record CreateEmpresaCommand(
    string ERazonSocial, string? ERUC,
    string? EDireccion, string? ETelefono, string? ECorreo,
    int? UserId)
    : IRequest<Result<EmpresaDto>>;
