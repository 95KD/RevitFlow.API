using Microsoft.Extensions.DependencyInjection;
using RevitFlow.API.Data;
using RevitFlow.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IObjectPointRepository, ObjectPointRepository>();
builder.Services.AddScoped<IObjectLineRepository, ObjectLineRepository>();
builder.Services.AddScoped<ILineStyleRepository, LineStyleRepository>();
builder.Services.AddScoped<IObjectParameterRepository, ObjectParameterRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var dbFactory = app.Services.GetRequiredService<DbConnectionFactory>();
try
{
    using var startupConn = dbFactory.CreateConnection();
    await startupConn.OpenAsync();
    Console.WriteLine("[RevitFlow] MariaDB 연결됨 (디버그/실행 시작 시 확인). Test");
}
catch (Exception ex)
{
    Console.WriteLine($"[RevitFlow] MariaDB 연결 실패: {ex.Message}");
    // DB가 무료 플랜 등으로 슬립 상태여도 API 서버는 계속 실행되도록 함.
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Ok("RevitFlow API is running successfully!"));

app.MapControllers();

app.MapGet("/health/db", async (DbConnectionFactory factory) =>
{
    try
    {
        using var conn = factory.CreateConnection();
        await conn.OpenAsync();
        return Results.Ok("DB connected");
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "DB unavailable",
            detail: $"DB가 슬립 상태이거나 연결할 수 없습니다. 잠시 후 다시 시도해 주세요. ({ex.Message})",
            statusCode: StatusCodes.Status503ServiceUnavailable
        );
    }
});

app.Run();
