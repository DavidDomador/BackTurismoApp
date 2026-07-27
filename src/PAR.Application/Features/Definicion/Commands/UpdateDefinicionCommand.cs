using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;

namespace PAR.Application.Features.Definicion.Commands;

public record UpdateDefinicionCommand(
    int Id,
    string DNombre,
    string? DDescripcion,
    bool Estado,
    int? UserId) : IRequest<Result<DefinicionDto>>;
