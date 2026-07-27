using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Cliente.DTOs;

namespace PAR.Application.Features.Cliente.Queries;

public record GetClientesQuery : IRequest<Result<IEnumerable<ClienteDto>>>;
