using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Salida.Commands;

public record DeleteSalidaCommand(int ICodSalida) : IRequest<Result<bool>>;
