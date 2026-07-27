using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Paquete.Queries;

public record GetPaquetePdfQuery(int ICodPaquete) : IRequest<Result<byte[]>>;
