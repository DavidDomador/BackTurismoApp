using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Guia.DTOs;

namespace PAR.Application.Features.Guia.Commands;

public record UpdateGuiaCommand(int Id, string GNombre, string GApellidos, string GDni, string GCorreo, bool IsActive, int? UserId)
    : IRequest<Result<GuiaDto>>;
