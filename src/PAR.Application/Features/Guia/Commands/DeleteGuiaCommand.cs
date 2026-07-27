using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Guia.Commands;

public record DeleteGuiaCommand(int Id) : IRequest<Result<bool>>;
