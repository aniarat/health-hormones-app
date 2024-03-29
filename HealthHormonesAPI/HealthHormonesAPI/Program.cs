using HealthHormonesAPI.Interfaces;
using HealthHormonesAPI.Models;
using HealthHormonesAPI.Repositories;
using HealthHormonesAPI.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("HealthHormonesDb"));

// Register mongoDb configuration as a singleton object
// builder.Services.AddSingleton<IMongoDatabase>(options =>
// {
//     var settings = builder.Configuration.GetSection("HealthHormonesDb").Get<MongoDbSettings>();
//     var client = new MongoClient(settings.ConnectionString);
//     return client.GetDatabase(settings.DatabaseName);
// });
builder.Services.AddSingleton<ISymptomChangeRepository, SymptomChangeRepository>();
builder.Services.AddSingleton<ISymptomRepository, SymptomRepository>();
builder.Services.AddScoped<ISymptomChangeService, SymptomChangeService>();


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


