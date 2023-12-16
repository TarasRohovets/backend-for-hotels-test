using BackendHotels.Services;
using BackendHotels.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IProductsService, ProductsService>(client =>
{
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BackendForHotels", "1.0"));
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

var url = builder.Configuration.GetSection("BestBuyAPIUrlBase");
var apiKey = builder.Configuration.GetSection("BestBuyAPIKey");

builder.Services.AddTransient<IProductsService> (sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    httpClient.BaseAddress = new Uri(url.Value);

    return new ProductsService(apiKey.Value, httpClient);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
}
