using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Paquete.DTOs;

namespace PAR.Application.Features.Paquete.Commands;

/// <summary>Actualiza (o limpia) el PDF adjunto de un paquete.</summary>
public record UpdatePaquetePdfAdjuntoCommand(int Id, string? PdfAdjunto)
    : IRequest<Result<PaqueteDto>>;
