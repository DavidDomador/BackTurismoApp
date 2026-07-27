using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Definicion.DTOs;

namespace PAR.Application.Features.Definicion.Queries;

public record GetDefinicionesQuery() : IRequest<Result<IEnumerable<DefinicionDto>>>;
