
namespace Retry.CircuitBreaker.Polly.Handlers
{
    public class AuthHandler: DelegatingHandler
    {
        private readonly ILogger<AuthHandler> logger;
        private readonly IConfiguration config;

        public AuthHandler(ILogger<AuthHandler> logger, IConfiguration config)
        {
            this.logger = logger;
            this.config = config;
        }
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            // Add Custom logic here like adding token to Auth headers etc
            var token = config.GetValue<string>("token");
           
            request.Headers.Add("Authorization", $"Bearer{token}");
            
            logger.LogInformation("Before calling external Api");

            return base.SendAsync(request, cancellationToken);
            
        }
    }
}
