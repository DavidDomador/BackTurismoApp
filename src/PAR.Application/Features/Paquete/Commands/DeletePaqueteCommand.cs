using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Paquete.Commands;

public record DeletePaqueteCommand(int Id) : IRequest<Result<bool>>;
