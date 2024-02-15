using Polly.Extensions.Http;
using Polly;

namespace Retry.CircuitBreaker.Polly.Policy
{
    public class Policy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Add Retry policy with exponential back off
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK)
                    .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            // Add Circuit breaker policy
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK) // By default only handles 5xx and 408
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(50));
        }
    }
}

