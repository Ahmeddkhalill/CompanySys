using CompanySys.API;
using CompanySys.API.Services;
using CompanySys.Application;
using CompanySys.Application.Common.Interfaces;
using CompanySys.Infrastructure;
using CompanySys.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    await IdentitySeeder.SeedRolesAsync(roleManager);
    await IdentitySeeder.SeedRolePermissionsAsync(roleManager);
    await IdentitySeeder.SeedOwnerAsync(userManager, app.Configuration);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();