using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scandium.Data;
using FastEndpoints.Swagger;
using FastEndpoints.Security;
using Scandium.Services.Abstract;
using Scandium.Services;
using Scandium.Extensions.ServiceExtensions;
using Scandium.Services.Concreate;
using Scandium.Data.Abstract;
using Scandium.Data.Concreate;

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
builder.Services.AddTransient<IHttpContextService,HttpContextService>();
builder.Services.AddScoped<IUserRepo,UserRepo>();
builder.Services.AddScoped<IMessageRepository,MessageRepository>();

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
