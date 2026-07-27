using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Guia.DTOs;

namespace PAR.Application.Features.Guia.Commands;

public record CreateGuiaCommand(string GNombre, string GApellidos, string GDni, string GCorreo, int? UserId)
    : IRequest<Result<GuiaDto>>;
