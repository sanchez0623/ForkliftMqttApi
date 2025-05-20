using ForkliftMqtt.Application;
using ForkliftMqtt.Infrastructure.Extensions;
using ForkliftMqtt.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// ���� Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ���Ӧ�ò����
builder.Services.AddApplicationServices();

//��ӻ�����ʩ�����
builder.Services.AddInfrastructureServices(builder.Configuration);
//builder.Services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();

// ���AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // ���ڿ����������Զ�Ӧ��Ǩ��
    await DbInitializer.InitializeDatabaseAsync(app.Services);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
