using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;

namespace PAR.Application.Features.Definicion.Commands;

public record UpdateDefinicionDetalleCommand(
    int Id,
    string DdValue,
    string? DdAbreviacion,
    string? DdDescripcion,
    bool Estado,
    int? UserId) : IRequest<Result<DefinicionDetalleDto>>;
