using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connString = "Datasource=:memory:";
var conn = new SqliteConnection(connString);
conn.Open();

builder.Services.AddDbContext<RoomBookingDbContext>(opt => opt.UseSqlite(conn));

EnsureDataBaseCreated(conn);
static void EnsureDataBaseCreated(SqliteConnection conn)
{
   var builder = new DbContextOptionsBuilder<RoomBookingDbContext>();
    builder.UseSqlite(conn);
    using var context = new RoomBookingDbContext(builder.Options);
    context.Database.EnsureCreated();
}

builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
var app = builder.Build();


builder.Services.AddScoped<IRoomBookingRequestProcessor, RoomBookingRequestProcessor>();

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
