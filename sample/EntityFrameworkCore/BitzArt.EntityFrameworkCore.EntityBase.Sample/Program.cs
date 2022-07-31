using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var db = builder.Configuration.GetConnectionString("DB");
builder.Services.AddDbContext<MyDbContext>(x => x.UseSqlServer(db));

var app = builder.Build();

app.MapControllers();

app.Run();
