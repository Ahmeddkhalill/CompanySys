namespace CompanySys.Application.Features.Departments.Queries.GetAllDepartments;

public record DepartmentResponse(Guid Id, string Name, string? Description, int TeamsCount);

public record GetDepartmentsQuery() : IRequest<Result<List<DepartmentResponse>>>;