using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scandium.Data;
using FastEndpoints.Swagger;
using FastEndpoints.Security;
using Scandium.Services.Abstract;
using Scandium.Services;
using Scandium.Extensions.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerDoc();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddAuthenticationJWTBearer(builder.Configuration.GetValue<string>("Jwt:Key"));

builder.Services.AddScoped<IJwtService,JwtService>();
builder.Services.AddScoped<IUserRepo,UserRepo>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(c => c.ConfigureDefaults());
}

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.Run();
