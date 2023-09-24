using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scandium.Data;
using FastEndpoints.Swagger;
using Scandium.Services.Abstract;
using Scandium.Services;
using Scandium.Extensions.ServiceExtensions;
using Scandium.Middlewares;
using Scandium.Actions;
using Scandium.Services.Concreate;
using Scandium.Data.Abstract;
using Scandium.Data.Concreate;
using Scandium.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerDoc();
builder.Services.AddSignalR();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<AppDbContext>(opt => opt
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddCustomJwtAuthentication(builder.Configuration);


builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddTransient<IHttpContextService, HttpContextService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFriendshipRequestRepository, FriendshipRequestRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(c => c.ConfigureDefaults());
}
app.UseRouting();
app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.UseMiddleware<ExceptionHandlingMiddleware>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<MessageHub>("/hubs/messageHub");
app.MapHub<FriendshipRequestHub>("/hubs/friendshipRequestHub");

app.UseFastEndpoints(FastEndpointsAction.GetConfigActions);
app.Run();
