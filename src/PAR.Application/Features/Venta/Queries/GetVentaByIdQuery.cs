using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Venta.DTOs;

namespace PAR.Application.Features.Venta.Queries;

public record GetVentaByIdQuery(int Id) : IRequest<Result<VentaDto>>;
