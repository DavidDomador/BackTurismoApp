using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;

namespace PAR.Application.Features.Paquete.Queries;

public record GetPaqueteByIdQuery(int Id) : IRequest<Result<PaqueteDto>>;
