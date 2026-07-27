using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Cliente.Commands;

public record DeleteClienteCommand(int Id) : IRequest<Result<bool>>;
