using CanvasRoomDesign.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using CanvasRoomDesign.UIServices;
using Microsoft.EntityFrameworkCore;
using CanvasRoomDesign.Model;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSweetAlert2();
builder.Services.AddScoped<IClientUIService, ClientUIService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

var connectionString=builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CanvasDbcontext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
