using System.Collections;
using FluentValidation;
using MediatR;
using Shared.CQRS;

namespace Shared.Behaviors;

public class ValidationBehaviors<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : ICommand<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResult = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        var failures = validationResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        
        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }
        
        return await next();
    }
}