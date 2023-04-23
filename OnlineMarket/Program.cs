using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using StudentUptBackend.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var connectionString = builder.Configuration.GetConnectionString("mySql"); // use "mySql" for deploy
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySQL(connectionString)); // "UseMySQL" for deploy

builder.Services.AddCors(o =>
{
    o.AddPolicy("_specificOrigin",
        p => p.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("_specificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();