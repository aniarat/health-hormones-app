using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using HealthHormonesAPI.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("HealthHormonesDb"));

builder.Services.AddSingleton<IMongoClient>(_ => {
    var connectionString = 
        builder
            .Configuration
            .GetSection("HealthHormonesDb:ConnectionString")?
            .Value;
    return new MongoClient(connectionString);
});
builder.Services.AddSingleton<ISymptomService, SymptomService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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


