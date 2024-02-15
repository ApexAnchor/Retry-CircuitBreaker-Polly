# Retry-CircuitBreaker-Polly
A sample project to demonstrate Retry and Circuit breaker policy using Microsoft.Extensions.Http.Polly

# Retry and Circuit Breaker Policies with Polly

## Introduction

[Polly](https://github.com/App-vNext/Polly) is a .NET resilience and transient-fault-handling library that helps developers deal with faults in distributed systems. This document focuses on the Retry and Circuit Breaker policies provided by Polly, particularly when used as part of the Microsoft.Extensions.Polly library.

## Installation

Install the `Microsoft.Extensions.Http.Polly` NuGet package to leverage Polly within the context of ASP.NET Core and HttpClient.

```bash
dotnet add package Microsoft.Extensions.Http.Polly
```
Retry policies are designed to handle transient faults by automatically retrying a failed operation. Polly provides a flexible and configurable way to implement retry logic. Below is an example of how to set up a simple retry policy using Microsoft.Extensions.Polly:

```
public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    // Add Retry policy with exponential back off
    return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

}
```

Circuit Breaker policies prevent an application from repeatedly performing an operation that's likely to fail. When a certain threshold of failures is reached, the circuit breaker "opens," and subsequent attempts to perform the operation will fail fast without executing the actual operation. After a specified time, the circuit breaker "closes" and allows the operation to be retried.
```
public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    // Add Circuit breaker policy
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK) // By default only handles 5xx and 408
        .CircuitBreakerAsync(2, TimeSpan.FromSeconds(50));
}
```
Register HttpClient (Typed or Named) with the above policies
```
builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
{
    client.BaseAddress = new Uri("http://api.weatherapi.com/v1/current.json");
})
.AddHttpMessageHandler<AuthHandler>()
.AddPolicyHandler(Policy.GetRetryPolicy())
.AddPolicyHandler(Policy.GetCircuitBreakerPolicy());

```

# Conclusion
Using Retry and Circuit Breaker policies with Polly provides a powerful mechanism to handle transient faults and enhance the resilience of your applications. Adjust the policy configurations based on your specific requirements and failure scenarios.

