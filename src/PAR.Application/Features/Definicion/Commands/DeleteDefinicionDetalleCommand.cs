using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Definicion.Commands;

public record DeleteDefinicionDetalleCommand(int Id) : IRequest<Result<bool>>;
