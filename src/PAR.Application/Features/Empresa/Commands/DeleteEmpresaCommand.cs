using MediatR;
using PAR.Application.Common.Models;

namespace PAR.Application.Features.Empresa.Commands;

public record DeleteEmpresaCommand(int Id) : IRequest<Result<bool>>;
