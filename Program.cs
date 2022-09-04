using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scandium.Data;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDoc();
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<IUserRepo,UserRepo>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(c => c.ConfigureDefaults());
}
app.UseFastEndpoints();
app.Run();
