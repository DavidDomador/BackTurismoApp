using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Salida.DTOs;

namespace PAR.Application.Features.Salida.Queries;

public record GetSalidaByIdQuery(int Id) : IRequest<Result<SalidaDto>>;
