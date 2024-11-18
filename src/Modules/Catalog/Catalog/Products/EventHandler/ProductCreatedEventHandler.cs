namespace Catalog.Products.EventHandler;

public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger) 
    : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event Handler: {DomainEvent}", notification.GetType().Name);
        
        return Task.CompletedTask;
    }
}