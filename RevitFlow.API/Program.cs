using Microsoft.Extensions.DependencyInjection;
using RevitFlow.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DbConnectionFactory>();
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
    throw;
}

app.UseSwagger();
app.UseSwaggerUI();
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
        return Results.Problem(ex.Message);
    }
});

app.Run();
