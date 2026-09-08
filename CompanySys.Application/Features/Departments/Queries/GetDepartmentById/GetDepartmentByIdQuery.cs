namespace CompanySys.Application.Features.Departments.Queries.GetDepartmentById;

public record TeamDto(Guid Id, string Name);

public record DepartmentDetailsResponse(Guid Id, string Name, string? Description, List<TeamDto> Teams);

public record GetDepartmentByIdQuery(Guid Id) : IRequest<Result<DepartmentDetailsResponse>>;