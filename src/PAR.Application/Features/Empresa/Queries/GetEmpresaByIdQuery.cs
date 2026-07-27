using MediatR;
using PAR.Application.Common.Models;
using PAR.Application.Features.Empresa.DTOs;

namespace PAR.Application.Features.Empresa.Queries;

public record GetEmpresaByIdQuery(int Id) : IRequest<Result<EmpresaDto>>;
