using Retry.CircuitBreaker.Polly;
using Retry.CircuitBreaker.Polly.Handlers;
using Retry.CircuitBreaker.Polly.Policy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<AuthHandler>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
{
    client.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
})
.AddHttpMessageHandler<AuthHandler>()
.AddPolicyHandler(Policy.GetRetryPolicy())
.AddPolicyHandler(Policy.GetCircuitBreakerPolicy());


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
