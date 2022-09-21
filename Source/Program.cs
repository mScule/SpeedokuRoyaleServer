using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// DB Context (Contains Models)
builder.Services.AddDbContext<MariaDbContext>(options => options.UseMySql(
    builder.Configuration.GetConnectionString("MariaDB"),
    new MariaDbServerVersion(new Version(10,7,3))
));

// Example services
builder.Services.AddScoped<TodoService>();

// Speedoku royale server services
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<ItemService>();

// Controllers
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
);

// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
