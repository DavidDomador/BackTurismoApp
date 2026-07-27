using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Transporte.DTOs;

namespace PAR.Application.Features.Transporte.Queries;

public record GetTransporteByIdQuery(int Id) : IRequest<Result<TransporteDto>>;
