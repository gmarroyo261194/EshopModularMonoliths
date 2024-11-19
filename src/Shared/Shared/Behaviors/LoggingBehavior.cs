using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull

{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handling request={Request} - Response={Response} - Request Data={RequestData}" , typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();
        
        var response = await next();
        
        timer.Stop();
        
        var timeTaken = timer.Elapsed;
        if(timeTaken.Seconds > 3)
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds to complete", typeof(TRequest).Name, timeTaken.Seconds);
        
        logger.LogInformation("[END] Handled request={Request} - Response={Response} - took {TimeTaken}", typeof(TRequest).Name, typeof(TResponse).Name, timeTaken.Seconds);
        
        return response;
    }
}