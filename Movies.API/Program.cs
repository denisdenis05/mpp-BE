using Movies.API.Hubs;
using Movies.API.Hubs.MovieStats;
using Movies.Business;
using Movies.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHostedService<StatsBroadcaster>(); 
builder.Services.AddSignalR();

builder.Services.AddSingleton(new DbContext());

var allowedOrigins = new[] {
    "http://localhost:3000",
    "http://localhost:3001",
    "http://localhost:3002"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});



var app = builder.Build();

app.UseCors("AllowFrontend");
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // This makes Swagger UI available at the root URL
});

app.UseAuthorization();
app.MapHub<MovieStatsHub>("/movieStatsHub");
app.MapControllers();


app.Run();