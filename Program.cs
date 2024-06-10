using Confluent.Kafka;
using KafkaLib.Kafka;
using Master.Microservice.Handler;
using Master.Microservice.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string? connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MasterContext>(options => options.UseNpgsql(connString), ServiceLifetime.Scoped);


builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure the HTTP request pipeline.


builder.Services.AddCors(o => {
    o.AddDefaultPolicy(b =>
    {
        b.AllowAnyOrigin();
        b.AllowAnyMethod();
        b.AllowAnyHeader();
    });

});


 
builder.Services.AddKafkaMessageBus();



#region To send the request to handler via MediatR
builder.Services.AddMediatR(typeof(RegionCommandHandler).Assembly);
//builder.Services.AddMediatR(typeof(CountryCommandHandler).Assembly);


#endregion

#region Kafka Producer 
var _configuration = builder.Configuration;
builder.Services.AddKafkaProducer<string, KafkaMessage<RegionRequest>>(p =>
{
    p.BootstrapServers = _configuration["Kafka:BootstrapServers"];
    p.SaslUsername = _configuration["Kafka:SaslUsername"];
    p.SaslPassword = _configuration["Kafka:SaslPassword"];
    p.SecurityProtocol = SecurityProtocol.SaslSsl;
    p.SaslMechanism = SaslMechanism.Plain;
    p.Acks = Acks.All;
    p.Topic = "Region";
 });
builder.Services.AddKafkaProducer<string, KafkaMessage<CountryRequest>>(p =>
{
    p.BootstrapServers = _configuration["Kafka:BootstrapServers"];
    p.SaslUsername = _configuration["Kafka:SaslUsername"];
    p.SaslPassword = _configuration["Kafka:SaslPassword"];
    p.SecurityProtocol = SecurityProtocol.SaslSsl;
    p.SaslMechanism = SaslMechanism.Plain;
    p.Acks = Acks.All;
    p.Topic = "Country";
});

builder.Services.AddKafkaProducer<string, KafkaMessage<CityRequest>>(p =>
{
    p.BootstrapServers = _configuration["Kafka:BootstrapServers"];
    p.SaslUsername = _configuration["Kafka:SaslUsername"];
    p.SaslPassword = _configuration["Kafka:SaslPassword"];
    p.SecurityProtocol = SecurityProtocol.SaslSsl;
    p.SaslMechanism = SaslMechanism.Plain;
    p.Acks = Acks.All;
    p.Topic = "City";
});

//builder.Services.AddKafkaProducer<string, KafkaMessage<CountryRequest>>(p =>
//{
//    p.Topic = "Country";
//    p.BootstrapServers = BootstrapServers;
//});

#endregion




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseStaticFiles();
app.UseRouting();   
app.UseCors();
app.UseAuthorization();


app.MapControllers();

app.Run();
