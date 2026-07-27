using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Guia.DTOs;

namespace PAR.Application.Features.Guia.Queries;

public record GetGuiasQuery : IRequest<Result<IEnumerable<GuiaDto>>>;
