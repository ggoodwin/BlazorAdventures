using Application.Extensions;
using Application.Interfaces.Services;
using Hangfire;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddValidators();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity();
builder.Services.AddJwtAuthentication(builder.Services.GetApplicationSettings(builder.Configuration));
builder.Services.AddSignalR();

builder.Services.AddInfrastructureMappings();
builder.Services.AddRepositories();
builder.Services.AddServerStorage();
builder.Services.AddSerialization();
builder.Services.AddApplicationLayer();
builder.Services.AddCurrentUserService();

builder.Services.AddForwarding(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddConfigs(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddSharedInfrastructure(builder.Configuration);

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddLazyCache();

var app = builder.Build();

Host.CreateDefaultBuilder(args).UseSerilog();

app.Initialize();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    DashboardTitle = "BlazorAdventures Jobs",
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints();

app.MapControllers();

app.Run();