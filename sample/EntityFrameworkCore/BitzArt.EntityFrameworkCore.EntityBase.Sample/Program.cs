using BitzArt.EntityFrameworkCore.EntityBase.Sample.Contexts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var db = builder.Configuration.GetConnectionString("DB");
builder.Services.AddDbContext<MyDbContext>(x =>
    x.UseSqlServer(db, o =>
        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

var app = builder.Build();

app.MapControllers();

app.Run();
