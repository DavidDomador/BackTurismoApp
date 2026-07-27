using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Transporte.Commands;

public record DeleteTransporteCommand(int Id) : IRequest<Result<bool>>;
