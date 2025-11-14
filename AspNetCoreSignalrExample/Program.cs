using AspNetCoreSignalrExample.Hubs;
using AspNetCoreSignalrExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add SignalR
builder.Services.AddSignalR();

// Register the background ping service
builder.Services.AddHostedService<PingSignalrService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Enable static files
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map the hub endpoint
app.MapHub<PingHub>("/pingHub");

app.Run();
